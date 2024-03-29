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

    let Hljs =
        Class "hljs"
        |> ImportDefault "highlight.js/lib/core"


    let Options =
        Pattern.Config "Options" {
            Required = []
            Optional =
            [
                "classPrefix", T<string>
                "languages", T<string[]>
                "cssSelector", T<string>
                "ignoreUnescapedHTML", T<bool>
                "throwUnescapedHTML", T<string>
                "languageDetectRe", T<JavaScript.RegExp>
                "noHighlightRe", T<JavaScript.RegExp>
            ]
        }

    let HighlightOptions =
        Pattern.Config "HighlightOptions" {
            Required =
                [
                    "language", T<string>
                ]
            Optional =
                [
                    "ignoreIllegals", T<bool>
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
                    "code", T<string>
                    "illegal", T<bool>
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
                "second_best", TSelf 
            ]
        }

    let Language =
        Class "Language"
    
    Hljs
        |+> Static [
            "configure" => Options ^-> T<unit>
            "highlight" => T<string>?code ^-> HighlightOptions ^-> Result
            "highlightAll" => T<unit> ^-> T<unit>
            "highlightAuto" => T<string>?code ^-> !?T<string> ^-> ResultAuto
            "highlightElement" => T<JavaScript.Dom.Node> ^-> T<unit>
            "listLanguages" => T<unit> ^-> !|T<string>
            "registerLanguage" => (T<string> * Language) ^-> T<unit>
            "unregisterLanguage" => T<string> ^-> T<unit>
            "getLanguage" => T<string> ^-> T<obj>
            "registerAliases" => T<string> + !|T<string> ^-> T<string> ^-> T<unit>
            
            "safeMode" => T<unit> ^-> T<unit>
            "debugMode" => T<unit> ^-> T<unit>

            "versionString" =? T<string>

            // Deprecated features
            "highlightBlock" => T<JavaScript.Dom.Node> ^-> T<unit>
                |> ObsoleteWithMessage "Please use HighlightElement instead"
            "initHighlighting" => T<unit> ^-> T<unit>
                |> ObsoleteWithMessage "Please use HighlightAll instead"
            "initHighlightingOnLoad" => T<unit> ^-> T<unit>
                |> ObsoleteWithMessage "Please use HighlightAll instead"
        ] |> ignore

    /// Transform eg "foo-bar" into "FooBar"
    let Capitalize (s: string) =
        Regex("(^|[-.]).").Replace(s.Replace("/", "_"), fun (m: Match) ->
            Char.ToUpperInvariant(m.Value.[m.Value.Length - 1])
            |> string)

    let _ =
        [
            "1c"
            "abnf"
            "accesslog"
            "actionscript"
            "ada"
            "angelscript"
            "apache"
            "applescript"
            "arcade"
            "arduino"
            "armasm"
            "asciidoc"
            "aspectj"
            "autohotkey"
            "autoit"
            "avrasm"
            "awk"
            "axapta"
            "bash"
            "basic"
            "bnf"
            "brainfuck"
            "c"
            "cal"
            "capnproto"
            "ceylon"
            "clean"
            "clojure-repl"
            "clojure"
            "cmake"
            "coffeescript"
            "coq"
            "cos"
            "cpp"
            "crmsh"
            "crystal"
            "csharp"
            "csp"
            "css"
            "d"
            "dart"
            "delphi"
            "diff"
            "django"
            "dns"
            "dockerfile"
            "dos"
            "dsconfig"
            "dts"
            "dust"
            "ebnf"
            "elixir"
            "elm"
            "erb"
            "erlang-repl"
            "erlang"
            "excel"
            "fix"
            "flix"
            "fortran"
            "fsharp"
            "gams"
            "gauss"
            "gcode"
            "gherkin"
            "glsl"
            "gml"
            "go"
            "golo"
            "gradle"
            "groovy"
            "haml"
            "handlebars"
            "haskell"
            "haxe"
            "hsp"
            "http"
            "hy"
            "inform7"
            "ini"
            "irpf90"
            "isbl"
            "java"
            "javascript"
            "jboss-cli"
            "json"
            "julia-repl"
            "julia"
            "kotlin"
            "lasso"
            "latex"
            "ldif"
            "leaf"
            "less"
            "lisp"
            "livecodeserver"
            "livescript"
            "llvm"
            "lsl"
            "lua"
            "makefile"
            "markdown"
            "mathematica"
            "matlab"
            "maxima"
            "mel"
            "mercury"
            "mipsasm"
            "mizar"
            "mojolicious"
            "monkey"
            "moonscript"
            "n1ql"
            "nestedtext"
            "nginx"
            "nim"
            "nix"
            "node-repl"
            "nsis"
            "objectivec"
            "ocaml"
            "openscad"
            "oxygene"
            "parser3"
            "perl"
            "pf"
            "pgsql"
            "php-template"
            "php"
            "plaintext"
            "pony"
            "powershell"
            "processing"
            "profile"
            "prolog"
            "properties"
            "protobuf"
            "puppet"
            "purebasic"
            "python-repl"
            "python"
            "q"
            "qml"
            "r"
            "reasonml"
            "rib"
            "roboconf"
            "routeros"
            "rsl"
            "ruby"
            "ruleslanguage"
            "rust"
            "sas"
            "scala"
            "scheme"
            "scilab"
            "scss"
            "shell"
            "smali"
            "smalltalk"
            "sml"
            "sqf"
            "sql"
            "stan"
            "stata"
            "step21"
            "stylus"
            "subunit"
            "swift"
            "taggerscript"
            "tap"
            "tcl"
            "thrift"
            "tp"
            "twig"
            "typescript"
            "vala"
            "vbnet"
            "vbscript-html"
            "vbscript"
            "verilog"
            "vhdl"
            "vim"
            "wasm"
            "wren"
            "x86asm"
            "xl"
            "xml"
            "xquery"
            "yaml"
            "zephir"
        ]
        |> List.iter (fun name ->
            Language
            |+> Static [
                name =? TSelf
                |> ImportDefault (sprintf "highlight.js/lib/languages/%s" name)
            ] |> ignore
        )

    let Styles =
        Class "Styles"

    let _ =
        [
            "a11y-dark"
            "a11y-light"
            "agate"
            "an-old-hope"
            "androidstudio"
            "arduino-light"
            "arta"
            "ascetic"
            "atom-one-dark-reasonable"
            "atom-one-dark"
            "atom-one-light"
            "brown-paper"
            "codepen-embed"
            "color-brewer"
            "dark"
            "default"
            "devibeans"
            "docco"
            "far"
            "felipec"
            "foundation"
            "github-dark-dimmed"
            "github-dark"
            "github"
            "gml"
            "googlecode"
            "gradient-dark"
            "gradient-light"
            "grayscale"
            "hybrid"
            "idea"
            "intellij-light"
            "ir-black"
            "isbl-editor-dark"
            "isbl-editor-light"
            "kimbie-dark"
            "kimbie-light"
            "lightfair"
            "lioshi"
            "magula"
            "mono-blue"
            "monokai-sublime"
            "monokai"
            "night-owl"
            "nnfx-dark"
            "nnfx-light"
            "nord"
            "obsidian"
            "paraiso-dark"
            "paraiso-light"
            "panda-syntax-light"
            "panda-syntax-dark"
            "pojoaque"
            "purebasic"
            "qtcreator-dark"
            "qtcreator-light"
            "rainbow"
            "routeros"
            "school-book"
            "shades-of-purple"
            "srcery"
            "stackoverflow-dark"
            "stackoverflow-light"
            "sunburst"
            "tokyo-night-dark"
            "tokyo-night-light"
            "tomorrow-night-blue"
            "tomorrow-night-bright"
            "vs"
            "vs2015"
            "xcode"
            "xt256"
            "base16/3024"
            "base16/apathy"
            "base16/apprentice"
            "base16/ashes"
            "base16/atelier-cave-light"
            "base16/atelier-cave"
            "base16/atelier-dune-light"
            "base16/atelier-dune"
            "base16/atelier-estuary-light"
            "base16/atelier-estuary"
            "base16/atelier-forest-light"
            "base16/atelier-forest"
            "base16/atelier-heath-light"
            "base16/atelier-heath"
            "base16/atelier-lakeside-light"
            "base16/atelier-lakeside"
            "base16/atelier-plateau-light"
            "base16/atelier-plateau"
            "base16/atelier-savanna-light"
            "base16/atelier-savanna"
            "base16/atelier-seaside-light"
            "base16/atelier-seaside"
            "base16/atelier-sulphurpool-light"
            "base16/atelier-sulphurpool"
            "base16/atlas"
            "base16/bespin"
            "base16/black-metal-bathory"
            "base16/black-metal-burzum"
            "base16/black-metal-dark-funeral"
            "base16/black-metal-gorgoroth"
            "base16/black-metal-immortal"
            "base16/black-metal-khold"
            "base16/black-metal-marduk"
            "base16/black-metal-mayhem"
            "base16/black-metal-nile"
            "base16/black-metal-venom"
            "base16/black-metal"
            "base16/brewer"
            "base16/bright"
            "base16/brogrammer"
            "base16/brush-trees-dark"
            "base16/brush-trees"
            "base16/chalk"
            "base16/circus"
            "base16/classic-dark"
            "base16/classic-light"
            "base16/codeschool"
            "base16/colors"
            "base16/cupcake"
            "base16/cupertino"
            "base16/danqing"
            "base16/darcula"
            "base16/dark-violet"
            "base16/darkmoss"
            "base16/darktooth"
            "base16/decaf"
            "base16/default-dark"
            "base16/default-light"
            "base16/dirtysea"
            "base16/dracula"
            "base16/edge-dark"
            "base16/edge-light"
            "base16/eighties"
            "base16/embers"
            "base16/equilibrium-dark"
            "base16/equilibrium-gray-dark"
            "base16/equilibrium-gray-light"
            "base16/equilibrium-light"
            "base16/espresso"
            "base16/eva-dim"
            "base16/eva"
            "base16/flat"
            "base16/framer"
            "base16/fruit-soda"
            "base16/gigavolt"
            "base16/github"
            "base16/google-dark"
            "base16/google-light"
            "base16/grayscale-dark"
            "base16/grayscale-light"
            "base16/green-screen"
            "base16/gruvbox-dark-hard"
            "base16/gruvbox-dark-medium"
            "base16/gruvbox-dark-pale"
            "base16/gruvbox-dark-soft"
            "base16/gruvbox-light-hard"
            "base16/gruvbox-light-medium"
            "base16/gruvbox-light-soft"
            "base16/hardcore"
            "base16/harmonic16-dark"
            "base16/harmonic16-light"
            "base16/heetch-dark"
            "base16/heetch-light"
            "base16/helios"
            "base16/hopscotch"
            "base16/horizon-dark"
            "base16/horizon-light"
            "base16/humanoid-dark"
            "base16/humanoid-light"
            "base16/ia-dark"
            "base16/ia-light"
            "base16/icy-dark"
            "base16/ir-black"
            "base16/isotope"
            "base16/kimber"
            "base16/london-tube"
            "base16/macintosh"
            "base16/marrakesh"
            "base16/materia"
            "base16/material-darker"
            "base16/material-lighter"
            "base16/material-palenight"
            "base16/material-vivid"
            "base16/material"
            "base16/mellow-purple"
            "base16/mexico-light"
            "base16/mocha"
            "base16/monokai"
            "base16/nebula"
            "base16/nord"
            "base16/nova"
            "base16/ocean"
            "base16/oceanicnext"
            "base16/one-light"
            "base16/onedark"
            "base16/outrun-dark"
            "base16/papercolor-dark"
            "base16/papercolor-light"
            "base16/paraiso"
            "base16/pasque"
            "base16/phd"
            "base16/pico"
            "base16/pop"
            "base16/porple"
            "base16/qualia"
            "base16/railscasts"
            "base16/rebecca"
            "base16/ros-pine-dawn"
            "base16/ros-pine-moon"
            "base16/ros-pine"
            "base16/sagelight"
            "base16/sandcastle"
            "base16/seti-ui"
            "base16/shapeshifter"
            "base16/silk-dark"
            "base16/silk-light"
            "base16/snazzy"
            "base16/solar-flare-light"
            "base16/solar-flare"
            "base16/solarized-dark"
            "base16/solarized-light"
            "base16/spacemacs"
            "base16/summercamp"
            "base16/summerfruit-dark"
            "base16/summerfruit-light"
            "base16/synth-midnight-terminal-dark"
            "base16/synth-midnight-terminal-light"
            "base16/tango"
            "base16/tender"
            "base16/tomorrow-night"
            "base16/tomorrow"
            "base16/twilight"
            "base16/unikitty-dark"
            "base16/unikitty-light"
            "base16/vulcan"
            "base16/windows-10-light"
            "base16/windows-10"
            "base16/windows-95-light"
            "base16/windows-95"
            "base16/windows-high-contrast-light"
            "base16/windows-high-contrast"
            "base16/windows-nt-light"
            "base16/windows-nt"
            "base16/woodland"
            "base16/xcode-dusk"
            "base16/zenburn"
        ]
        |> List.iter (fun name ->
            let ident = Capitalize name
            Styles
            |+> Static [
                ident => T<unit> ^-> T<unit>
                |> ImportFile (sprintf "highlight.js/styles/%s.css" name)
            ] |> ignore
            
        )

    let Assembly =
        Assembly [
            Namespace "WebSharper.HighlightJS" [
                Options
                Result
                ResultAuto
                HighlightOptions
                Hljs
                Styles
                Language
            ]
        ]

[<Sealed>]
type Extension() =
    interface IExtension with
        member ext.Assembly =
            Definition.Assembly

[<assembly: Extension(typeof<Extension>)>]
do ()
