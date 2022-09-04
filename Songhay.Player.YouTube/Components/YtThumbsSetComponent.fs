namespace Songhay.Player.YouTube.Components

open Microsoft.AspNetCore.Components
open Microsoft.JSInterop

open Bolero
open Bolero.Html
open Elmish

open Songhay.Modules.Bolero.BoleroUtility
open Songhay.Modules.Bolero.Visuals.Bulma.Block
open Songhay.Modules.Bolero.Visuals.Svg
open Songhay.Modules.Models
open Songhay.Player.YouTube

type YtThumbsSetComponent() =
    inherit ElmishComponent<YouTubeModel, YouTubeMessage>()

    static let click = GlobalEventHandlers.OnClick

    static let bulmaDropdown (dispatch: Dispatch<YouTubeMessage>) (_: IJSRuntime) (model: YouTubeModel) =
        if model.YtSetIndex.IsNone then empty()
        else
            let _, segmentName, documents = model.YtSetIndex.Value
            let displayText = segmentName.Value |> Option.defaultWith (fun _ -> "[missing]")
            let dropdownClasses = CssClasses [
                "dropdown"
                if model.YtSetRequestSelection then "is-active"
            ]

            div {
                dropdownClasses.ToHtmlClassAttribute

                div {
                    "dropdown-trigger" |> toHtmlClass

                    button {
                        "button" |> toHtmlClass
                        "aria-haspopup" => "true"; "aria-controls" => "dropdown-menu"
                        on.click (fun _ -> SelectYtSet |> dispatch)

                        span { text displayText }
                    }
                }
                div {
                    "dropdown-menu" |> toHtmlClass; "role" => "menu"

                    div {
                        "dropdown-content" |> toHtmlClass

                        forEach documents <| fun (display, _) ->
                            let clientId = ClientId.fromIdentifier display.id
                            if display.displayText.IsSome then
                                a {
                                    attr.href "#"
                                    [
                                        "dropdown-item"
                                        if clientId = snd model.YtSetIndexSelectedDocument then
                                            "is-active"
                                    ] |> toHtmlClassFromList
                                    click.PreventDefault
                                    on.click (fun _ ->
                                        CallYtSet (display.displayText.Value, clientId) |> dispatch)

                                    text (display.displayText |> Option.get).Value
                                }
                            else empty()
                    }
                }
            }

    static let ytSetOverlayCloseCommand (dispatch: Dispatch<YouTubeMessage>) =
        a {
            [ "image"; "is-48x48" ] |> toHtmlClassFromList
            attr.href "#"
            attr.title "close b-roll overlay"
            click.PreventDefault
            on.click (fun _ -> YouTubeMessage.CloseYtSetOverlay |> dispatch)

            svgNode (svgViewBoxSquare 24) svgData[Keys.MDI_CLOSE_BOX_24PX.ToAlphanumeric]
        }

    static let ytThumbsSetNode (dispatch: Dispatch<YouTubeMessage>) (jsRuntime: IJSRuntime) (model: YouTubeModel) =
        let overlayClasses =
            CssClasses [
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

        div {
            overlayClasses.ToHtmlClassAttribute
            cond model.YtSetIndex.IsSome <| function
            | true ->
                nav {
                    [ "level"; "m-2" ] |> toHtmlClassFromList

                    div {
                        "level-left" |> toHtmlClass

                        div {
                            "level-item" |> toHtmlClass

                            model |> bulmaDropdown dispatch jsRuntime
                        }
                        div {
                            [ "level-item"; "is-size-2" ] |> toHtmlClassFromList

                            text (fst model.YtSetIndexSelectedDocument).Value
                        }
                    }
                    div {
                        "level-right" |> toHtmlClass

                        div {
                            "level-item" |> toHtmlClass

                            ytSetOverlayCloseCommand dispatch
                        }
                    }
                }
            | false ->
                nav {
                    [ "level"; "m-2" ] |> toHtmlClassFromList

                    div {
                        "level-right" |> toHtmlClass

                        div {
                            "level-item" |> toHtmlClass

                            ytSetOverlayCloseCommand dispatch
                        }
                    }
                }
            cond model.YtSet.IsSome <| function
            | true ->
                div {
                    "set" |> toHtmlClass

                    forEach model.YtSet.Value <| fun (_, items) ->
                        YtThumbsComponent.EComp None { model with YtItems = Some items } dispatch
                }
            | false -> (6, 6) ||> bulmaLoader
        }

    static member val Id = "yt-thumbs-set-block" with get

    static member EComp (model: YouTubeModel) dispatch =
        ecomp<YtThumbsSetComponent, _, _> model dispatch { attr.empty() }

    [<Inject>]
    member val JSRuntime = Unchecked.defaultof<IJSRuntime> with get, set

    override this.View model dispatch =
        model |> ytThumbsSetNode dispatch this.JSRuntime
