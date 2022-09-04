namespace Songhay.Dashboard.Client.Visuals

open Songhay.Modules.Bolero.Visuals.Bulma.Tile

module Tile =
    let studioPageNode nodes =
        bulmaColumnTile 0 [ bulmaContentParentTile true nodes ]
