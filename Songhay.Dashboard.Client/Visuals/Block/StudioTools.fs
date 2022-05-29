module Songhay.Dashboard.Client.Visuals.Block.StudioTools

open System

open Bolero
open Bolero.Html

open Songhay.Modules.Models
open Songhay.Dashboard.Client
open Songhay.Modules.Bolero.Visuals.Svg

let studioToolsData = [
    (
        DisplayText ".NET API Catalog",
        Uri "https://apisof.net/",
        Alphanumeric "mdi_dotnet_24px"
    )
    (
        DisplayText ".NET Fiddle",
        Uri "https://dotnetfiddle.net/",
        Alphanumeric "mdi_dotnet_24px"
    )
    (
        DisplayText ".NET Reference Source",
        Uri "https://referencesource.microsoft.com/",
        Alphanumeric "mdi_dotnet_24px"
    )
    (
        DisplayText "CamelCase Converter",
        Uri "https://en.toolpage.org/tool/camelcase",
        Alphanumeric "mdi_wrench_24px"
    )
    (
        DisplayText "draw.io",
        Uri "https://www.draw.io/",
        Alphanumeric "mdi_vector_curve_24px"
    )
    (
        DisplayText "feedly OPML API",
        Uri "https://developer.feedly.com/v3/opml/",
        Alphanumeric "mdi_rss_24px"
    )
    (
        DisplayText "fuget.org: pro nuget package browsing",
        Uri "https://www.fuget.org/",
        Alphanumeric "mdi_package_24px"
    )
    (
        DisplayText "Google Search Central",
        Uri "https://developers.google.com/search/",
        Alphanumeric "mdi_google_24px"
    )
    (
        DisplayText "JS Bin",
        Uri "http://jsbin.com/",
        Alphanumeric "mdi_vector_curve_24px"
    )
    (
        DisplayText "JSON Viewer",
        Uri "https://codebeautify.org/jsonviewer",
        Alphanumeric "mdi_json_24px"
    )
    (
        DisplayText "quicktype",
        Uri "https://app.quicktype.io/",
        Alphanumeric "mdi_wrench_24px"
    )
    (
        DisplayText "RegExr",
        Uri "https://regexr.com/",
        Alphanumeric "mdi_regex_24px"
    )
    (
        DisplayText "StackBlitz",
        Uri "https://stackblitz.com/@BryanWilhite",
        Alphanumeric "mdi_code_tags_24px"
    )
    (
        DisplayText "StackEdit",
        Uri "https://stackedit.io/",
        Alphanumeric "mdi_cloud_tags_24px"
    )
    (
        DisplayText "Twitter Publish",
        Uri "https://publish.twitter.com/",
        Alphanumeric "mdi_twitter_24px"
    )
    (
        DisplayText "UNPKG",
        Uri "https://unpkg.com/",
        Alphanumeric "mdi_package_24px"
    )
    (
        DisplayText "vim cheatsheet",
        Uri "http://michael.peopleofhonoronly.com/vim/",
        Alphanumeric "mdi_library_24px"
    )
    (
        DisplayText "Visual Studio Code: Variables Reference",
        Uri "https://code.visualstudio.com/docs/editor/variables-reference/",
        Alphanumeric "mdi_library_24px"
    )
]

let rec studioToolIcon (svgKey: Identifier) =
    if not (svgData.ContainsKey svgKey) then
        RawHtml $"<!-- {nameof studioToolIcon}: {nameof svgKey} `{svgKey}` was not found. -->"
    else
        let svgPathData = svgData[svgKey]

        figure
            [ attr.classes [ "media-left" ] ]
            [
                p
                    [ attr.classes [ "image"; "is-48x48" ]; "aria-hidden" => "true" ]
                    [
                        svgNode (svgViewBoxSquare 24) svgPathData
                    ]
            ]

let toBulmaArticleNode (title: DisplayText, location: Uri, svgKey: Identifier) =
    article
        [ attr.classes [ "tile"; "m-3" ] ]
        [
            studioToolIcon svgKey
            div
                [ attr.classes [ "media-content" ] ]
                [
                    div
                        [ attr.classes [ "content" ] ]
                        [
                            a
                                [
                                    attr.classes [ "title"; "is-5" ]
                                    attr.href location.OriginalString
                                    attr.target "_blank"
                                ]
                                [ text title.Value ]
                        ]
                ]
        ]

let studioToolsNode () =
    let getGroup g =
        div
            [ attr.classes [ "tile"; "is-parent"; "has-background-greys-dark-tone" ] ]
            (g |> List.map toBulmaArticleNode)

    div
        [ attr.classes (App.appBlockChildCssClasses @ [ "notification" ]) ]
        (studioToolsData |> List.chunkBySize 2 |> List.map getGroup)
