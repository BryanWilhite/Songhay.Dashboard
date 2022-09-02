namespace Songhay.Dashboard.Server.Visuals

open Bolero.Html
open Bolero.Server.Html

open Songhay.Modules.Bolero.BoleroUtility
open Songhay.Modules.Bolero.Visuals

open Songhay.Dashboard.Client.Components
open Songhay.Dashboard.Client.Visuals.Document

module Document =

    let bodyElements =
        let svgSymbolsBlock = Svg.svgDataArray |> Svg.svgSymbolsBlock
        [
            svgSymbolsBlock
            div {
                attr.id ContentBlockComponent.Id

                rootComp<ContentBlockComponent>
            }
            boleroScript
            footerElement
            newLine
        ]

    let docElements =
        [
            headElement ContentBlockComponent.Id
            body { forEach (bodyElements |> wrapn 2) <| id }
            newLine
        ]

    let document = doctypeHtml { forEach (docElements |> wrapn 1) <| id }
