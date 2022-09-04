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

    let elements =
        let indentationLevel = 1
        (indentationLevel , (ContentBlockComponent.Id, boleroScript, rootCompContainer) |||> docElements) ||> wrapn

    let document = doctypeHtml { forEach elements <| id }
