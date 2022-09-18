namespace Songhay.Dashboard.Client.Components

open Microsoft.AspNetCore.Components
open Microsoft.JSInterop

open Bolero
open Bolero.Html

open Songhay.Modules.Bolero.Models
open Songhay.Modules.Bolero.Visuals.Bulma.Component
open Songhay.Modules.Bolero.Visuals.Bulma.Element
open Songhay.Modules.Bolero.Visuals.Bulma.CssClass
open Songhay.Modules.Bolero.Visuals.Bulma.Layout
open Songhay.Modules.Bolero.Visuals.BodyElement

open Songhay.Dashboard.Models
open Songhay.Dashboard.Client
open Songhay.Dashboard.Client.Visuals.Colors
open Songhay.Dashboard.Client.ElmishTypes

type StudioComponent() =
    inherit ElmishComponent<Model, Message>()

    static let studioLogo =
        let spanClasses = (CssClasses (title (HasFontSize Size2))).Append (hidden Touch)

        div {
            "logo" |> CssClasses.toHtmlClass
            attr.title App.AppTitle

            span { ((elementFontWeight Normal) |> spanClasses.Prepend).ToHtmlClassAttribute; text "Songhay" }
            span { spanClasses.ToHtmlClassAttribute; text "System" }
            span { (title (HasFontSize Size1)) |> CssClasses.toHtmlClassFromList; text "(::)" }
        }

    static let svgVersionNode (data: VersionData) =
        let classes = CssClasses [
            levelItem
            bulmaAkyinkyinBase
            elementTextIsUnselectable
            elementTextAlign Center
        ]

        bulmaLevelItem
            (HasClasses classes)
            (HasAttrs [ (attr.title data.title.Value) ])
            [
                bulmaIcon (svgElement (bulmaIconSvgViewBox Square24) SonghaySvgData.map[data.id])
                span { fontSize Size7 |> CssClasses.toHtmlClass; text data.version }
            ]

    static let studioNode =
        let cssClassesParentLevel = CssClasses [ levelContainer; isMobileModifier ]

        let cssClassesSvgLinkNodes = [ m (LR, L6) ] |> cssClassesParentLevel.AppendList

        let cssClassesSvgVersionNodes =
            [ bulmaTextGreyLightTone; m (T, L6); p (T, L6) ] |> cssClassesParentLevel.AppendList

        let cardContentNodes =
            [
                div {
                    [ content; elementTextAlign Center ] |> CssClasses.toHtmlClassFromList
                    studioLogo
                }
                div {
                    cssClassesSvgLinkNodes.ToHtmlClassAttribute
                    forEach App.appSocialLinks <| bulmaAnchorIconButton
                }
                div {
                    cssClassesSvgVersionNodes.ToHtmlClassAttribute
                    forEach App.appVersions <| svgVersionNode
                }
            ]

        let cardNode =
            bulmaCard
                (HasClasses (CssClasses [ bulmaBackgroundGreyDarkTone ]))
                NoNode
                NoNode
                NoNode
                NoCssClasses
                cardContentNodes

        bulmaTile
            TileSizeAuto
            (HasClasses (CssClasses [ tileIsChild ]))
            [ cardNode ]

    static member EComp model dispatch =
        ecomp<StudioComponent, _, _> model dispatch { attr.empty() }

    [<Inject>]
    member val JSRuntime = Unchecked.defaultof<IJSRuntime> with get, set

    override this.View _ _ =
        bulmaTile
            TileSizeAuto
            NoCssClasses
            [
                bulmaTile
                    TileSizeAuto
                    (HasClasses (CssClasses [ tileIsParent ]))
                    [ studioNode ]
            ]
