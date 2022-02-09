module Songhay.Dashboard.Client.Visuals.Button

open Bolero.Html

open Songhay.Dashboard.Client.Models
open Songhay.Dashboard.Client.Visuals.Svg

let bulmaAnchorIconButton data =
    a
        [
            attr.classes [ "level-item"; "has-text-centered" ]
            attr.href data.href
            attr.target "_blank"
            attr.title data.title
        ]
        [ span [
            attr.classes [ "icon" ]
            "aria-hidden" => "true"
        ] [ svgNode data.viewBox svgData[data.id] ] ]
