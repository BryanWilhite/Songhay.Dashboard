module Songhay.Player.YouTube.Visuals.Block.YtThumbsNavigation

open Microsoft.JSInterop

open Bolero.Html

open Songhay.Player.YouTube

let bulmaDropdown (jsRuntime: IJSRuntime) (model: YouTubeModel) =
    let clientId, segmentName, documents = model.YtSetIndex.Value

    div
        [ attr.classes [ "dropdown"; "is-active" ] ]
        [
            div
                [ attr.classes [ "dropdown-trigger" ] ]
                [
                    button
                        [ attr.classes [ "button" ] ; "aria-haspopup" => "true"; "aria-controls" => "dropdown-menu" ]
                        [
                            span [] [ text $"{segmentName.Value}" ]
                        ]
                ]
            div
                [ attr.classes [ "dropdown-menu" ] ; "role" => "menu" ]
                [
                ]
        ]

let ytSetOverlayCloseCommand (jsRuntime: IJSRuntime) =
    text "[close command]"
