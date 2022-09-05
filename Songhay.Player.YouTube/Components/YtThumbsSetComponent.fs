namespace Songhay.Player.YouTube.Components

open Microsoft.AspNetCore.Components
open Microsoft.JSInterop

open Bolero
open Bolero.Html
open Elmish

open Songhay.Modules.Bolero.Models
open Songhay.Modules.Bolero.BoleroUtility
open Songhay.Modules.Bolero.Visuals.Bulma.CssClass
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
            let isActive = model.YtSetRequestSelection
            let callback = (fun _ -> SelectYtSet |> dispatch)
            let dropDownContent =
                forEach documents <| fun (display, _) ->
                    if display.displayText.IsSome then
                        let clientId = ClientId.fromIdentifier display.id
                        let itemIsActive = (clientId = snd model.YtSetIndexSelectedDocument)
                        let itemCallback = (fun _ -> CallYtSet (display.displayText.Value, clientId) |> dispatch)
                        let itemDisplayText =
                            (display.displayText |> Option.defaultWith (fun _ -> DisplayText "[missing]")).Value

                        (itemIsActive, itemCallback, itemDisplayText) |||> bulmaDropdownItem

                    else empty()

            dropDownContent |> bulmaDropdown isActive displayText callback

    static let ytSetOverlayCloseCommand (dispatch: Dispatch<YouTubeMessage>) =
        a {
            imageContainer (Square Square48) |> toHtmlClassFromList
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

        let levelRight =
            div {
                level Right

                div { levelItem; ytSetOverlayCloseCommand dispatch }
            }

        div {
            overlayClasses.ToHtmlClassAttribute
            cond model.YtSetIndex.IsSome <| function
            | true ->
                nav {
                    [ levelContainer; m (All, L2)] |> toHtmlClassFromList

                    div {
                        level Left

                        div { levelItem ; (dispatch, jsRuntime, model) |||> bulmaDropdown }
                        div {
                            [ levelItem; fontSize Size2 ] |> toHtmlClassFromList
                            text (fst model.YtSetIndexSelectedDocument).Value
                        }
                    }
                    levelRight
                }
            | false ->
                nav {
                    [ levelContainer; m (All, L2)] |> toHtmlClassFromList

                    levelRight
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
