# WebSharper.HighlightJS

[![NuGet](https://img.shields.io/nuget/v/WebSharper.HighlightJS.svg)](https://www.nuget.org/packages/WebSharper.HighlightJS)

WebSharper bindings for [highlight.js](https://highlightjs.org/) 11.x — client-side syntax
highlighting for F# and C# web apps.

This repository contains:

| Project | Purpose |
|---|---|
| `WebSharper.Highlightjs` | The binding: typed F#/C# wrappers for highlight.js. |
| `WebSharper.Highlightjs.Sample` | A WebSharper SPA demo that highlights a code string and the page's `<pre><code>` blocks. |

The binding wraps the **core** build of highlight.js, so only the languages and styles
you actually reference are bundled.

## Live demos

- **Binding sample (this repo):** https://dotnet-websharper.github.io/highlightjs/
- **Sample in the WebSharper samples gallery:** https://websharper-samples.github.io/HighlightJS/
- **Sample source in the gallery:** https://github.com/websharper-samples/HighlightJS
- **WebSharper:** https://websharper.com

## Install

```bash
dotnet add package WebSharper.HighlightJS
```

Or with Paket:

```fsharp
nuget WebSharper.HighlightJS
```

The package targets `netstandard2.0` and works with any WebSharper 10 project.

## Usage

All types live in the `WebSharper.HighlightJS` namespace.

### Highlight a string

```fsharp
open WebSharper
open WebSharper.HighlightJS

[<JavaScript>]
let HighlightFSharp (code: string) =
    Hljs.RegisterLanguage("fsharp", Language.Fsharp)
    let result = Hljs.Highlight(code, HighlightOptions("fsharp"))
    result.Value  // HTML string containing <span class="hljs-..."> markup
```

### Highlight the page

Register the languages you use, then highlight every `<pre><code>` block on the page:

```fsharp
open WebSharper.HighlightJS

[<JavaScript>]
let HighlightPage () =
    Styles.Default()                       // bundles highlight.js/styles/default.css
    Hljs.RegisterLanguage("fsharp", Language.Fsharp)
    Hljs.RegisterLanguage("cpp", Language.Cpp)
    Hljs.HighlightAll()
```

To highlight a single element after the fact, call `Hljs.HighlightElement(element)`.

### Language auto-detection

```fsharp
let result = Hljs.HighlightAuto(code, [| "fsharp"; "csharp" |])
let detected = result.Language
```

### Themes

`Styles` exposes the highlight.js stylesheets as static members — calling one includes it:

```fsharp
Styles.Github()
Styles.AtomOneDark()
Styles.Vs2015()
```

### Notes on the API

- `Hljs.Highlight(code, options)` and `Hljs.HighlightAuto(code, languageSubset)` are ordinary
  method calls. (They used to compile to curried calls, so this is the main fix to be aware of.)
- `Hljs.RegisterAliases(aliases, RegisterAliasesOptions(...))` registers extra aliases.
- `Result.Value` is the marked-up HTML string; `Result.Language` and `Result.Relevance`
  describe the match.
- `Language.*` and `Styles.*` are generated for the full highlight.js language/style set and are
  imported on demand from the npm package.

## Build from source

### Prerequisites

- .NET SDK 10 (or any SDK that can build `netstandard2.0`).
- [Node.js](https://nodejs.org/) (LTS) for the esbuild bundle step.
- [Paket](https://fsprojects.github.io/Paket/) as a local dotnet tool (configured in `.config/dotnet-tools.json`).

### Restore and build

```bash
dotnet tool restore
dotnet tool run paket restore
dotnet build WebSharper.Highlightjs.sln -c Release
```

### Run the sample

The sample compiles to static assets in `dist`, so build it and serve that folder:

```bash
dotnet build WebSharper.Highlightjs.Sample/WebSharper.Highlightjs.Sample.fsproj -c Release
python3 -m http.server -d dist 8080
```

Then open <http://localhost:8080>.

### Pack the NuGet package

After a Release build (so the assembly exists in `bin/Release/netstandard2.0`):

```bash
dotnet tool run paket pack ./out
```

### Deployment

Pushing to `master` triggers `.github/workflows/ghpages.yml`, which builds the sample in
Release and publishes `dist` to the `gh-pages` branch via GitHub Pages.

## Repository layout

```
WebSharper.Highlightjs/          # the binding
  Main.fs                        # types + [<assembly: Extension>] registration
WebSharper.Highlightjs.Sample/   # static SPA demo
  Client.fs
  index.html
  esbuild.config.mjs
nuget/                           # Paket pack template
.github/workflows/ghpages.yml    # build + deploy the sample to gh-pages
```

## License

Apache 2.0 — see [LICENSE.md](LICENSE.md).
