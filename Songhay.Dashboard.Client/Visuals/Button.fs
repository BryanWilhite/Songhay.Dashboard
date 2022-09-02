namespace Songhay.Dashboard.Client.Visuals

open System
open Bolero.Html

open Songhay.Modules.Bolero.Visuals.Svg
open Songhay.Modules.Models

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
