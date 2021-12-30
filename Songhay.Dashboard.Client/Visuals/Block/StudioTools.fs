module Songhay.Dashboard.Client.Visuals.Block.StudioTools

open Bolero.Html
open Songhay.Dashboard.Client

let studioToolsNode =
    div
        [ attr.classes ([ "card" ] @ App.appBlockChildCssClasses) ]
        [
            div
                [ attr.classes [ "card-content" ] ]
                [
                    div
                        [ attr.classes [ "content"; "has-text-centered" ] ]
                        [ text "[StudioTools]" ]
                ]
        ]
