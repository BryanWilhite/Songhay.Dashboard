module Songhay.Dashboard.Client.Visuals.Tile

open Bolero.Html

let bulmaParentTile (width: int) nodes =
    div [attr.classes [ "tile"; $"is-{width}" ] ] nodes
