#load "tools/includes.fsx"
open IntelliFactory.Build

let bt =
    BuildTool().PackageId("Zafir.HighlightJS")
        .VersionFrom("Zafir")
        .WithFSharpVersion(FSharpVersion.FSharp30)
        .WithFramework(fun f -> f.Net40)

let main =
    bt.Zafir.Extension("WebSharper.Highlightjs")
        .SourcesFromProject()
        .Embed([])
        .References(fun r -> [])

let tests =
    bt.Zafir.SiteletWebsite("WebSharper.Highlightjs.Tests")
        .SourcesFromProject()
        .Embed([])
        .References(fun r ->
            [
                r.Project(main)
                r.NuGet("Zafir.Testing").Latest(true).Reference()
                r.NuGet("Zafir.UI.Next").Latest(true).Reference()
            ])

bt.Solution [
    main
    tests

    bt.NuGet.CreatePackage()
        .Configure(fun c ->
            { c with
                Title = Some "WebSharper.HighlightJS"
                LicenseUrl = Some "http://websharper.com/licensing"
                ProjectUrl = Some "https://github.com/intellifactory/https://github.com/intellifactory/websharper.highlightjs"
                Description = "WebSharper.HighlightJS"
                RequiresLicenseAcceptance = true })
        .Add(main)
]
|> bt.Dispatch
