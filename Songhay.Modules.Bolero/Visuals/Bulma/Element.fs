namespace Songhay.Modules.Bolero.Visuals.Bulma

open System

open Bolero
open Bolero.Html

open Songhay.Modules.Models
open Songhay.Modules.Bolero.Models
open Songhay.Modules.Bolero.BoleroUtility
open Songhay.Modules.Bolero.Visuals.Svg

///<summary>
/// Bulma Elements
/// “Essential interface elements that only require a single CSS class…”
/// — https://bulma.io/documentation/elements/
///</summary>
module Element = 
    let bulmaAnchorIconButton (title: DisplayText, href: Uri, id: Identifier) =
        a {
            (CssClasses [ CssClass.levelItem; CssClass.elementTextAlign Center ]).ToHtmlClassAttribute
            attr.href href.OriginalString
            attr.target "_blank"
            attr.title title.Value

            span {
                "icon" |> toHtmlClass
                "aria-hidden" => "true"

                svgNode (svgViewBoxSquare 24) svgData[id]
            }
        }

    let bulmaIcon (childNode: Node) =
        span { "icon" |> toHtmlClass; "aria-hidden" => "true"; childNode }

    let bulmaIconSvgViewBox (square: BulmaSquareDimension) =
        svgViewBox (0,0) (square.ToWidthOrHeight, square.ToWidthOrHeight)

    let bulmaImage (size: BulmaRatioDimension) (childNode: Node) =
        figure {
            size |> CssClass.imageContainer |> toHtmlClassFromList
            "aria-hidden" => "true"
            childNode
        }

    let bulmaPanelIcon (moreClasses: CssClassesOrEmpty) (childNode: Node) =
        span {
            CssClasses [ "panel-icon" ] |> moreClasses.ToHtmlClassAttribute
            "aria-hidden" => "true"
            childNode
        }
