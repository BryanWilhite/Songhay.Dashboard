module Songhay.Dashboard.Server.Visuals.Body

open System
open Bolero.Html
open Bolero.Server.Html
open Songhay.Modules.Bolero.BoleroUtility
open Songhay.Dashboard.Client.Components

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

let bodyElements =
    [
        div {
            attr.id ContentBlockComponent.Id

            rootComp<ContentBlockComponent>
        }
        boleroScript
        footerElement
        newLine
    ]
