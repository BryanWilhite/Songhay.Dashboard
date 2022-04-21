module Songhay.Player.YouTube.Visuals.Block.YtThumbsSet

open Microsoft.JSInterop

open Bolero
open Bolero.Html
open Elmish

open Songhay.Modules.Bolero.JsRuntimeUtility
open Songhay.Player.YouTube
open Songhay.Player.YouTube.Visuals.Block.YtThumbsNavigation

[<Literal>] // see `Songhay.Player.YouTube/src/scss/you-tube-thumbs-set.scss`
let CssAnimationNameBRollOverlayFadeIn = "b-roll-overlay-fade-in"

[<Literal>] // see `Songhay.Player.YouTube/src/scss/you-tube-thumbs-set.scss`
let CssAnimationNameBRollOverlayFadeOut = "b-roll-overlay-fade-out"

[<Literal>] // see `$var-thumbs-overlay-animation-name` in `Songhay.Player.YouTube/src/scss/you-tube-thumbs-set.scss`
let CssVarThumbsOverlayAnimationName = "--thumbs-overlay-animation-name"

let ytThumbsSetNode (dispatch: Dispatch<YouTubeMessage>) (jsRuntime: IJSRuntime) (thumbsSetContainerRef: HtmlRef) (model: YouTubeModel) =
    task {
        if model.YtSetIsRequested then
            let! playState = jsRuntime |> getComputedStylePropertyValueAsync thumbsSetContainerRef "animation-play-state"
            jsRuntime |> consoleLogAsync [| "ytThumbsSetNode:"; {| playState = playState |} |] |> ignore

            jsRuntime
            |> setComputedStylePropertyValueAsync
                thumbsSetContainerRef
                CssVarThumbsOverlayAnimationName
                CssAnimationNameBRollOverlayFadeIn
            |> ignore
    }
    |> ignore

    div
        [ attr.classes [ "rx"; "b-roll"; "overlay" ] ; attr.ref thumbsSetContainerRef ]
        [
            cond (model.YtSet.IsSome && model.YtSetIndex.IsSome) <| function
            | true ->
                nav
                    [ attr.classes [ "level" ] ]
                    [
                        div
                            [ attr.classes [ "level-left" ] ]
                            [
                                div [ attr.classes [ "level-item" ] ] [ model |> bulmaDropdown dispatch jsRuntime ]
                            ]
                        div
                            [ attr.classes [ "level-right" ] ]
                            [
                                div [ attr.classes [ "level-item" ] ] [ ytSetOverlayCloseCommand jsRuntime ]
                            ]
                    ]
            | false ->
                div
                    [ attr.classes [ "has-text-centered"; "loader-container"; "p-6"] ]
                    [
                        div [ attr.classes [ "image"; "is-128x128"; "loader"; "m-6" ]; attr.title "Loading…" ] []
                    ]
        ]
