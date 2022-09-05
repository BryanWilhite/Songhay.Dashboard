namespace Songhay.Dashboard.Client.Visuals

open Songhay.Modules.Bolero.Models
open Songhay.Modules.Bolero.Visuals.Bulma.CssClass
open Songhay.Modules.Bolero.Visuals.Bulma.Block

module Block =
    let studioPageNode nodes =
        bulmaTile
            TileSizeAuto
            None
            [
                bulmaTile
                    TileSizeAuto
                    ([tileIsParent; tileIsVertical] |> Some)
                    nodes
            ]