namespace Songhay.Dashboard.Client.Visuals

open System

open Bolero
open Bolero.Html

open Songhay.Modules.Bolero.BoleroUtility
open Songhay.Modules.Bolero.Models
open Songhay.Modules.Bolero.Visuals.BodyElement
open Songhay.Modules.Bolero.Visuals.HeadElement

open Songhay.Dashboard.Client

module HtmlDocument =
    let headChildElements (rootCompId: string) =
        concat {
            newLine; indent 2
            HtmlCharSet.ToMetaElement

            newLine; indent 2
            HtmlMetaElement.ViewPortInitialScale1.ToMetaElement

            newLine; indent 2
            baseElement None

            newLine; indent 2
            linkRelElement
                RelStylesheet
                NoAttrs
                (($"css/{rootCompId}.min.css", UriKind.Relative) |> Uri)

            newLine; indent 2
            linkRelElement
                RelIcon
                NoAttrs
                (("favicon.ico", UriKind.Relative) |> Uri)

            newLine; indent 2
            script { attr.src "js/songhay.min.js" }

            newLine; indent 2
            App.AppTitle |> titleElement

            newLine
        }

    let footerNode =
        div {
            [ "content"; "has-text-centered"; "is-small" ] |> CssClasses.toHtmlClassFromList
            p {
                span {
                    "copyright" |> CssClasses.toHtmlClass
                    rawHtml $"© Bryan D. Wilhite {DateTime.Now.Year}"
                }
            }
        }

    let bodyElements (boleroScript: Node) (rootCompContainer: Node) =
        let svgSymbolsBlock = SonghaySvgData.svgDataArray |> svgSymbolsContainer
        concat {
            newLine; indent 2
            svgSymbolsBlock

            newLine; indent 2
            rootCompContainer

            newLine; indent 2
            boleroScript

            newLine; indent 2
            footerElement
                (HasClasses (CssClasses [ nameof footer ]))
                footerNode

            newLine
        }

    let docElements (rootCompId: string) (boleroScript: Node) (rootCompContainer: Node) =
        concat {
            newLine; indent 1
            head { headChildElements rootCompId }

            newLine; indent 1
            body { (boleroScript, rootCompContainer) ||> bodyElements }

            newLine
        }
