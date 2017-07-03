namespace WebSharper.Highlightjs.Sample

open WebSharper
open WebSharper.JavaScript
open WebSharper.JQuery
open WebSharper.UI.Next
open WebSharper.UI.Next.Client
open WebSharper.UI.Next.Templating
open WebSharper.Highlightjs

[<JavaScript>]
module Client =

    [<SPAEntryPoint>]
    let Main () =
        let hljsConfig = new WebSharper.Highlightjs.Options()
        hljsConfig.TabReplace <- "    "
        hljsConfig.UseBR <- false

        Hljs.Configure(hljsConfig)
        WebSharper.Highlightjs.Hljs.InitHighlightingOnLoad()
