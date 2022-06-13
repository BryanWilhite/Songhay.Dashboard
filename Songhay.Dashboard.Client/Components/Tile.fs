module Songhay.Dashboard.Client.Components.Tile

open Bolero.Html
open Songhay.Modules.Bolero.BoleroUtility

open Songhay.Dashboard.Client.Components.Block

let bulmaColumnTile width nodes =
    let cssClasses = CssClasses [
        "tile"
        if width > 0 then $"is-{width}"
    ]

    div {
        cssClasses.ToHtmlClassAttribute
        forEach nodes <| id
    }

let bulmaContentParentTile isVertical nodes =
    let cssClasses = CssClasses [
        "tile"
        "is-parent"
        if isVertical then "is-vertical"
    ]

    div {
        cssClasses.ToHtmlClassAttribute
        forEach nodes <| id
    }

let studioComponentNode =
    bulmaColumnTile 0 [ bulmaContentParentTile false [ Studio.studioNode ] ]

let studioLinksNode =
    bulmaColumnTile 0 [ bulmaContentParentTile false [ StudioLinks.studioLinksNode ] ]

let studioPageNode nodes =
    bulmaColumnTile 0 [ bulmaContentParentTile true nodes ]
