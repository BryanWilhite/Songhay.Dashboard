module Songhay.Dashboard.Server.Visuals.ContentBlock

open Bolero.Html
open Bolero.Server.Html
open Songhay.Modules.Bolero.BoleroUtility
open Songhay.Dashboard.Server.Visuals.Head
open Songhay.Dashboard.Server.Visuals.Body

let pageElements =
    [
        head { forEach (headElements |> wrapn 2) <| id }
        body { forEach (bodyElements |> wrapn 2) <| id }
        newLine
    ]

let page = doctypeHtml { forEach (pageElements |> wrapn 1) <| id }
