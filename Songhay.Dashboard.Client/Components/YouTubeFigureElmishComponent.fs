namespace Songhay.Dashboard.Client.Components

open System

open Bolero
open Bolero.Html

open Songhay.Modules.Models
open Songhay.Modules.Bolero.JsRuntimeUtility
open Songhay.Modules.Bolero.Models
open Songhay.Modules.Bolero.Visuals.BodyElement
open Songhay.Modules.Bolero.Visuals.Bulma
open Songhay.Modules.Bolero.Visuals.Bulma.Form
open Songhay.Modules.Bolero.Visuals.Bulma.Layout

open Songhay.Dashboard.Client.App.Colors
open Songhay.Dashboard.Client.Models

type YouTubeFigureElmishComponent() =
    inherit ElmishComponent<DashboardModel, DashboardMessage>()

    static let jsRuntime = Songhay.Modules.Bolero.ServiceProviderUtility.getIJSRuntime()

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

        let bindTitle = bind.input.string
                            ytFigure.title
                            (fun s -> dispatch <| ChangeVisualState (YouTubeFigure {ytFigure with title = s}))

        let bindVideoId = bind.input.string
                              ytFigure.id
                              (fun s -> dispatch <| ChangeVisualState (YouTubeFigure {ytFigure with id = s}))

        div {
            CssClass.m (All, L4) |> CssClasses.toHtmlClass
            bulmaField
                (HasClasses <| CssClasses [CssClass.m (T, L1)])
                (concat {
                    bulmaLabel
                        (HasClasses <| CssClasses [ShadeWhiteTer.TextCssClass])
                        (HasAttr <| attr.``for`` "yt-title")
                        (text "Title:")
                    bulmaInput
                        NoCssClasses
                        (HasAttr <| attrs {attr.id "yt-title"; attr.size 42; bindTitle})
                })
            bulmaField
                (HasClasses <| CssClasses [CssClass.m (T, L1)])
                (concat {
                    bulmaLabel
                        (HasClasses <| CssClasses [ShadeWhiteTer.TextCssClass])
                        (HasAttr <| attr.``for`` "yt-video-id")
                        (text "Video ID:")
                    bulmaInput
                        NoCssClasses
                        (HasAttr <| attrs {attr.id "yt-video-id"; attr.size 42; bindVideoId})
                })

            let data = [
                "maxresdefault"
                "hqdefault"
                "mqdefault"
                "sddefault"
                "default"
            ]

            bulmaSelect
                (HasClasses <| CssClasses [CssClass.m (B, L4)])
                (HasAttr <| on.change (fun e -> dispatch <| ChangeVisualState (YouTubeFigure { ytFigure with resolution = $"{e.Value}" })))
                (forEach data <| fun res -> option { text res })

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

            bulmaField
                (HasClasses <| CssClasses ([CssClass.m (T, L2)] @ CssClass.fieldIsGrouped AlignLeft))
                (bulmaControl
                    NoCssClasses
                    (anchorButtonElement
                        (HasClasses <| CssClasses [CssClass.buttonClass; ColorPrimary.CssClass; "is-light"])
                        (HasAttr <| on.click (fun _ ->
                               let data = getClipboardData model
                               jsRuntime |> copyToClipboard data |> ignore
                           )
                        )
                        (text "Copy Text")
                    )
                )
        }

    static member EComp model dispatch =
        ecomp<YouTubeFigureElmishComponent, _, _> model dispatch { attr.empty() }

    override this.View model dispatch =
        bulmaTile
            HSizeAuto
            (HasClasses <| CssClasses [ CssClass.tileIsChild; CssClass.notification; bulmaBackgroundGreyDarkTone ])
            (divNode model dispatch)
