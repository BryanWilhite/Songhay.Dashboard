module Songhay.Dashboard.Server.Index

open Bolero.Html
open Bolero.Server.Html
open Songhay.Dashboard
open System

let metaElements =
    [
        meta [attr.charset "UTF-8"]
        meta [attr.name "viewport"; attr.content "width=device-width, initial-scale=1.0"]
    ]

let linkElements =
    [
        link [attr.rel "stylesheet"; attr.href "https://fonts.googleapis.com/icon?family=Material+Icons"]
        link [attr.rel "stylesheet"; attr.href "https://fonts.googleapis.com/css?family=Roboto:300,400,500"]
        link [attr.rel "stylesheet"; attr.href "css/index.min.css"]
        link [attr.rel "icon"; attr.``type`` "image/x-icon"; attr.href "favicon.ico"]
    ]

let footerElement =
    footer [] [
        span [attr.classes ["copyright"]] [text $"© Bryan D. Wilhite {DateTime.Now.Year}"]
    ]

let page = doctypeHtml [] [
    head [] (
        metaElements
        @
        [``base`` [attr.href "/"]]
        @
        linkElements
        @
        [title [] [text "SonghaySystem(::)"]]
    )
    body [attr.classes ["mat-app-background"; "mat-typography"]] [
        div [attr.id "main"] [rootComp<Client.Main.MyApp>]
        boleroScript
        footerElement
    ]
]
