module Songhay.Player.YouTube.Visuals.Block.YtThumbsSet

open Microsoft.JSInterop

open Bolero.Html
open Elmish

open Songhay.Modules.Bolero.BoleroUtility
open Songhay.Modules.Bolero.JsRuntimeUtility
open Songhay.Modules.Models
open Songhay.Player.YouTube

let click = GlobalEventHandlers.OnClick

let bulmaDropdown (dispatch: Dispatch<YouTubeMessage>) (_: IJSRuntime) (model: YouTubeModel) =
    let _, segmentName, documents = model.YtSetIndex.Value

    let dropdownClasses = [
        "dropdown"
        if model.YtSetRequestSelection then "is-active"
    ]

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

let ytSetOverlayCloseCommand (dispatch: Dispatch<YouTubeMessage>) =
    a
        [
            attr.href "#"
            click.PreventDefault
            on.click (fun _ -> YouTubeMessage.CloseYtSetOverlay |> dispatch)
        ]
        [ text "×" ]

let ytThumbsSetNode (dispatch: Dispatch<YouTubeMessage>) (jsRuntime: IJSRuntime) (model: YouTubeModel) =
    let overlayClasses =
        [
            "rx"
            "b-roll"
            "overlay"
            match model.YtSetOverlayIsVisible with
            | None -> ()
            | Some b ->
                "animate"
                if b then
                    "fade-in"
                else
                    "fade-out"
        ]

    task { jsRuntime |> consoleLogAsync [| {| overlayClasses = overlayClasses |} |] |> ignore } |> ignore

    div
        [ attr.classes overlayClasses ]
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
                                    [ ytSetOverlayCloseCommand dispatch ]
                            ]
                    ]
            | false ->
                div
                    [ attr.classes [ "has-text-centered"; "loader-container"; "p-6"] ]
                    [
                        div [ attr.classes [ "image"; "is-128x128"; "loader"; "m-6" ]; attr.title "Loading…" ] []
                    ]
        ]
