module Songhay.Player.YouTube.YtThumbs

open System.Collections.Generic
open Bolero
open Bolero.Html
open Elmish

open Microsoft.AspNetCore.Components.Web
open Microsoft.JSInterop
open FsToolkit.ErrorHandling
open Humanizer

open Songhay.Modules.StringUtility
open Songhay.Modules.Bolero.BoleroUtility
open Songhay.Modules.Bolero.JsRuntimeUtility
open Songhay.Modules.Bolero.Visuals.Svg

open Songhay.Player.YouTube
open Songhay.Player.YouTube.Models
open Songhay.Player.YouTube.YtItemUtility

[<Literal>] // see `$var-thumbs-container-wrapper-left` in `Songhay.Player.YouTube/src/scss/you-tube-css-variables.scss`
let CssVarThumbsContainerWrapperLeft = "--thumbs-container-wrapper-left"

[<Literal>] // see `$thumbnail-margin-right` in `Songhay.Player.YouTube/src/scss/you-tube-thumbs.scss`
let ThumbnailMarginRight = 4

type SlideDirection = | Left | Right

let click = GlobalEventHandlers.OnClick

let getYtThumbsCaption (item: YouTubeItem) =
    let limit = 60
    let caption =
        if item.snippet.title.Length > limit then
            $"{item.snippet.title.Substring(0, limit)}…"
        else
            item.snippet.title

    a
        [ attr.href (item.tryGetUri |> Result.valueOr raise); attr.target "_blank" ]
        [ text caption ]

let getYtThumbsTitle (dispatch: Dispatch<YouTubeMessage>) (_: IJSRuntime)
    (itemsTitle: string option) (model: YouTubeModel) =

    let items = model.YtItems

    if items.IsNone then
        RawHtml "&#160;"
    else
        if itemsTitle.IsNone then
            let pair = items.Value |> Array.head |> getYtItemsPair
            a [ attr.href (fst pair); attr.target "_blank" ] [ text (snd pair) ]
        else
            a
                [
                    attr.href "#" ; attr.title $"{itemsTitle.Value}: show curated YouTube™ channels"
                    click.PreventDefault
                    on.click (fun _ -> YouTubeMessage.OpenYtSetOverlay |> dispatch)
                ]
                [
                    text itemsTitle.Value
                ]

///<remarks>
/// this member is needed to ‘jumpstart’ CSS animations
/// without this member, the first interop with CSS will not function as expected
///</remarks>
let initAsync (initCache: Dictionary<GlobalEventHandlers, bool>) (blockWrapperRef: HtmlRef) (jsRuntime: IJSRuntime) =
    task {
        if not initCache[OnLoad] then
            let! wrapperLeftStr = jsRuntime |> getComputedStylePropertyValueAsync blockWrapperRef "left"

            jsRuntime
            |> setComputedStylePropertyValueAsync blockWrapperRef CssVarThumbsContainerWrapperLeft wrapperLeftStr
            |> ignore

        initCache[OnLoad] <- true
    }

let ytThumbnailsNode (_: IJSRuntime) (blockWrapperRef: HtmlRef) (items: YouTubeItem[] option) =

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

    cond items.IsSome <| function
        | true -> div [ attr.ref blockWrapperRef ] [ forEach items.Value <| toSpan ]
        | false ->
            div
                [ attr.classes [ "has-text-centered"; "loader-container"; "p-6"] ]
                [
                    div [ attr.classes [ "image"; "is-128x128"; "loader"; "m-3" ]; attr.title "Loading…" ] []
                ]

let ytThumbsNode (dispatch: Dispatch<YouTubeMessage>) (jsRuntime: IJSRuntime)
    (initCache: Dictionary<GlobalEventHandlers, bool>) (thumbsContainerRef: HtmlRef) (blockWrapperRef: HtmlRef)
    (itemsTitle: string option) (model: YouTubeModel) =

    let items = model.YtItems
    let slideAsync (direction: SlideDirection) (_: MouseEventArgs) =
        async {
            if items.IsNone then ()
            else
                jsRuntime |> initAsync initCache blockWrapperRef |> ignore
                let! wrapperContainerWidthStr =
                    jsRuntime
                    |> getComputedStylePropertyValueAsync thumbsContainerRef "width"
                    |> Async.AwaitTask
                let! wrapperLeftStr =
                    jsRuntime
                    |> getComputedStylePropertyValueAsync blockWrapperRef "left"
                    |> Async.AwaitTask

                let wrapperContainerWidth = wrapperContainerWidthStr |> toNumericString |> Option.defaultValue "0" |> int
                let wrapperLeft = wrapperLeftStr |> toNumericString |> Option.defaultValue "0" |> int

                let cannotSlideLeft =
                    let itemsHead = items.Value |> Array.head
                    let fixedBlockWidth = itemsHead.snippet.thumbnails.medium.width + ThumbnailMarginRight
                    let totalWidth = fixedBlockWidth * (items.Value |> Array.length)
                    let slideLeftLength = abs(wrapperLeft) + wrapperContainerWidth

                    slideLeftLength >= totalWidth

                let getSlideRightLength =
                    let l = abs wrapperLeft
                    if l > wrapperContainerWidth then wrapperContainerWidth
                    else l

                let nextLeft =
                    match direction with
                    | Left ->
                        if cannotSlideLeft then wrapperLeft
                        else wrapperLeft - wrapperContainerWidth
                    | Right ->
                        if wrapperLeft >= 0 then wrapperLeft
                        else wrapperLeft + getSlideRightLength

                jsRuntime
                |> setComputedStylePropertyValueAsync blockWrapperRef CssVarThumbsContainerWrapperLeft $"{nextLeft}px"
                |> ignore
        }

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
                                [ svgNode (svgViewBoxSquare 24) svgData[Keys.MDI_YOUTUBE_24PX.ToAlphanumeric] ]
                            span
                                [ attr.classes [ "level-item"; "is-size-2" ] ]
                                [ (jsRuntime, itemsTitle, model) |||> getYtThumbsTitle dispatch ]
                        ]
                ]
            div
                [
                    attr.ref thumbsContainerRef
                    attr.classes [ "video"; "thumbs"; "thumbs-container" ]
                ]
                [
                    items |> ytThumbnailsNode jsRuntime blockWrapperRef
                    a
                        [
                            attr.href "#"; attr.classes [ "command"; "left"; "image"; "is-48x48" ]
                            click.PreventDefault
                            on.async.click (slideAsync Right)
                        ]
                        [
                            svgNode (svgViewBoxSquare 24) svgData[Keys.MDI_ARROW_LEFT_DROP_CIRCLE_24PX.ToAlphanumeric]
                        ]
                    a
                        [
                            attr.href "#"; attr.classes [ "command"; "right"; "image"; "is-48x48" ]
                            click.PreventDefault
                            on.async.click (slideAsync Left)
                        ]
                        [
                            svgNode (svgViewBoxSquare 24) svgData[Keys.MDI_ARROW_RIGHT_DROP_CIRCLE_24PX.ToAlphanumeric]
                        ]
                ]
        ]
