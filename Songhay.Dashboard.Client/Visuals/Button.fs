module Songhay.Dashboard.Client.Visuals.Button

open System
open Bolero.Html

open Songhay.Modules.Bolero.Visuals.Svg
open Songhay.Modules.Models

let bulmaAnchorIconButton (title: DisplayText, href: Uri, id: Identifier) =
    a
        [
            attr.classes [ "level-item"; "has-text-centered" ]
            attr.href href.OriginalString
            attr.target "_blank"
            attr.title title.Value
        ]
        [ span [
            attr.classes [ "icon" ]
            "aria-hidden" => "true"
        ] [ svgNode (svgViewBoxSquare 24) svgData[id] ] ]
