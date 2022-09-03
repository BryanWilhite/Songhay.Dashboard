namespace Songhay.Dashboard.Client.Visuals

open Bolero.Html

open Songhay.Modules.Bolero.BoleroUtility

module Tile =

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

    let studioPageNode nodes =
        bulmaColumnTile 0 [ bulmaContentParentTile true nodes ]
