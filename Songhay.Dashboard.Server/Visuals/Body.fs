module Songhay.Dashboard.Server.Visuals.Body

open System
open Bolero
open Bolero.Html
open Bolero.Server.Html
open Songhay.Blazor.BoleroUtility
open Songhay.Dashboard.Client.Components.ContentBlock

let footerElement =
    footer [ attr.classes [ nameof footer ] ] [
        div [ attr.classes [ "content"; "has-text-centered"; "is-small" ] ] [
            p [] [
                span [ attr.classes [ "copyright" ] ]
                    [ RawHtml $"© Bryan D. Wilhite {DateTime.Now.Year}" ]
            ]
        ]
    ]

let bodyElements =
    [
        section [
            attr.id ContentBlockComponent.Id
            attr.classes [ nameof section ] ] 
            ([ rootComp<ContentBlockComponent> ] |> wrapn 3)
        boleroScript
        footerElement
        newLine
    ]
