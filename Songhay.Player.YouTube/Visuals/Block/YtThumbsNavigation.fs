module Songhay.Player.YouTube.Visuals.Block.YtThumbsNavigation

open System
open Microsoft.JSInterop

open Bolero.Html

open Songhay.Player.YouTube
open Songhay.Modules.Bolero.JsRuntimeUtility

let bulmaDropdown (jsRuntime: IJSRuntime) (model: YouTubeModel) =
    let _, segmentName, documents = model.YtSetIndex.Value

    div
        [ attr.classes [ "dropdown" ] ]
        [
            div
                [ attr.classes [ "dropdown-trigger" ] ]
                [
                    button
                        [
                            attr.classes [ "button" ]; "aria-haspopup" => "true"; "aria-controls" => "dropdown-menu"
                            on.task.click (fun _ ->
                                jsRuntime |> consoleLogAsync [| "click!" |]
                            )
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
                                        [ attr.classes [ "dropdown-item" ]; "data-id" => display.id.StringValue ]
                                        [ text display.displayText.Value.Value ]
                                else empty
                        ]
                ]
        ]

let ytSetOverlayCloseCommand (jsRuntime: IJSRuntime) =
    text "[close command]"
