module Songhay.Dashboard.Client.Visuals.Block.StudioTools

open Bolero.Html

let studioToolsNode =
    div
        [ attr.classes [ "card"; "has-background-greys-dark-tone"; "is-child"; "tile" ] ]
        [
            div
                [ attr.classes [ "card-content" ] ]
                [
                    div
                        [ attr.classes [ "content"; "has-text-centered" ] ]
                        [ text "[StudioTools]" ]
                ]
        ]
