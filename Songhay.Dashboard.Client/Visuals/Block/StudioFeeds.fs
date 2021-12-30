module Songhay.Dashboard.Client.Visuals.Block.StudioFeeds

open Bolero.Html
open Songhay.Dashboard.Client

let studioFeedsNode =
    div
        [ attr.classes ([ "card" ] @ App.appBlockChildCssClasses) ]
        [
            div
                [ attr.classes [ "card-content" ] ]
                [
                    div
                        [ attr.classes [ "content"; "has-text-centered" ] ]
                        [ text "[StudioFeeds]" ]
                ]
        ]
