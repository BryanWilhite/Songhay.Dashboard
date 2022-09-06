namespace Songhay.Dashboard.Client.Components

open Microsoft.AspNetCore.Components
open Microsoft.JSInterop

open Bolero
open Bolero.Html

open Songhay.Modules.Bolero.BoleroUtility
open Songhay.Modules.Bolero.Models
open Songhay.Modules.Bolero.Visuals.Bulma.Button
open Songhay.Modules.Bolero.Visuals.Bulma.Block
open Songhay.Modules.Bolero.Visuals.Bulma.CssClass
open Songhay.Modules.Bolero.Visuals.Svg

open Songhay.Dashboard.Models
open Songhay.Dashboard.Client
open Songhay.Dashboard.Client.ElmishTypes

type StudioComponent() =
    inherit ElmishComponent<Model, Message>()

    static let studioLogo =
        let spanClasses = CssClasses [ "title"; fontSize Size2; hidden Touch ]

        div {
            "logo" |> toHtmlClass
            attr.title App.AppTitle

            span { (elementFontWeight Normal) |> spanClasses.Prepend |> toHtmlClassFromData; text "Songhay" }
            span { spanClasses.ToHtmlClassAttribute; text "System" }
            span { [ "title"; fontSize Size1 ] |> toHtmlClassFromList; text "(::)" }
        }

    static let svgVersionNode (data: VersionData) =
        let classes = CssClasses [
            levelItem
            "is-akyinkyin-base"
            elementTextIsUnselectable
            elementTextAlign Center
        ]

        div {
            classes.ToHtmlClassAttribute
            attr.title data.title.Value

            span {
                "icon" |> toHtmlClass
                "aria-hidden" => "true"

                svgNode (svgViewBoxSquare 24) svgData[data.id]
            }
            span { fontSize Size7 |> toHtmlClass; text data.version }
        }

    static let studioNode =
        let cssClassesParentLevel = CssClasses [ levelContainer; isMobileModifier ]

        let cssClassesSvgLinkNodes = [ m (LR, L6) ] |> cssClassesParentLevel.AppendList

        let cssClassesSvgVersionNodes =
            [ "has-text-greys-light-tone"; m (T, L6); p (T, L6) ] |> cssClassesParentLevel.AppendList

        div {
            "card" |> App.appBlockChildCssClasses.Prepend |> toHtmlClassFromData
            div {
                "card-content" |> toHtmlClass
                div {
                    [ "content"; elementTextAlign Center ] |> toHtmlClassFromList
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
            }
        }

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
                    (Has (CssClasses [ tileIsParent ]))
                    [ studioNode ]
            ]
