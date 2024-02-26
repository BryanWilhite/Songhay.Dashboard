namespace Songhay.Dashboard.Client.Components

open System
open Microsoft.JSInterop

open Bolero
open Bolero.Html

open Songhay.Modules.Models
open Songhay.Modules.Bolero.Models
open Songhay.Modules.Bolero.Visuals.Bulma
open Songhay.Modules.Bolero.Visuals.Bulma.Layout

open Songhay.Dashboard.Client.App.Colors
open Songhay.Dashboard.Client.Models

type YouTubeFigureElmishComponent() =
    inherit ElmishComponent<DashboardModel, DashboardMessage>()

    static let click = DomElementEvent.Click

    let ytVideoUri (model: DashboardModel) =
        let ytFigure = model.getYouTubeFigure()
        $"https://www.youtube.com/watch?v={ytFigure.id}"

    let getClipboardData (model: DashboardModel) =
        let ytFigure = model.getYouTubeFigure()
        String.Concat(
            ytFigure.title,
            Environment.NewLine,
            Environment.NewLine,
            (model |> ytVideoUri))

    let divNode (model: DashboardModel) dispatch =
        let ytFigure = model.getYouTubeFigure()
        let bulmaField (fId: string) (fLabel: string) (size: int) (bindAttr: Attr) =
            div {
                [ "field"; CssClass.m (T, L1) ] |> CssClasses.toHtmlClassFromList
                label {
                    [ "label"; ShadeWhiteTer.TextCssClass] |> CssClasses.toHtmlClassFromList
                    attr.``for`` fId
                    text $"{fLabel}:"
                }
                input {
                    "input" |> CssClasses.toHtmlClass
                    bindAttr
                    attr.id fId
                    attr.size size
                }
            }

        let bulmaSelect =
            let data = [
                "maxresdefault"
                "hqdefault"
                "mqdefault"
                "sddefault"
                "default"
            ]
            div {
                [ "select"; CssClass.m (B, L4) ] |> CssClasses.toHtmlClassFromList
                select {
                    on.change (fun e -> dispatch <| ChangeVisualState (YouTubeFigure { ytFigure with resolution = $"{e.Value}" }))
                    forEach data <| fun res -> option { text res }
                }
            }

        let bindTitle = bind.input.string
                            ytFigure.title
                            (fun s -> dispatch <| ChangeVisualState (YouTubeFigure {ytFigure with title = s}))

        let bindVideoId = bind.input.string
                              ytFigure.id
                              (fun s -> dispatch <| ChangeVisualState (YouTubeFigure {ytFigure with id = s}))

        div {
            CssClass.m (All, L4) |> CssClasses.toHtmlClass
            bulmaField "yt-title" "Title" 42 bindTitle
            bulmaField "yt-video-id" "Video ID" 42 bindVideoId
            bulmaSelect
            figure {
                a {
                    attr.href (model |> ytVideoUri)
                    attr.target "_blank"
                    img {
                        attr.alt <| ytFigure.title
                        attr.src $"https://img.youtube.com/vi/{ytFigure.id}/{ytFigure.resolution}.jpg"
                        attr.width 480
                    }
                }
                p {
                    ShadeWhiteTer.TextCssClass |> CssClasses.toHtmlClass
                    small { text <| ytFigure.title }
                }
            }
            div {
                [ "field"; "is-grouped"; CssClass.m (T, L2) ] |> CssClasses.toHtmlClassFromList
                p {
                    "control" |> CssClasses.toHtmlClass
                    a {
                       [ "button"; "is-primary"; "is-light" ] |> CssClasses.toHtmlClassFromList
                       click.PreventDefault
                       on.click (fun _ ->
                           let data = getClipboardData model
                           model.boleroServices.jsRuntime.InvokeVoidAsync("window.navigator.clipboard.writeText", data).AsTask() |> ignore
                       )

                       text "Copy Text"
                    }
                }
            }
        }

    static member EComp model dispatch =
        ecomp<YouTubeFigureElmishComponent, _, _> model dispatch { attr.empty() }

    override this.View model dispatch =
        bulmaTile
            HSizeAuto
            (HasClasses <| CssClasses [ CssClass.tileIsChild; CssClass.notification; bulmaBackgroundGreyDarkTone ])
            (divNode model dispatch)
