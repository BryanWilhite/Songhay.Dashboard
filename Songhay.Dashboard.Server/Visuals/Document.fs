namespace Songhay.Dashboard.Server.Visuals

open Bolero.Html
open Bolero.Server.Html

open Songhay.Modules.Bolero.BoleroUtility

open Songhay.Dashboard.Client.Components
open Songhay.Dashboard.Client.Visuals.Document

module Document =

    let rootCompContainer =
            div {
                attr.id ContentBlockComponent.Id

                rootComp<ContentBlockComponent>
            }

    let document =
        doctypeHtml { forEach ((ContentBlockComponent.Id, boleroScript, rootCompContainer) |||> docElements |> wrapn 1) <| id }
