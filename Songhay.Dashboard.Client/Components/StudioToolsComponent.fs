namespace Songhay.Dashboard.Client.Components

open System

open Bolero
open Bolero.Html

open Songhay.Modules.Models
open Songhay.Modules.Bolero.Models
open Songhay.Modules.Bolero.Visuals.BodyElement
open Songhay.Modules.Bolero.Visuals.SvgElement
open Songhay.Modules.Bolero.Visuals.Bulma.CssClass
open Songhay.Modules.Bolero.Visuals.Bulma.Element
open Songhay.Modules.Bolero.Visuals.Bulma.Layout

open Songhay.Dashboard.Client.App.Colors

type StudioToolsComponent() =
    inherit Component()

    let studioToolsData = [
        (
            DisplayText ".NET API Catalog",
            Uri "https://apisof.net/",
            SonghaySvgKeys.MDI_DOTNET_24PX.ToAlphanumeric
        )
        (
            DisplayText ".NET Fiddle",
            Uri "https://dotnetfiddle.net/",
            SonghaySvgKeys.MDI_DOTNET_24PX.ToAlphanumeric
        )
        (
            DisplayText ".NET Reference Source",
            Uri "https://referencesource.microsoft.com/",
            SonghaySvgKeys.MDI_DOTNET_24PX.ToAlphanumeric
        )
        (
            DisplayText "CamelCase Converter",
            Uri "https://en.toolpage.org/tool/camelcase",
            SonghaySvgKeys.MDI_WRENCH_24PX.ToAlphanumeric
        )
        (
            DisplayText "draw.io",
            Uri "https://www.draw.io/",
            SonghaySvgKeys.MDI_VECTOR_CURVE_24PX.ToAlphanumeric
        )
        (
            DisplayText "feedly OPML API",
            Uri "https://developer.feedly.com/v3/opml/",
            SonghaySvgKeys.MDI_RSS_24PX.ToAlphanumeric
        )
        (
            DisplayText "fuget.org: pro nuget package browsing",
            Uri "https://www.fuget.org/",
            SonghaySvgKeys.MDI_PACKAGE_24PX.ToAlphanumeric
        )
        (
            DisplayText "Google Search Central",
            Uri "https://developers.google.com/search/",
            SonghaySvgKeys.MDI_GOOGLE_24PX.ToAlphanumeric
        )
        (
            DisplayText "JS Bin",
            Uri "http://jsbin.com/",
            SonghaySvgKeys.MDI_LANGUAGE_JAVASCRIPT_24PX.ToAlphanumeric
        )
        (
            DisplayText "JSON Viewer",
            Uri "https://codebeautify.org/jsonviewer",
            SonghaySvgKeys.MDI_JSON_24PX.ToAlphanumeric
        )
        (
            DisplayText "omatsuri: base64 encoding",
            Uri "https://omatsuri.app/b64-encoding/",
            SonghaySvgKeys.MDI_IMAGE_MULTIPLE_24PX.ToAlphanumeric
        )
        (
            DisplayText "quicktype",
            Uri "https://app.quicktype.io/",
            SonghaySvgKeys.MDI_WRENCH_24PX.ToAlphanumeric
        )
        (
            DisplayText "RegExr",
            Uri "https://regexr.com/",
            SonghaySvgKeys.MDI_REGEX_24PX.ToAlphanumeric
        )
        (
            DisplayText "sharplab.io",
            Uri "https://sharplab.io/",
            SonghaySvgKeys.MDI_DOTNET_24PX.ToAlphanumeric
        )
        (
            DisplayText "StackBlitz",
            Uri "https://stackblitz.com/@BryanWilhite",
            SonghaySvgKeys.MDI_CODE_TAGS_24PX.ToAlphanumeric
        )
        (
            DisplayText "StackEdit",
            Uri "https://stackedit.io/",
            SonghaySvgKeys.MDI_CLOUD_TAGS_24PX.ToAlphanumeric
        )
        (
            DisplayText "Twitter Publish",
            Uri "https://publish.twitter.com/",
            SonghaySvgKeys.MDI_TWITTER_24PX.ToAlphanumeric
        )
        (
            DisplayText "UNPKG",
            Uri "https://unpkg.com/",
            SonghaySvgKeys.MDI_PACKAGE_24PX.ToAlphanumeric
        )
        (
            DisplayText "vim cheatsheet",
            Uri "http://michael.peopleofhonoronly.com/vim/",
            SonghaySvgKeys.MDI_LIBRARY_24PX.ToAlphanumeric
        )
        (
            DisplayText "Visual Studio Code: Variables Reference",
            Uri "https://code.visualstudio.com/docs/editor/variables-reference/",
            SonghaySvgKeys.MDI_LIBRARY_24PX.ToAlphanumeric
        )
    ]

    let bulmaMediaLeftNode (svgKey: Identifier) =
        if not (SonghaySvgData.HasKey svgKey) then
            htmlComment
                $"{nameof svgKey} `{svgKey}` was not found."
        else
            let svgPathData = SonghaySvgData.Get(svgKey)

            bulmaMediaLeft
                (HasClasses <| CssClasses [ ShadeGreyDark.TextCssClass ])
                NoAttr
                (bulmaImageContainer
                    (Square Square48)
                    NoAttr
                    (svgElement (bulmaIconSvgViewBox Square24) svgPathData))

    let toBulmaMediaNode (title: DisplayText, location: Uri, svgKey: Identifier) =
        bulmaTile
            HSizeAuto
            NoCssClasses
            (bulmaMedia
                (HasClasses <| CssClasses [ m (All, L3) ])
                (HasNode <| bulmaMediaLeftNode svgKey)
                (bulmaContent
                    (HasClasses <| CssClasses [ m (T, L3) ])
                    (anchorElement
                        (HasClasses <| CssClasses [ "title"; fontSize Size5 ])
                        location
                        TargetBlank
                        NoAttr
                        (text title.Value)
                        )))

    let studioToolsNode =
        let getGroup g =
            bulmaTile
                HSizeAuto
                (HasClasses <| CssClasses [ tileIsParent ])
                (forEach g <| toBulmaMediaNode)

        let forEachNode = forEach (studioToolsData |> List.chunkBySize 2) <| getGroup

        bulmaTile
            HSizeAuto
            (HasClasses <| CssClasses [ tileIsChild; notification; bulmaBackgroundGreyDarkTone ])
            forEachNode

    static member BComp = comp<StudioToolsComponent> { attr.empty() }

    override this.Render() =
        studioToolsNode
