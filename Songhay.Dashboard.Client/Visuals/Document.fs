namespace Songhay.Dashboard.Client.Visuals

open System

open Bolero
open Bolero.Html

open Songhay.Modules.Bolero.BoleroUtility
open Songhay.Modules.Bolero.Visuals
open Songhay.Dashboard.Client

module Document =

    let metaElements =
        [
            meta { attr.charset "UTF-8" }
            meta { attr.name "viewport"; attr.content "width=device-width, initial-scale=1.0" }
        ]

    let linkElements (rootCompId: string) =
        [
            link { attr.rel "stylesheet"; attr.href $"css/{rootCompId}.min.css" }
            link { attr.rel "icon"; attr.``type`` "image/x-icon"; attr.href "favicon.ico" }
        ]

    let headElements (rootCompId: string) =
        metaElements
        @
        [ ``base`` { attr.href "/" } ]
        @
        (rootCompId |> linkElements)
        @
        [ script { attr.src "js/songhay.min.js" } ]
        @
        [ title { text App.AppTitle } ]

    let headElement (rootCompId: string) =
        head { forEach (rootCompId |> headElements |> wrapn 2) <| id }

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

    let bodyElements (boleroScript: Node) (rootCompContainer: Node) =
        let svgSymbolsBlock = Svg.svgDataArray |> Svg.svgSymbolsBlock
        [
            svgSymbolsBlock
            rootCompContainer
            boleroScript
            footerElement
            newLine
        ]

    let docElements (rootCompId: string) (boleroScript: Node) (rootCompContainer: Node) =
        [
            headElement rootCompId
            body { forEach ((boleroScript, rootCompContainer) ||> bodyElements |> wrapn 2) <| id }
            newLine
        ]
