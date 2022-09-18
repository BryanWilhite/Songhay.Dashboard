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
    let headElements (rootCompId: string) =
        [
            HtmlCharSet.ToMetaElement
            HtmlMetaElement.ViewPortInitialScale1.ToMetaElement
        ]
        @
        [ baseElement None ]
        @
        [
            linkRelElement
                RelStylesheet
                NoAttrs
                (($"css/{rootCompId}.min.css", UriKind.Relative) |> Uri)
            linkRelElement
                RelIcon
                NoAttrs
                (("favicon.ico", UriKind.Relative) |> Uri)
        ]
        @
        [ script { attr.src "js/songhay.min.js" } ]
        @
        [ App.AppTitle |> titleElement ]
        @
        [ newLine ]

    let headElement (rootCompId: string) = rootCompId |> headElements |> headElement

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
        [
            svgSymbolsBlock
            rootCompContainer
            boleroScript
            footerElement
                (HasClasses (CssClasses [ nameof footer ]))
                [ footerNode ]
            newLine
        ]

    let docElements (rootCompId: string) (boleroScript: Node) (rootCompContainer: Node) =
        [
            headElement rootCompId
            body { forEach ((boleroScript, rootCompContainer) ||> bodyElements |> wrapn 2) <| id }
            newLine
        ]
