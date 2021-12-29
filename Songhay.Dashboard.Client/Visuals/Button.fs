module Songhay.Dashboard.Client.Visuals.Button

open Bolero.Html
open Songhay.Dashboard.Client.App
open Songhay.Dashboard.Client.Visuals

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
        ] [ Svg.svgSpriteNode $"./#{data.id}" data.viewBox ] ]
