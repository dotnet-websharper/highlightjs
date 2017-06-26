namespace WebSharper.Highlightjs

open WebSharper
open WebSharper.JavaScript
open WebSharper.InterfaceGenerator

module Definition =
    let Highlightjs = Class "Highlightjs"
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
    let LanguageDef = T<unit> ^-> T<obj>
    Highlightjs
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
        ]|>ignore

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
    let GetLanguage = "getLanguage" => T<string> ^-> T<obj>
    *)

    let Assembly =
        Assembly [
            Namespace "WebSharper.Highlightjs.Resources" [
                Resource "Js" "https://cdnjs.cloudflare.com/ajax/libs/highlight.js/9.4.0/languages/fsharp.min.js"
                |> AssemblyWide
            ]
            Namespace "WebSharper.Highlightjs" [
                Options
                Result
                ResultAuto
                Highlightjs
            ]
        ]

[<Sealed>]
type Extension() =
    interface IExtension with
        member ext.Assembly =
            Definition.Assembly

[<assembly: Extension(typeof<Extension>)>]
do ()
