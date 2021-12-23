module Songhay.Dashboard.Client.Visuals.Block.StudioFeeds

open Bolero.Html

let studioFeedsNode =
    div
        [ attr.classes [ "card"; "has-background-greys-dark-tone"; "is-child"; "tile" ] ]
        [
            div
                [ attr.classes [ "card-content" ] ]
                [
                    div
                        [ attr.classes [ "content"; "has-text-centered" ] ]
                        [ text "[StudioFeeds]" ]
                ]
        ]
