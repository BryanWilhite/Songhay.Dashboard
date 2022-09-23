namespace Songhay.Dashboard.Client.Components

open System
open System.Linq
open Microsoft.AspNetCore.Components
open Microsoft.JSInterop

open Bolero
open Bolero.Html

open Songhay.Modules.Models
open Songhay.Modules.Bolero.Models
open Songhay.Modules.Bolero.SvgUtility
open Songhay.Modules.Bolero.Visuals.Bulma.Component
open Songhay.Modules.Bolero.Visuals.Bulma.Element
open Songhay.Modules.Bolero.Visuals.Bulma.CssClass
open Songhay.Modules.Bolero.Visuals.Bulma.Layout
open Songhay.Modules.Bolero.Visuals.BodyElement

open Songhay.Dashboard.Models
open Songhay.Dashboard.Client
open Songhay.Dashboard.Client.App.Colors
open Songhay.Dashboard.Client.ElmishTypes

type StudioComponent() =
    inherit ElmishComponent<Model, Message>()

    static let studioAnchor (title: DisplayText, href: Uri, id: Identifier) =
        anchorElement
            (HasClasses (CssClasses [ levelItem; elementTextAlign AlignCentered ]))
            href
            TargetBlank
            (HasAttr (attr.title title.Value))
            (bulmaIcon
                (((svgViewBoxSquare 24), SonghaySvgData.Get(id)) ||> svgElement))

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
            elementTextAlign AlignCentered
        ]

        bulmaLevelItem
            (HasClasses classes)
            (HasAttr (attr.title data.title.Value))
            (concat {
                bulmaIcon (((svgViewBoxSquare 24), SonghaySvgData.Get(data.id)) ||> svgElement)
                span { fontSize Size7 |> CssClasses.toHtmlClass; text data.version }
            })

    static let studioNode =
        let cssClassesParentLevel = CssClasses [ levelContainer; isMobileModifier ]

        let cssClassesSvgLinkNodes = [ m (LR, L6) ] |> cssClassesParentLevel.AppendList

        let cssClassesSvgVersionNodes =
            [ bulmaTextGreyLightTone; m (T, L6); p (T, L6) ] |> cssClassesParentLevel.AppendList

        let cardContentNodes =
            concat {
                div {
                    [ content; elementTextAlign AlignCentered ] |> CssClasses.toHtmlClassFromList
                    studioLogo
                }
                div {
                    cssClassesSvgLinkNodes.ToHtmlClassAttribute
                    forEach App.appSocialLinks <| studioAnchor
                }
                div {
                    cssClassesSvgVersionNodes.ToHtmlClassAttribute
                    forEach App.appVersions <| svgVersionNode
                }
            }

        let cardNode =
            bulmaCard
                (HasClasses (CssClasses [ bulmaBackgroundGreyDarkTone ]))
                NoNode
                NoNode
                NoNode
                NoCssClasses
                cardContentNodes

        bulmaTile
            HSizeAuto
            (HasClasses (CssClasses [ tileIsChild ]))
            cardNode

    static member EComp model dispatch =
        ecomp<StudioComponent, _, _> model dispatch { attr.empty() }

    [<Inject>]
    member val JSRuntime = Unchecked.defaultof<IJSRuntime> with get, set

    override this.View _ _ =
        bulmaTile
            HSizeAuto
            NoCssClasses
            (bulmaTile
                HSizeAuto
                (HasClasses (CssClasses [ tileIsParent ]))
                studioNode)
