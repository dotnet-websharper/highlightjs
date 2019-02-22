// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2018 IntelliFactory
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
namespace WebSharper.Highlightjs.Extension

open System
open System.Text.RegularExpressions
open WebSharper
open WebSharper.JavaScript
open WebSharper.InterfaceGenerator

module Definition =

    let Hljs = Class "hljs"


    let Options =
        Pattern.Config "Options"{
            Required = []
            Optional =
            [
                "tabReplace", T<string>
                "useBR", T<bool>
                "classPrefix", T<string>
                "languages", T<string[]>
            ]
        }
    let Result = 
        Pattern.Config "Result" {
            Required = 
                [
                    "language", T<string>
                    "relevance", T<int>
                    "value", T<string>
                    "top", T<string>
                ]
            Optional = []
        }
    let ResultAuto =
        Pattern.Config "ResultAuto" {
            Required =
                [
                    "language", T<string>
                    "relevance", T<int>
                    "value", T<string>
                ]
            Optional = 
            [
                "second_best", T<obj> //?
            ]
        }
    let LanguageDef = Hljs ^-> T<obj>
    Hljs
        |+> Static[
            //Constructor (T<unit> + Options)
            "highlight" => (T<string * string * bool> * !? (* T<obj> *) T<string[]>) ^-> Result //??
            "highlightAuto" => (T<string> * !? T<string[]>) ^-> ResultAuto
            "fixMarkup" => T<string> ^-> T<string>
            "highlightBlock" => T<JavaScript.Dom.Node> ^-> T<unit>
            "configure" => Options ^-> T<unit>
            "initHighlighting" => T<unit> ^-> T<unit>
            "initHighlightingOnLoad" => T<unit> ^-> T<unit>
            "registerLanguage" => (T<string>*LanguageDef) ^-> T<unit>
            "listLanguages" => T<unit> ^-> T<list<string>>
            "getLanguage" => T<string> ^-> T<obj>
        ]|>ignore

  (*  Hljs
        |+> Static[
            Constructor (T<unit> + Options)
        ]
        |+> Instance[
            "highlight" => (T<string * string * bool> * !? (*T<obj> *) T<string[]>) ^-> Result
            "highlightAuto" => (T<string> * !? T<string[]>) ^-> ResultAuto
            "fixMarkup" => T<string> ^-> T<string>
            "highlightBlock" => T<obj> ^-> T<obj>
            "configure" => Options ^-> T<unit>
            "initHighlighting" => T<unit> ^-> T<unit>
            "initHighlightingOnLoad" => T<unit> ^-> T<unit>
            "registerLanguage" => (T<string>*LanguageDef) ^-> T<unit>
            "listLanguages" => T<unit> ^-> T<list<string>>
            "getLanguage" => T<string> ^-> T<obj>
        ]|>ignore *)

    (*
    let Highlight = "highlight" => (T<string * string * bool> * !? (*T<obj> *) T<string[]>) ^-> Result
    let HighlightAuto = "highlightAuto" => (T<string> * !? T<string[]>) ^-> Result //mybe I shoudl use another type here?
    let FixMarkup = "fixMarkup" => T<string> ^-> T<string>
    let HighlightBlock = "highlightBlock" => T<obj> ^-> T<obj> //appliest highlight to a dom block
    let Configure = "configure" => Options ^-> T<unit>
    let InitHighLighting = "initHighlighting" => T<unit> ^-> T<unit>
    let InitHighlightinhOnLoad = "initHighlightingOnLoad" => T<unit> ^-> T<unit>
    let LanguageDef = T<unit> ^-> T<obj>
    let RegisterLanguage = "registerLanguage" => (T<string>*LanguageDef) ^-> T<unit>
    let ListLanguages = "listLanguages" => T<unit> ^-> T<list<string>>
    let GetLanguage = "getLanguage" => T<string> ^-> T<obj> *)

    let baseUrl = sprintf "https://cdnjs.cloudflare.com/ajax/libs/highlight.js/9.14.2"

    let MainJs =    
        Resource "Js" (sprintf "%s/highlight.min.js" baseUrl)
        |> AssemblyWide

    /// Transform eg "foo-bar" into "FooBar"
    let Capitalize s =
        Regex("(^|[-_.]).").Replace(s, fun (m: Match) ->
            Char.ToUpperInvariant(m.Value.[m.Value.Length - 1])
            |> string)

    let Languages =
        [
            "1c"
            "abnf"; "accesslog"; "actionscript"; "ada"; "angelscript"; "apache"; "applescript"; "arcade"; "arduino"; "armasm"; "asciidoc"; "aspectj"; "autohotkey"; "autoit"; "avrasm"; "awk"; "axapta"
            "bash"; "basic"; "bnf"; "brainfuck"
            "cal"; "capnproto"; "ceylon"; "clean"; "clojure-repl"; "clojure"; "cmake"; "coffeescript"; "coq"; "cos"; "cpp"; "crmsh"; "crystal"; "cs"; "csp"; "css"
            "d"; "dart"; "delphi"; "diff"; "django"; "dns"; "dockerfile"; "dos"; "dsconfig"; "dts"; "dust"
            "ebnf"; "elixir"; "elm"; "erb"; "erlang-repl"; "erlang"; "excel"
            "fix"; "flix"; "fortran"; "fsharp"
            "gams"; "gauss"; "gcode"; "gherkin"; "glsl"; "gml"; "go"; "golo"; "gradle"; "groovy"
            "haml"; "handlebars"; "haskell"; "haxe"; "hsp"; "htmlbars"; "http"; "hy"
            "inform7"; "ini"; "irpf90"; "isbl"
            "java"; "javascript"; "jboss-cli"; "json"; "julia-repl"; "julia"
            "kotlin"
            "lasso"; "ldif"; "leaf"; "less"; "lisp"; "livecodeserver"; "livescript"; "llvm"; "lsl"; "lua"
            "makefile"; "markdown"; "mathematica"; "matlab"; "maxima"; "mel"; "mercury"; "mipsasm"; "mizar"; "mojolicious"; "monkey"; "moonscript"
            "n1ql"; "nginx"; "nimrod"; "nix"; "nsis"
            "objectivec"; "ocaml"; "openscad"; "oxygene"
            "parser3"; "perl"; "pf"; "pgsql"; "php"; "plaintext"; "pony"; "powershell"; "processing"; "profile"; "prolog"; "properties"; "protobuf"; "puppet"; "purebasic"; "python"
            "q"; "qml"
            "r"; "reasonml"; "rib"; "roboconf"; "routeros"; "rsl"; "ruby"; "ruleslanguage"; "rust"
            "sas"; "scala"; "scheme"; "scilab"; "scss"; "shell"; "smali"; "smalltalk"; "sml"; "sqf"; "sql"; "stan"; "stata"; "step21"; "stylus"; "subunit"; "swift"
            "taggerscript"; "tap"; "tcl"; "tex"; "thrift"; "tp"; "twig"; "typescript"
            "vala"; "vbnet"; "vbscript-html"; "vbscript"; "verilog"; "vhdl"; "vim"
            "x86asm"; "xl"; "xml"; "xquery"
            "yaml"
            "zephir"
        ]
        |> List.map (fun name ->
            let ident = Capitalize name
            Resource ident (sprintf "%s/languages/%s.min.js" baseUrl name)
            |> Requires [MainJs]
            :> CodeModel.NamespaceEntity
        )

    let Styles =
        [
            "a11y-dark"; "a11y-light"; "agate"; "an-old-hope"; "androidstudio"; "arduino-light"; "arta"; "ascetic"
            "atelier-cave-dark"; "atelier-cave-light"; "atelier-dune-dark"; "atelier-dune-light"; "atelier-estuary-dark"; "atelier-estuary-light"
            "atelier-forest-dark"; "atelier-forest-light"; "atelier-heath-dark"; "atelier-heath-light"; "atelier-lakeside-dark"; "atelier-lakeside-light"
            "atelier-plateau-dark"; "atelier-plateau-light"; "atelier-savanna-dark"; "atelier-savanna-light"; "atelier-seaside-dark"; "atelier-seaside-light"; "atelier-sulphurpool-dark"; "atelier-sulphurpool-light"
            "atom-one-dark-reasonable"; "atom-one-dark"; "atom-one-light"
            "brown-paper"
            "codepen-embed"; "color-brewer"
            "darcula"; "dark"; "darkula"; "default"; "docco"; "dracula"
            "far"; "foundation"
            "github-gist"; "github"; "gml"; "googlecode"; "grayscale"; "gruvbox-dark"; "gruvbox-light"
            "hopscotch"; "hybrid"
            "idea"; "ir-black"; "isbl-editor-dark"; "isbl-editor-light"
            "kimbie.dark"; "kimbie.light"
            "lightfair"
            "magula"; "mono-blue"; "monokai-sublime"; "monokai"
            "nord"
            "obsidian"; "ocean"
            "paraiso-dark"; "paraiso-light"; "pojoaque"; "purebasic"
            "qtcreator_dark"; "qtcreator_light"
            "railscasts"; "rainbow"; "routeros"
            "school-book"; "shades-of-purple"; "solarized-dark"; "solarized-light"; "sunburst"
            "tomorrow-night-blue"; "tomorrow-night-bright"; "tomorrow-night-eighties"; "tomorrow-night"; "tomorrow"
            "vs"; "vs2015"
            "xcode"; "xt256"
            "zenburn"
        ]
        |> List.map (fun name ->
            let ident = Capitalize name
            Resource ident (sprintf "%s/styles/%s.min.css" baseUrl name)
            :> CodeModel.NamespaceEntity
        )

    let Assembly =
        Assembly [
            Namespace "WebSharper.HighlightJS.Resources" [
                MainJs
            ]
            Namespace "WebSharper.HighlightJS.Resources.Languages" Languages
            Namespace "WebSharper.HighlightJS.Resources.Styles" Styles
            Namespace "WebSharper.HighlightJS" [
                Options
                Result
                ResultAuto
                Hljs
            ]
        ]

[<Sealed>]
type Extension() =
    interface IExtension with
        member ext.Assembly =
            Definition.Assembly

[<assembly: Extension(typeof<Extension>)>]
do ()
