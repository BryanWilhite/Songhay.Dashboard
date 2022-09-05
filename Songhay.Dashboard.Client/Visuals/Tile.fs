namespace Songhay.Dashboard.Client.Visuals

open Songhay.Modules.Bolero.Models
open Songhay.Modules.Bolero.Visuals.Bulma.CssClass
open Songhay.Modules.Bolero.Visuals.Bulma.Tile

module Tile =
    let studioPageNode nodes =
        bulmaColumnTile
            TileSizeAuto
            None
            [
                bulmaColumnTile
                    TileSizeAuto
                    ([tileIsParent; tileIsVertical] |> Some)
                    nodes
            ]
