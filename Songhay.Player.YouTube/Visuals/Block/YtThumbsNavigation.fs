module Songhay.Player.YouTube.Visuals.Block.YtThumbsNavigation

open Microsoft.JSInterop

open Bolero.Html
open Elmish

open Songhay.Modules.Models
open Songhay.Modules.Bolero.BoleroUtility
open Songhay.Modules.Bolero.Models
open Songhay.Player.YouTube

let click = GlobalEventHandlers.OnClick

let bulmaDropdown (dispatch: Dispatch<YouTubeMessage>) (_: IJSRuntime) (model: YouTubeModel) =
    let _, segmentName, documents = model.YtSetIndex.Value
    let dropdownClassesDefault = [ "dropdown" ]
    let dropdownClasses =
        match model.YtCssClass with
        | None -> dropdownClassesDefault
        | Some (cmd, id) ->
            match cmd with
            | Remove -> dropdownClassesDefault
            | _ -> dropdownClassesDefault @ [ id.StringValue ]

    div
        [ attr.classes dropdownClasses ]
        [
            div
                [ attr.classes [ "dropdown-trigger" ] ]
                [
                    button
                        [
                            attr.classes [ "button" ]; "aria-haspopup" => "true"; "aria-controls" => "dropdown-menu"
                            on.click (fun _ -> CssClass (Add, Identifier.fromString "is-active") |> dispatch)
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
                                            "data-id" => display.id.StringValue
                                            on.click (fun _ -> CssClass (Remove, Identifier.fromString "is-active") |> dispatch)
                                        ]
                                        [ text (display.displayText |> Option.get).Value ]
                                else empty
                        ]
                ]
        ]

let ytSetOverlayCloseCommand (jsRuntime: IJSRuntime) =
    text "[close command]"
