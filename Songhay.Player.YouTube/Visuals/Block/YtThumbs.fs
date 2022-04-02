module Songhay.Player.YouTube.Visuals.Block.YtThumbs

open Bolero.Html

open Microsoft.JSInterop
open Songhay.Modules.Models
open Songhay.Modules.Bolero.Visuals.Svg

open Songhay.Player.YouTube.Models
open Songhay.Player.YouTube.YtItemUtility

let getYtThumbsTitle (items: YouTubeItem[] option) =
    if items.IsNone then
        empty
    else
        let pair = items.Value |> Array.head |> getYtItemsPair
        a [ attr.href (fst pair) ] [ text (snd pair) ]

let ytThumbsNode (_: IJSRuntime) (items: YouTubeItem[] option) =
    //jsRuntime.InvokeVoidAsync("console.log", "ytThumbsNode", items) |> ignore

    div
        [ attr.classes [ "rx"; "b-roll" ] ]
        [
            nav
                [ attr.classes [ "level"; "video"; "thumbs"; "header" ] ]
                [
                    div
                        [ attr.classes [ "level-left" ] ]
                        [
                            span
                                [ attr.classes [ "level-item"; "image"; "is-48x48" ] ]
                                [ svgNode (svgViewBoxSquare 24) svgData[Identifier.fromString "mdi_youtube_24px"] ]
                            span
                                [ attr.classes [ "level-item"; "is-size-2" ] ]
                                [ items |> getYtThumbsTitle ]
                        ]
                ]
            div
                [ attr.classes [ "video"; "thumbs"; "thumbs-container" ] ]
                [
                    a
                        [ attr.href "#"; attr.classes [ "command"; "left"; "image"; "is-48x48" ] ]
                        [
                            svgNode (svgViewBoxSquare 24) svgData[Identifier.fromString "mdi_arrow_left_drop_circle_24px"]
                        ]

                    a
                        [ attr.href "#"; attr.classes [ "command"; "right"; "image"; "is-48x48" ] ]
                        [
                            svgNode (svgViewBoxSquare 24) svgData[Identifier.fromString "mdi_arrow_right_drop_circle_24px"]
                        ]
                ]
        ]
