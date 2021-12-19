module Songhay.Dashboard.Client.Visuals.Tile

open Bolero.Html
open Songhay.Dashboard.Client.Visuals.Block

let bulmaColumnTile width nodes =
    let cssClasses = [
        "tile"
        if width > 0 then $"is-{width}"
    ]

    div [ attr.classes cssClasses ] nodes

let bulmaContentParentTile isVertical nodes =
    let cssClasses = [
        "tile"
        "is-parent"
        if isVertical then "is-vertical"
    ]

    div [ attr.classes cssClasses ] nodes

let studioComponentNode =
    bulmaColumnTile 0 [ bulmaContentParentTile false [ Studio.studioNode ] ]

let studioLinksNode = //TODO add `Block.StudioLinks`
    bulmaColumnTile 0 [ bulmaContentParentTile false [ text "[StudioLinks]"  ] ]

let studioPageNode block =
    bulmaColumnTile 0 [ bulmaContentParentTile false [ block ] ]