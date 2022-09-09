namespace Songhay.Dashboard.Client.Components.Block

open System

open Bolero.Html

open Songhay.Modules.Models
open Songhay.Modules.Bolero.BoleroUtility
open Songhay.Modules.Bolero.Models
open Songhay.Modules.Bolero.Visuals.Element
open Songhay.Modules.Bolero.Visuals.Bulma.CssClass
open Songhay.Modules.Bolero.Visuals.Bulma.Element
open Songhay.Modules.Bolero.Visuals.Bulma.Layout
open Songhay.Modules.Bolero.Visuals.Svg

open Songhay.Dashboard.Client.Visuals.Colors

module StudioTools =

    let studioToolsData = [
        (
            DisplayText ".NET API Catalog",
            Uri "https://apisof.net/",
            Keys.MDI_DOTNET_24PX.ToAlphanumeric
        )
        (
            DisplayText ".NET Fiddle",
            Uri "https://dotnetfiddle.net/",
            Keys.MDI_DOTNET_24PX.ToAlphanumeric
        )
        (
            DisplayText ".NET Reference Source",
            Uri "https://referencesource.microsoft.com/",
            Keys.MDI_DOTNET_24PX.ToAlphanumeric
        )
        (
            DisplayText "CamelCase Converter",
            Uri "https://en.toolpage.org/tool/camelcase",
            Keys.MDI_WRENCH_24PX.ToAlphanumeric
        )
        (
            DisplayText "draw.io",
            Uri "https://www.draw.io/",
            Keys.MDI_VECTOR_CURVE_24PX.ToAlphanumeric
        )
        (
            DisplayText "feedly OPML API",
            Uri "https://developer.feedly.com/v3/opml/",
            Keys.MDI_RSS_24PX.ToAlphanumeric
        )
        (
            DisplayText "fuget.org: pro nuget package browsing",
            Uri "https://www.fuget.org/",
            Keys.MDI_PACKAGE_24PX.ToAlphanumeric
        )
        (
            DisplayText "Google Search Central",
            Uri "https://developers.google.com/search/",
            Keys.MDI_GOOGLE_24PX.ToAlphanumeric
        )
        (
            DisplayText "JS Bin",
            Uri "http://jsbin.com/",
            Keys.MDI_LANGUAGE_JAVASCRIPT_24PX.ToAlphanumeric
        )
        (
            DisplayText "JSON Viewer",
            Uri "https://codebeautify.org/jsonviewer",
            Keys.MDI_JSON_24PX.ToAlphanumeric
        )
        (
            DisplayText "omatsuri: base64 encoding",
            Uri "https://omatsuri.app/b64-encoding/",
            Keys.MDI_IMAGE_MULTIPLE_24PX.ToAlphanumeric
        )
        (
            DisplayText "quicktype",
            Uri "https://app.quicktype.io/",
            Keys.MDI_WRENCH_24PX.ToAlphanumeric
        )
        (
            DisplayText "RegExr",
            Uri "https://regexr.com/",
            Keys.MDI_REGEX_24PX.ToAlphanumeric
        )
        (
            DisplayText "sharplab.io",
            Uri "https://sharplab.io/",
            Keys.MDI_DOTNET_24PX.ToAlphanumeric
        )
        (
            DisplayText "StackBlitz",
            Uri "https://stackblitz.com/@BryanWilhite",
            Keys.MDI_CODE_TAGS_24PX.ToAlphanumeric
        )
        (
            DisplayText "StackEdit",
            Uri "https://stackedit.io/",
            Keys.MDI_CLOUD_TAGS_24PX.ToAlphanumeric
        )
        (
            DisplayText "Twitter Publish",
            Uri "https://publish.twitter.com/",
            Keys.MDI_TWITTER_24PX.ToAlphanumeric
        )
        (
            DisplayText "UNPKG",
            Uri "https://unpkg.com/",
            Keys.MDI_PACKAGE_24PX.ToAlphanumeric
        )
        (
            DisplayText "vim cheatsheet",
            Uri "http://michael.peopleofhonoronly.com/vim/",
            Keys.MDI_LIBRARY_24PX.ToAlphanumeric
        )
        (
            DisplayText "Visual Studio Code: Variables Reference",
            Uri "https://code.visualstudio.com/docs/editor/variables-reference/",
            Keys.MDI_LIBRARY_24PX.ToAlphanumeric
        )
    ]

    let bulmaMediaLeftNode (svgKey: Identifier) =
        if not (svgData.ContainsKey svgKey) then
            htmlComment
                $"{nameof svgKey} `{svgKey}` was not found."
        else
            let svgPathData = svgData[svgKey]

            bulmaMediaLeft
                NoCssClasses
                (bulmaImageContainer
                    (Square Square48)
                    (svgNode (bulmaIconSvgViewBox Square24) svgPathData))

    let toBulmaMediaNode (title: DisplayText, location: Uri, svgKey: Identifier) =
        bulmaTile
            TileSizeAuto
            NoCssClasses
            [
                bulmaMedia
                    (HasClasses (CssClasses [ m (All, L3) ]))
                    (HasNode (bulmaMediaLeftNode svgKey))
                    [
                        bulmaContent
                            (HasClasses (CssClasses [ m (T, L3) ]))
                            [
                                anchorElement
                                    (HasClasses (CssClasses [ "title"; fontSize Size5 ]))
                                    location
                                    TargetBlank
                                    [ text title.Value ]
                            ]
                    ]
            ]

    let studioToolsNode =
        let getGroup g =
            bulmaTile
                TileSizeAuto
                (HasClasses (CssClasses [ tileIsParent ]))
                [ forEach g <| toBulmaMediaNode ]

        let forEachNode = forEach (studioToolsData |> List.chunkBySize 2) <| getGroup

        bulmaTile
            TileSizeAuto
            (HasClasses (CssClasses [ tileIsChild; notification; bulmaBackgroundGreyDarkTone ]))
            [ forEachNode ]
