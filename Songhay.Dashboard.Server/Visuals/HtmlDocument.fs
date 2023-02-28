namespace Songhay.Dashboard.Server.Visuals

open Bolero.Html
open Bolero.Server.Html

open Songhay.Modules.Bolero.BoleroUtility

open Songhay.Dashboard.Client.Components
open Songhay.Dashboard.Client.Visuals.HtmlDocument

module HtmlDocument =

    let rootCompContainer =
            div {
                attr.id ContentBlockComponent.Id

                newLine; indent 2
                comp<ContentBlockComponent>
            }

    let document = doctypeHtml {
        (ContentBlockComponent.Id, rootCompContainer) ||> docElements
        boleroScript
    }
