module Songhay.Player.YouTube.Visuals.Block.YtThumbs

open Bolero.Html

open Microsoft.JSInterop
open Songhay.Modules.Models
open Songhay.Modules.Bolero.Visuals.Svg

open Songhay.Player.YouTube.Models

let ytThumbsNode (jsRuntime: IJSRuntime) (items: YouTubeItem[] option) =
    jsRuntime.InvokeVoidAsync("console.log", "ytThumbsNode", items) |> ignore
    div
        [ attr.classes [ "rx"; "b-roll" ] ]
        [
            div
                [ attr.classes [ "video"; "thumbs"; "header" ] ]
                [
                    svgNode (svgViewBoxSquare 24) svgData[Identifier.fromString "mdi_youtube_24px"]
                ]
            div
                [ attr.classes [ "video"; "thumbs"; "thumbs-container" ] ]
                [
                    button
                        [ attr.``type`` "button" ]
                        [
                            svgNode (svgViewBoxSquare 24) svgData[Identifier.fromString "mdi_arrow_left_drop_circle_24px"]
                        ]

                    button
                        [ attr.``type`` "button" ]
                        [
                            svgNode (svgViewBoxSquare 24) svgData[Identifier.fromString "mdi_arrow_right_drop_circle_24px"]
                        ]
                ]
        ]
