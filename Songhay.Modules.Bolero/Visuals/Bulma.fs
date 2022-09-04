namespace Songhay.Modules.Bolero.Visuals

open System
open Bolero.Html

open Songhay.Modules.Models
open Songhay.Modules.Bolero.BoleroUtility
open Songhay.Modules.Bolero.Visuals.Svg

module Bulma =
    module Button =

        let bulmaAnchorIconButton (title: DisplayText, href: Uri, id: Identifier) =
            a {
                attr.``class`` "level-item has-text-centered"
                attr.href href.OriginalString
                attr.target "_blank"
                attr.title title.Value

                span {
                    attr.``class`` "icon"
                    "aria-hidden" => "true"

                    svgNode (svgViewBoxSquare 24) svgData[id]
                }
            }

    module Tile =
        let bulmaColumnTile width nodes =
            let cssClasses = CssClasses [
                "tile"
                if width > 0 then $"is-{width}"
            ]

            div {
                cssClasses.ToHtmlClassAttribute
                forEach nodes <| id
            }

        let bulmaContentParentTile isVertical nodes =
            let cssClasses = CssClasses [
                "tile"
                "is-parent"
                if isVertical then "is-vertical"
            ]

            div {
                cssClasses.ToHtmlClassAttribute
                forEach nodes <| id
            }
