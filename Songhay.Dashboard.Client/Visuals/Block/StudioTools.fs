module Songhay.Dashboard.Client.Visuals.Block.StudioTools

open Bolero
open Bolero.Html

open Songhay.Dashboard.Client
open Songhay.Dashboard.Client.Visuals.Svg

let studioToolsData = [
    (".NET API Catalog", "https://apisof.net/", "mdi_dotnet_24px")
    (".NET Reference Source", "https://referencesource.microsoft.com/", "mdi_dotnet_24px")
    ("CamelCase Converter", "https://en.toolpage.org/tool/camelcase", "mdi_wrench_24px")
    ("draw.io", "https://www.draw.io/", "mdi_vector_curve_24px")
    ("feedly OPML API", "https://developer.feedly.com/v3/opml/", "mdi_rss_24px")
    ("fuget.org: pro nuget package browsing", "https://www.fuget.org/", "mdi_package_24px")
    ("Google Search Central", "https://developers.google.com/search/", "mdi_google_24px")
    ("JS Bin", "http://jsbin.com/", "mdi_vector_curve_24px")
    ("JSON Viewer", "https://codebeautify.org/jsonviewer", "mdi_json_24px")
    ("quicktype", "https://app.quicktype.io/", "mdi_wrench_24px")
    ("RegExr", "https://regexr.com/", "mdi_regex_24px")
    ("StackBlitz", "https://stackblitz.com/@BryanWilhite", "mdi_code_tags_24px")
    ("StackEdit", "https://stackedit.io/", "mdi_cloud_tags_24px")
    ("Twitter Publish", "https://publish.twitter.com/", "mdi_twitter_24px")
    ("UNPKG", "https://unpkg.com/", "mdi_package_24px")
    ("vim cheatsheet", "http://michael.peopleofhonoronly.com/vim/", "mdi_library_24px")
    ("Visual Studio Code: Variables Reference", "https://code.visualstudio.com/docs/editor/variables-reference/", "mdi_library_24px")
]

let rec studioToolIcon (svgKey: string) =
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

let toBulmaArticleNode (title: string, location: string, svgKey: string) =
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
                                    attr.href location
                                ]
                                [ text title ]
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
