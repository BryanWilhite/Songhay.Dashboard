module Songhay.Dashboard.Client.Visuals.Button

open System
open Bolero.Html

open Songhay.Dashboard.Client.Visuals.Svg
open Songhay.Modules.Models

let bulmaAnchorIconButton (title: DisplayText, href: Uri, id: Identifier) =
    a
        [
            attr.classes [ "level-item"; "has-text-centered" ]
            attr.href href.OriginalString
            attr.target "_blank"
            attr.title (title |> DisplayText.toDisplayString)
        ]
        [ span [
            attr.classes [ "icon" ]
            "aria-hidden" => "true"
        ] [ svgNode (svgViewBoxSquare 24) svgData[id] ] ]
