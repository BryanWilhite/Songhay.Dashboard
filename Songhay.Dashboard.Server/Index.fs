module Songhay.Dashboard.Server.Index

open Bolero
open Bolero.Html
open Bolero.Server.Html
open Songhay.Dashboard.Client
open System

let newLine = RawHtml $"{Environment.NewLine}"

let wrapn indentLevel (nodes : Node list) =
    let numberOfSpaces = 4
    let spaceChar = ' '
    let charArray =
        spaceChar
        |> Array.replicate (numberOfSpaces * indentLevel)
    let indent = String(charArray)

    nodes |> List.collect (fun node -> [newLine; text indent; node])

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
        div [ attr.id "main" ] ([ rootComp<Main.MainApp> ] |> wrapn 3)
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
