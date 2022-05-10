module Songhay.Player.YouTube.YtThumbsSet

open Microsoft.JSInterop

open Bolero.Html
open Elmish

open Songhay.Modules.Bolero.BoleroUtility
open Songhay.Modules.Bolero.Visuals.Svg
open Songhay.Modules.Models
open Songhay.Player.YouTube
open Songhay.Player.YouTube.Components

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
                                let clientId = ClientId.fromIdentifier display.id
                                if display.displayText.IsSome then
                                    a
                                        [
                                            attr.href "#"
                                            attr.classes [
                                                "dropdown-item"
                                                if clientId = snd model.YtSetIndexSelectedDocument then
                                                    "is-active"
                                            ]
                                            click.PreventDefault
                                            on.click (fun _ ->
                                                CallYtSet (display.displayText.Value, clientId) |> dispatch)
                                        ]
                                        [ text (display.displayText |> Option.get).Value ]
                                else empty
                        ]
                ]
        ]

let ytSetOverlayCloseCommand (dispatch: Dispatch<YouTubeMessage>) =
    a
        [
            attr.classes [ "image"; "is-48x48" ]
            attr.href "#"
            attr.title "close b-roll overlay"
            click.PreventDefault
            on.click (fun _ -> YouTubeMessage.CloseYtSetOverlay |> dispatch)
        ]
        [ svgNode (svgViewBoxSquare 24) svgData[Identifier.fromString "mdi_close_box_24px"] ]

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

    div
        [ attr.classes overlayClasses ]
        [
            cond model.YtSetIndex.IsSome <| function
            | true ->
                nav
                    [ attr.classes [ "level"; "m-2" ] ]
                    [
                        div
                            [ attr.classes [ "level-left" ] ]
                            [
                                div
                                    [ attr.classes [ "level-item"; "is-size-2" ] ]
                                    [ text (fst model.YtSetIndexSelectedDocument).Value ]
                                div
                                    [ attr.classes [ "level-item" ] ]
                                    [ model |> bulmaDropdown dispatch jsRuntime ]
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
                nav
                    [ attr.classes [ "level"; "m-2" ] ]
                    [
                        div
                            [ attr.classes [ "level-right" ] ]
                            [
                                div
                                    [ attr.classes [ "level-item" ] ]
                                    [ ytSetOverlayCloseCommand dispatch ]
                            ]
                    ]

            cond model.YtSet.IsSome <| function
            | true ->
                div
                    [ attr.classes [ "set" ] ]
                    [
                        forEach model.YtSet.Value <| fun (_, items) ->
                            YtThumbsComponent.EComp [] { model with YtItems = Some items } dispatch
                    ]
            | false ->
                div
                    [ attr.classes [ "has-text-centered"; "loader-container"; "p-6"] ]
                    [
                        div [ attr.classes [ "image"; "is-128x128"; "loader"; "m-6" ]; attr.title "Loading…" ] []
                    ]
        ]
