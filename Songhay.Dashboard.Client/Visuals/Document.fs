namespace Songhay.Dashboard.Client.Visuals

open System

open Bolero
open Bolero.Html

open Songhay.Modules.Bolero.BoleroUtility
open Songhay.Modules.Bolero.Models
open Songhay.Modules.Bolero.Visuals.Element
open Songhay.Modules.Bolero.Visuals.Document

open Songhay.Dashboard.Client

module Document =
    let headElements (rootCompId: string) =
        Head.metaElements
        @
        [(None |> Head.baseElement)]
        @
        (rootCompId |> Head.studioLinkElements)
        @
        [ Head.studioScriptElement ]
        @
        [ App.AppTitle |> Head.titleElement ]

    let headElement (rootCompId: string) = rootCompId |> headElements |> Head.headElement

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
