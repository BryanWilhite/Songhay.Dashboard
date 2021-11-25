module Songhay.Dashboard.Server.ContentBlock

open System
open Bolero
open Bolero.Html
open Bolero.Server.Html
open Songhay.Blazor.BoleroUtility
open Songhay.Dashboard.Client.Components.ContentBlock

let metaElements =
    [
        meta [ attr.charset "UTF-8" ]
        meta [ attr.name "viewport"; attr.content "width=device-width, initial-scale=1.0" ]
    ]

let linkElements =
    [
        link [ attr.rel "stylesheet"; attr.href "css/index.min.css" ]
        link [ attr.rel "icon"; attr.``type`` "image/x-icon"; attr.href "favicon.ico" ]
    ]

let headElements =
    metaElements
    @
    [ ``base`` [ attr.href "/" ] ]
    @
    linkElements
    @
    [ title [] [ text "SonghaySystem(::)"] ]

let footerElement =
    footer [] [
        span [ attr.classes ["copyright"] ] [ RawHtml $"© Bryan D. Wilhite {DateTime.Now.Year}" ]
    ]

let bodyElements =
    [
        section [
            attr.id ContentBlockComponent.Id
            attr.classes [ "section" ] ] 
            ([ rootComp<ContentBlockComponent> ] |> wrapn 3)
        boleroScript
        footerElement
        newLine
    ]

let pageElements =
    [
        head [] (headElements |> wrapn 2)
        body [] (bodyElements |> wrapn 2)
        newLine
    ]

let page = doctypeHtml [] (pageElements |> wrapn 1)
