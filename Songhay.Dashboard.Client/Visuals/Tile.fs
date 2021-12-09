module Songhay.Dashboard.Client.Visuals.Tile

open Bolero.Html
open Songhay.Dashboard.Client.Visuals.Block

let bulmaColumnTile (width: int) nodes =
    let classes =
        match width with
        | 0 -> [ "tile" ]
        | _ -> [ "tile"; $"is-{width}" ]

    div [ attr.classes classes ] nodes

let bulmaContentParentTile nodes =
    div [ attr.classes [ "tile"; "is-parent" ] ] nodes

let studioComponentNode =
    bulmaColumnTile 0 [
        bulmaContentParentTile [
            div [ attr.classes [ "card"; "has-background-grey-darker"; "is-child"; "tile" ] ] [
                div [ attr.classes [ "card-content" ] ] [
                    div [ attr.classes [ "content"; "has-text-centered" ] ] [ Studio.studioLogo ]
                    div [ attr.classes [ "content"; "has-text-centered" ] ] Studio.svgLinkNodes
                    div
                        [ attr.classes [ "tile"; "is-parent"; "mt-6"; "pt-6" ] ]
                        Studio.svgVersionNodes
                ]
            ]
        ]
    ]