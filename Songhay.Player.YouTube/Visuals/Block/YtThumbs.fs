module Songhay.Player.YouTube.Visuals.Block.YtThumbs

open Bolero
open Bolero.Html

open Microsoft.JSInterop
open FsToolkit.ErrorHandling

open Humanizer

open Songhay.Modules.Models
open Songhay.Modules.Bolero.Visuals.Svg

open Songhay.Player.YouTube.Models
open Songhay.Player.YouTube.YtItemUtility

let getYtThumbsCaption (item: YouTubeItem) =
    let limit = 60
    let caption =
        if item.snippet.title.Length > limit then
            $"{item.snippet.title.Substring(0, limit)}…"
        else
            item.snippet.title

    a
        [ attr.href (item.tryGetUri |> Result.valueOr raise) ]
        [ text caption ]

let getYtThumbsTitle (itemsTitle: string option) (items: YouTubeItem[] option) =
    if items.IsNone then
        RawHtml "&#160;"
    else
        if itemsTitle.IsNone then
            let pair = items.Value |> Array.head |> getYtItemsPair
            a [ attr.href (fst pair); attr.target "_blank" ] [ text (snd pair) ]
        else
            text itemsTitle.Value

let ytThumbnailsNode (_: IJSRuntime) (items: YouTubeItem[] option) =
    //jsRuntime.InvokeVoidAsync("console.log", "ytThumbsNode", items) |> ignore

    let toSpan (item: YouTubeItem) =
        let duration =
            match item.tryGetDuration with
            | Ok ts -> ts.ToString() |> text
            | _ -> text ":00"

        span [] [
            a
                [
                    attr.href (item.tryGetUri |> Result.valueOr raise).OriginalString
                    attr.target "_blank"
                    attr.title item.snippet.title
                ]
                [
                    img
                        [
                            attr.src item.snippet.thumbnails.medium.url
                            attr.width item.snippet.thumbnails.medium.width
                            attr.height item.snippet.thumbnails.medium.height
                        ]
                ]
            span [ attr.classes [ "published-at"; "is-size-6" ] ] [ (item.getPublishedAt.Humanize() |> text) ]
            span [ attr.classes [ "caption"; "has-text-weight-semibold"; "is-size-5" ] ] [ (item |> getYtThumbsCaption) ]
            span [ attr.classes [ "duration"; "is-size-6" ] ] [ span [] [ duration ] ]
        ]

    if items.IsNone then
        div
            [ attr.classes [ "has-text-centered"; "loader-container"; "p-6"] ]
            [
                div [ attr.classes [ "image"; "is-128x128"; "loader"; "m-3" ]; attr.title "Loading…" ] []
            ]
    else
        div [] (items.Value |> Array.map(toSpan) |> List.ofArray)

let ytThumbsNode (jsRuntime: IJSRuntime) (itemsTitle: string option) (items: YouTubeItem[] option) =
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
                                [ (itemsTitle, items) ||> getYtThumbsTitle ]
                        ]
                ]
            div
                [ attr.classes [ "video"; "thumbs"; "thumbs-container" ] ]
                [
                    items |> ytThumbnailsNode jsRuntime
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
