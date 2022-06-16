module Songhay.Dashboard.Client.Visuals.Document

open System
open Bolero.Html

open Songhay.Modules.Bolero.BoleroUtility
open Songhay.Dashboard.Client

let metaElements =
    [
        meta { attr.charset "UTF-8" }
        meta { attr.name "viewport"; attr.content "width=device-width, initial-scale=1.0" }
    ]

let linkElements (contentBlockComponentId: string) =
    [
        link { attr.rel "stylesheet"; attr.href $"css/{contentBlockComponentId}.min.css" }
        link { attr.rel "icon"; attr.``type`` "image/x-icon"; attr.href "favicon.ico" }
    ]

let headElements (contentBlockComponentId: string) =
    metaElements
    @
    [ ``base`` { attr.href "/" } ]
    @
    (contentBlockComponentId |> linkElements)
    @
    [ script { attr.src "js/songhay.min.js" } ]
    @
    [ title { text App.AppTitle } ]

let headElement (contentBlockComponentId: string) =
    head { forEach (contentBlockComponentId |> headElements |> wrapn 2) <| id }

let footerElement =
    footer {
        nameof footer |> toHtmlClass

        div {
            [ "content"; "has-text-centered"; "is-small" ] |> toHtmlClassFromList
            p {
                span {
                    "copyright" |> toHtmlClass
                    rawHtml $"© Bryan D. Wilhite {DateTime.Now.Year}"
                }
            }
        }
    }
