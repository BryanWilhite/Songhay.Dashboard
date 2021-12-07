module Songhay.Dashboard.Client.Visuals.Svg

open Bolero
open Bolero.Html

let svgSpriteNode (href: string) (viewBox: string) =
    svg
        [
            "fill" => "currentColor"
            nameof viewBox => viewBox
            "xmlns" => "http://www.w3.org/2000/svg"
        ]
        [ RawHtml $@"<use href=""{href}""></use>" ]
