module Songhay.Dashboard.Client.Visuals.Block.StudioFeeds

open Bolero.Html

open Songhay.Dashboard.Client
open Songhay.Dashboard.Client.ElmishTypes

let studioFeedsNode (model: Model) =
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
