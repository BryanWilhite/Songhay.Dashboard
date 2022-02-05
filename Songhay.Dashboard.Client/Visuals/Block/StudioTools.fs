module Songhay.Dashboard.Client.Visuals.Block.StudioTools

open Bolero.Html
open Songhay.Dashboard.Client
open Songhay.Dashboard.Client.ElmishTypes

let studioToolsNode (model: Model) =
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
