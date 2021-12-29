module Songhay.Dashboard.Server.Visuals.Head

open Bolero.Html
open Songhay.Dashboard.Client
open Songhay.Dashboard.Client.Components.ContentBlock

let metaElements =
    [
        meta [ attr.charset "UTF-8" ]
        meta [ attr.name "viewport"; attr.content "width=device-width, initial-scale=1.0" ]
    ]

let linkElements =
    [
        link [ attr.rel "stylesheet"; attr.href $"css/{ContentBlockComponent.Id}.min.css" ]
        link [ attr.rel "icon"; attr.``type`` "image/x-icon"; attr.href "favicon.ico" ]
    ]

let headElements =
    metaElements
    @
    [ ``base`` [ attr.href "/" ] ]
    @
    linkElements
    @
    [ title [] [ text App.appTitle ] ]
