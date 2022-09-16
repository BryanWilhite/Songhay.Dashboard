namespace Songhay.Modules.Bolero.Visuals.Bulma

open System

open Bolero
open Bolero.Html

open Songhay.Modules.Models
open Songhay.Modules.Bolero.Models
open Songhay.Modules.Bolero.SvgUtility
open Songhay.Modules.Bolero.Visuals.Element

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
                "icon" |> CssClasses.toHtmlClass
                AriaHidden.ToAttr

                svgElement (svgViewBoxSquare 24) SonghaySvgData.map[id]
            }
        }

    let bulmaContent (moreClasses: CssClassesOrEmpty) (childNodes: Node list) =
        div {
            CssClasses [ CssClass.content ] |> moreClasses.ToHtmlClassAttribute

            forEach childNodes <| id
        }

    let bulmaIcon (visualNode: Node) = span { "icon" |> CssClasses.toHtmlClass; AriaHidden.ToAttr; visualNode }

    let bulmaIconSvgViewBox (square: BulmaSquareDimension) =
        svgViewBox (0,0) (square.ToWidthOrHeight, square.ToWidthOrHeight)

    let bulmaImageContainer (size: BulmaRatioDimension) (attribute: HtmlAttributeOrEmpty) (visualNode: Node) =
        figure {
            size |> CssClass.imageContainer |> CssClasses.toHtmlClassFromList
            attribute.Value

            visualNode
        }

    ///<summary>
    /// Bulma CSS class-name function for <see cref="CssClass.notification" />.
    ///</summary>
    let bulmaNotification (moreClasses: CssClassesOrEmpty) (childNodes: Node list) =
        div {
             CssClasses [ CssClass.notification ] |> moreClasses.ToHtmlClassAttribute

             forEach childNodes <| id
        }
