module Songhay.Dashboard.Client.Visuals.Button

open Bolero
open Bolero.Html
open Songhay.Dashboard.Client.Visuals.Types

let svgSpriteNode (href: string) (viewBox: string) =
    svg
        [
            "fill" => "currentColor"
            nameof viewBox => viewBox
            "xmlns" => "http://www.w3.org/2000/svg"
        ]
        [ RawHtml $@"<use href=""{href}""></use>" ]

let bulmaAnchorIconButton data =
    a
        [
            attr.classes [ "button"; "is-ghost" ]
            attr.href data.href
            attr.target "_blank"
            attr.title data.title
        ]
        [ span [ attr.classes [ "icon" ] ] [ svgSpriteNode $"./#{data.id}" data.viewBox ] ]
