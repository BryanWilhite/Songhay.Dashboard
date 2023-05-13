namespace Songhay.Dashboard.Client.Components

open System

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

    let ytVideoUri model = $"https://www.youtube.com/watch?v={model.ytFigureVideoId}"

    let getClipboardData model=
        String.Concat(
            model.ytFigureTitle,
            Environment.NewLine,
            Environment.NewLine,
            (model |> ytVideoUri))

    let divNode model dispatch =
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
                    on.change (fun e -> dispatch <| YouTubeFigureResolutionChange $"{e.Value}")
                    forEach data <| fun res -> option { text res }
                }
            }

        let bindTitle = bind.input.string model.ytFigureTitle (fun s -> dispatch <| SetYouTubeFigureTitle s)

        let bindVideoId = bind.input.string model.ytFigureVideoId (fun s -> dispatch <| SetYouTubeFigureId s)

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
                        attr.alt model.ytFigureTitle
                        attr.src $"https://img.youtube.com/vi/{model.ytFigureVideoId}/{model.ytFigureThumbRes}.jpg"
                        attr.width 480
                    }
                }
                p {
                    ShadeWhiteTer.TextCssClass |> CssClasses.toHtmlClass
                    small { text model.ytFigureTitle }
                }
            }
            div {
                [ "field"; "is-grouped"; CssClass.m (T, L2) ] |> CssClasses.toHtmlClassFromList
                p {
                    "control" |> CssClasses.toHtmlClass
                    a {
                       [ "button"; "is-primary"; "is-light" ] |> CssClasses.toHtmlClassFromList
                       click.PreventDefault
                       on.click (fun _ -> CopyToClipboard (getClipboardData model) |> dispatch)
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
