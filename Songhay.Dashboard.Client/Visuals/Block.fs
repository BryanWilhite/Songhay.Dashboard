namespace Songhay.Dashboard.Client.Visuals

open Songhay.Modules.Bolero.BoleroUtility
open Songhay.Modules.Bolero.Models
open Songhay.Modules.Bolero.Visuals.Bulma.CssClass
open Songhay.Modules.Bolero.Visuals.Bulma.Layout

module Block =
    let studioPageNode nodes =
        bulmaTile
            TileSizeAuto
            NoCssClasses
            [
                bulmaTile
                    TileSizeAuto
                    (HasClasses (CssClasses [tileIsParent; tileIsVertical]))
                    nodes
            ]
