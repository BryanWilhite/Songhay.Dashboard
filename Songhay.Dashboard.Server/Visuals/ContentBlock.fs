module Songhay.Dashboard.Server.Visuals.ContentBlock

open Bolero.Html
open Bolero.Server.Html
open Songhay.Blazor.BoleroUtility
open Songhay.Dashboard.Server.Visuals.Head
open Songhay.Dashboard.Server.Visuals.Body

let pageElements =
    [
        head [] (headElements |> wrapn 2)
        body [] (bodyElements |> wrapn 2)
        newLine
    ]

let page = doctypeHtml [] (pageElements |> wrapn 1)
