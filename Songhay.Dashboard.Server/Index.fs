module Songhay.Dashboard.Server.Index

open Bolero
open Bolero.Html
open Bolero.Server.Html
open Songhay.Dashboard
open System

let newLine = RawHtml $"{Environment.NewLine}"

let metaElements =
    [
        newLine
        meta [attr.charset "UTF-8"]
        newLine
        meta [attr.name "viewport"; attr.content "width=device-width, initial-scale=1.0"]
        newLine
        newLine
    ]

let linkElements =
    [
        link [attr.rel "stylesheet"; attr.href "https://fonts.googleapis.com/icon?family=Material+Icons"]
        link [attr.rel "stylesheet"; attr.href "https://fonts.googleapis.com/css?family=Roboto:300,400,500"]
        link [attr.rel "stylesheet"; attr.href "css/index.min.css"]
        newLine
        link [attr.rel "icon"; attr.``type`` "image/x-icon"; attr.href "favicon.ico"]
        newLine
        newLine
    ]

let footerElement =
    footer [] [
        span [attr.classes ["copyright"]] [text $"© Bryan D. Wilhite {DateTime.Now.Year}"]
        newLine
    ]

let page = doctypeHtml [] [
    head [] (
        metaElements
        @
        [``base`` [attr.href "/"]; newLine]
        @
        linkElements
        @
        [title [] [text "SonghaySystem(::)"]; newLine]
    )
    body [attr.classes ["mat-app-background"; "mat-typography"]] [
        div [attr.id "main"] [rootComp<Client.Main.MyApp>]
        newLine
        boleroScript
        footerElement
    ]
]
