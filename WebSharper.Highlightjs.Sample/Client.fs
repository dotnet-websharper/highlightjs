// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2026 IntelliFactory
//
// Licensed under the Apache License, Version 2.0 (the "License"); you
// may not use this file except in compliance with the License.  You may
// obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
// implied.  See the License for the specific language governing
// permissions and limitations under the License.
//
// $end{copyright}
namespace WebSharper.Highlightjs.Sample

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open WebSharper.UI.Notation
open WebSharper.UI.Templating
open WebSharper.HighlightJS

[<JavaScript>]
module Client =

    // The whole document is the template, loaded from the DOM.
    type IndexTemplate = Template<"index.html", ClientLoad.FromDocument>

    /// `$0 == null` is true for both `null` and `undefined`, so it catches the
    /// optional values highlight.js returns.
    [<Inline "$0 == null">]
    let private isNullish (x: obj) : bool = X<bool>

    let private fsharpSource =
        """let rec fib n =
    if n < 2 then n
    else fib (n - 1) + fib (n - 2)

[<EntryPoint>]
let main _ =
    printfn "fib 10 = %d" (fib 10)
    0
"""

    /// Languages offered by the demo, mapped onto the binding's generated
    /// `Language.*` statics. `graphql` and `freedesktop` were added in 11.12.
    let private demoLanguages =
        [
            "fsharp", Language.Fsharp
            "csharp", Language.Csharp
            "cpp", Language.Cpp
            "graphql", Language.Graphql
            "freedesktop", Language.Freedesktop
            "json", Language.Json
            "xml", Language.Xml
            "typescript", Language.Typescript
        ]

    let private languageOf (name: string) =
        demoLanguages |> List.tryPick (fun (n, l) -> if n = name then Some l else None)

    // ------------------------------------------------------------------
    // State
    // ------------------------------------------------------------------

    // Shared with the bootstrap script in index.html.
    let themeStorageKey = "websharper-highlightjs-theme"
    let theme = Var.Create "light"
    let themeLabel = Var.Create "🌙 Dark mode"

    let notice = Var.Create ""
    let version = Var.Create "-"

    // highlight() a string.
    let source = Var.Create fsharpSource
    let language = Var.Create "fsharp"
    let ignoreIllegals = Var.Create false
    let highlighted = Var.Create Doc.Empty
    let resultLanguage = Var.Create "-"
    let resultRelevance = Var.Create "-"
    let resultIllegal = Var.Create "-"

    // highlightAuto().
    let autoSubset = Var.Create "fsharp,csharp,cpp,graphql"
    let autoHighlighted = Var.Create Doc.Empty
    let autoLanguage = Var.Create "-"
    let autoRelevance = Var.Create "-"
    let autoSecondBest = Var.Create "-"

    // highlightAll() / highlightElement().
    let pageStatus = Var.Create "Press a button to highlight the page."
    let private singleCode : Elt =
        Doc.Element "code" [attr.``class`` "language-fsharp"] [text fsharpSource]
    let private singleTarget : Elt =
        Doc.Element "pre" [attr.``class`` "hljs"] [ singleCode :> Doc ]

    // The language registry.
    let registryStatus = Var.Create ""
    let registered = Var.Create ([] : string list)
    let registryLanguage = Var.Create "graphql"

    // configure().
    let cssSelector = Var.Create "#page-demo code"
    let classPrefix = Var.Create "hljs-"
    let configureStatus = Var.Create ""

    // ------------------------------------------------------------------
    // Helpers
    // ------------------------------------------------------------------

    let applyTheme () =
        JS.Document.DocumentElement.SetAttribute("data-theme", theme.Value)
        themeLabel.Value <-
            if theme.Value = "dark" then "☀️ Light mode" else "🌙 Dark mode"

    let toggleTheme () =
        theme.Value <- (if theme.Value = "dark" then "light" else "dark")
        try JS.Window.LocalStorage.SetItem(themeStorageKey, theme.Value) with _ -> ()
        applyTheme ()

    let private hljsBlock (html: string) = Doc.Verbatim html

    let private refreshLanguages () =
        registered.Value <- Hljs.ListLanguages() |> List.ofArray

    let private demoOptions () =
        let opts = HighlightOptions(language.Value)
        opts.IgnoreIllegals <- ignoreIllegals.Value
        opts

    // ------------------------------------------------------------------
    // highlight / highlightAuto
    // ------------------------------------------------------------------

    let highlightString () =
        let result = Hljs.Highlight(source.Value, demoOptions ())
        highlighted.Value <- hljsBlock result.Value
        resultLanguage.Value <- result.Language
        resultRelevance.Value <- string result.Relevance
        resultIllegal.Value <- (if result.Illegal then "yes" else "no")

    let highlightAuto () =
        let subset =
            autoSubset.Value.Split(',')
            |> Array.map (fun s -> s.Trim())
            |> Array.filter (fun s -> s <> "")
        let result = Hljs.HighlightAuto(source.Value, subset)
        autoHighlighted.Value <- hljsBlock result.Value
        autoLanguage.Value <- result.Language
        autoRelevance.Value <- string result.Relevance
        let second = result.Second_best
        autoSecondBest.Value <-
            if isNullish second then "-"
            else sprintf "%s (%d)" second.Language second.Relevance

    // ------------------------------------------------------------------
    // highlightAll / highlightElement
    // ------------------------------------------------------------------

    let highlightAll () =
        Hljs.HighlightAll()
        pageStatus.Value <-
            sprintf "highlightAll() ran over \"%s\""
                (if cssSelector.Value = "" then "pre code" else cssSelector.Value)

    let highlightElement () =
        Hljs.HighlightElement(singleCode.Dom)
        pageStatus.Value <- "highlightElement() ran on the <pre><code> below."

    // ------------------------------------------------------------------
    // Language registry
    // ------------------------------------------------------------------

    let registerLanguage () =
        let name = registryLanguage.Value
        match languageOf name with
        | Some lang ->
            Hljs.RegisterLanguage(name, lang)
            registryStatus.Value <-
                sprintf "registerLanguage(\"%s\", Language.%s)" name (name.Substring(0, 1).ToUpper() + name.Substring(1))
        | None ->
            registryStatus.Value <- sprintf "no Language.%s static in the binding" name
        refreshLanguages ()

    let unregisterLanguage () =
        let name = registryLanguage.Value
        Hljs.UnregisterLanguage(name)
        registryStatus.Value <- sprintf "unregisterLanguage(\"%s\")" name
        refreshLanguages ()

    let getLanguage () =
        let name = registryLanguage.Value
        let lang = Hljs.GetLanguage(name)
        registryStatus.Value <-
            if isNullish lang
            then sprintf "getLanguage(\"%s\") -> undefined" name
            else sprintf "getLanguage(\"%s\") -> a registered definition" name

    let registerAliases () =
        let opts = RegisterAliasesOptions()
        opts.LanguageName <- "fsharp"
        Hljs.RegisterAliases([| "fs"; "f#" |], opts)
        registryStatus.Value <- "registerAliases([ \"fs\"; \"f#\" ], { languageName: \"fsharp\" })"

    // ------------------------------------------------------------------
    // configure
    // ------------------------------------------------------------------

    let configure () =
        let opts = Options()
        opts.CssSelector <- cssSelector.Value
        opts.ClassPrefix <- classPrefix.Value
        Hljs.Configure(opts)
        configureStatus.Value <-
            sprintf "configure({ cssSelector: \"%s\", classPrefix: \"%s\" })"
                cssSelector.Value classPrefix.Value

    // ------------------------------------------------------------------
    // Entry point
    // ------------------------------------------------------------------

    [<SPAEntryPoint>]
    let Main () =
        try
            let stored = JS.Window.LocalStorage.GetItem(themeStorageKey)
            if stored ===. "dark" || stored ===. "light" then theme.Value <- stored
        with _ -> ()
        applyTheme ()

        // Linking a highlight.js theme: the call bundles that stylesheet (the
        // sample uses the light theme and overrides the token colours in
        // site.css for dark mode).
        Styles.AtomOneLight()

        version.Value <- Hljs.VersionString

        // Register the demo languages, then keep highlightAll() scoped to the
        // page demo block (the output panes are ours).
        demoLanguages |> List.iter (fun (name, lang) -> Hljs.RegisterLanguage(name, lang))
        refreshLanguages ()
        configure ()

        // Seed the "highlight a string" pane with one call.
        highlightString ()

        IndexTemplate()
            .notice(notice.V)
            .version(version.V)
            .themeLabel(themeLabel.V)
            .Source(source)
            .Language(language)
            .IgnoreIllegals(ignoreIllegals)
            .HighlightString(fun _ -> highlightString ())
            .resultLanguage(resultLanguage.V)
            .resultRelevance(resultRelevance.V)
            .resultIllegal(resultIllegal.V)
            .Highlighted(Doc.EmbedView highlighted.View)
            .AutoSubset(autoSubset)
            .HighlightAutoDemo(fun _ -> highlightAuto ())
            .autoLanguage(autoLanguage.V)
            .autoRelevance(autoRelevance.V)
            .autoSecondBest(autoSecondBest.V)
            .AutoHighlighted(Doc.EmbedView autoHighlighted.View)
            .HighlightAllDemo(fun _ -> highlightAll ())
            .HighlightElementDemo(fun _ -> highlightElement ())
            .pageStatus(pageStatus.V)
            .SingleTarget(singleTarget)
            .languageCount(registered.View |> View.Map (fun l -> string l.Length))
            .LanguageChips(
                Doc.BindSeqCached
                    (fun (s: string) -> li [attr.``class`` "chip"] [text s])
                    registered.View)
            .RegistryLanguage(registryLanguage)
            .RegisterLanguageDemo(fun _ -> registerLanguage ())
            .UnregisterLanguageDemo(fun _ -> unregisterLanguage ())
            .GetLanguageDemo(fun _ -> getLanguage ())
            .RegisterAliasesDemo(fun _ -> registerAliases ())
            .registryStatus(registryStatus.V)
            .CssSelector(cssSelector)
            .ClassPrefix(classPrefix)
            .ConfigureDemo(fun _ -> configure ())
            .configureStatus(configureStatus.V)
            .ToggleTheme(fun _ -> toggleTheme ())
            .Bind()
