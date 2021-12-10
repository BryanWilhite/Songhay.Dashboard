module Songhay.Dashboard.Client.Visuals.Button

open Bolero.Html
open Songhay.Dashboard.Client.Visuals.Svg
open Songhay.Dashboard.Client.Visuals.Types

let bulmaAnchorIconButton data =
    a
        [
            attr.classes [ "level-item"; "has-text-centered" ]
            attr.href data.href
            attr.target "_blank"
            attr.title data.title
        ]
        [ span [ attr.classes [ "icon" ] ] [ svgSpriteNode $"./#{data.id}" data.viewBox ] ]
