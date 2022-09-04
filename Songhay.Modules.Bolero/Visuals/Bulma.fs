namespace Songhay.Modules.Bolero.Visuals

open System
open Bolero.Html

open Songhay.Modules.Models
open Songhay.Modules.Bolero.BoleroUtility
open Songhay.Modules.Bolero.Visuals.Svg

module Bulma =
    module Block =
        let bulmaLoader (padding: int) (margin: int) =
            div {
                [ "has-text-centered"; "loader-container"; $"p-{padding}"] |> toHtmlClassFromList

                div {
                    [ "image"; "is-128x128"; "loader"; $"m-{margin}" ] |> toHtmlClassFromList

                    attr.title "Loading…"
                }
            }

    module Button =
        let bulmaAnchorIconButton (title: DisplayText, href: Uri, id: Identifier) =
            a {
                (CssClasses [ "level-item"; "has-text-centered" ]).ToHtmlClassAttribute
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
