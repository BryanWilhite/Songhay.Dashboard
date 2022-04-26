module Songhay.Player.YouTube.Visuals.Block.YtThumbsSet

open Microsoft.JSInterop

open Bolero
open Bolero.Html
open Elmish

open Songhay.Modules.Bolero.BoleroUtility
open Songhay.Modules.Bolero.JsRuntimeUtility
open Songhay.Modules.Models
open Songhay.Player.YouTube

[<Literal>] // see `Songhay.Player.YouTube/src/scss/you-tube-thumbs-set.scss`
let CssAnimationNameBRollOverlayFadeIn = "b-roll-overlay-fade-in"

[<Literal>] // see `Songhay.Player.YouTube/src/scss/you-tube-thumbs-set.scss`
let CssAnimationNameBRollOverlayFadeOut = "b-roll-overlay-fade-out"

[<Literal>] // see `$var-thumbs-overlay-animation-name` in `Songhay.Player.YouTube/src/scss/you-tube-thumbs-set.scss`
let CssVarThumbsOverlayAnimationName = "--thumbs-overlay-animation-name"

let click = GlobalEventHandlers.OnClick

let bulmaDropdown (dispatch: Dispatch<YouTubeMessage>) (_: IJSRuntime) (model: YouTubeModel) =
    let _, segmentName, documents = model.YtSetIndex.Value
    let dropdownClassesDefault = [ "dropdown" ]
    let dropdownClasses =
        if model.YtSetRequestSelection then dropdownClassesDefault @ [ "is-active" ]
        else dropdownClassesDefault

    div
        [ attr.classes dropdownClasses ]
        [
            div
                [ attr.classes [ "dropdown-trigger" ] ]
                [
                    button
                        [
                            attr.classes [ "button" ]; "aria-haspopup" => "true"; "aria-controls" => "dropdown-menu"
                            on.click (fun _ -> SelectYtSet |> dispatch)
                        ]
                        [
                            span [] [ text $"{segmentName.Value}" ]
                        ]
                ]
            div
                [ attr.classes [ "dropdown-menu" ]; "role" => "menu" ]
                [
                    div
                        [ attr.classes [ "dropdown-content" ] ]
                        [
                            forEach documents <| fun (display, _) ->
                                if display.displayText.IsSome then
                                    a
                                        [
                                            attr.href "#"; attr.classes [ "dropdown-item" ]
                                            click.PreventDefault
                                            on.click (fun _ -> CallYtSet (ClientId.fromIdentifier display.id) |> dispatch)
                                        ]
                                        [ text (display.displayText |> Option.get).Value ]
                                else empty
                        ]
                ]
        ]

let ytSetOverlayCloseCommand (jsRuntime: IJSRuntime) (thumbsSetContainerRef: HtmlRef) =
    a
        [
            attr.href "#"
            click.PreventDefault
            on.async.click (fun _ ->
                async {
                    jsRuntime
                    |> setComputedStylePropertyValueAsync
                        thumbsSetContainerRef
                        CssVarThumbsOverlayAnimationName
                        CssAnimationNameBRollOverlayFadeOut
                    |> ignore
                }
            )
        ]
        [ text "×" ]

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
        [ attr.classes [ "rx"; "b-roll"; "overlay" ]; attr.ref thumbsSetContainerRef ]
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
                                div
                                    [ attr.classes [ "level-item" ] ]
                                    [ ytSetOverlayCloseCommand jsRuntime thumbsSetContainerRef ]
                            ]
                    ]
            | false ->
                div
                    [ attr.classes [ "has-text-centered"; "loader-container"; "p-6"] ]
                    [
                        div [ attr.classes [ "image"; "is-128x128"; "loader"; "m-6" ]; attr.title "Loading…" ] []
                    ]
        ]
