namespace Songhay.Dashboard.Server.Visuals

open Bolero.Html
open Bolero.Server.Html

open Songhay.Modules.Bolero.BoleroUtility

open Songhay.Dashboard.Client.Components

module HtmlDocument =

    let rootCompContainer =
            div {
                attr.id ContentBlockProgramComponent.Id

                newLine; indent 2
                comp<ContentBlockProgramComponent>
            }

    let document = doctypeHtml {
        (ContentBlockProgramComponent.Id, rootCompContainer) ||> HtmlDocumentComponent.BComp
        boleroScript
    }
