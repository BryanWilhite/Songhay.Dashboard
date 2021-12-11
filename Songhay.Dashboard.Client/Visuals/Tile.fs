module Songhay.Dashboard.Client.Visuals.Tile

open Bolero.Html
open Songhay.Dashboard.Client.Visuals.Block

let bulmaColumnTile width nodes =
    let cssClasses =
        match width with
        | 0 -> [ "tile" ]
        | _ -> [ "tile"; $"is-{width}" ]

    div [ attr.classes cssClasses ] nodes

let bulmaContentParentTile isVertical nodes =
    let cssClasses =
        let cssDefault = [ "tile"; "is-parent" ]
        match isVertical with
        | true -> cssDefault @ [ "is-vertical" ]
        | _ -> cssDefault

    div [ attr.classes cssClasses ] nodes

let studioComponentNode =
    bulmaColumnTile 0 [ bulmaContentParentTile false [ Studio.studioNode ] ]
