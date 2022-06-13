module Songhay.Dashboard.Client.Visuals.Block.StudioLinks

open System
open Microsoft.AspNetCore.Components.Routing
open Bolero.Html

open Songhay.Modules.Models
open Songhay.Modules.Bolero.BoleroUtility
open Songhay.Modules.Bolero.Visuals.Svg

open Songhay.Dashboard.Client
open Songhay.Dashboard.Client.ElmishTypes

let bulmaPanelIcon (id: Identifier) =
    span {
        [ "panel-icon"; "image"; "is-24x24" ] |> toHtmlClassFromList
        svgNode (svgViewBoxSquare 24) svgData[id]
    }

let linkNodes =
    let linkNode (title: DisplayText, href: Uri, id: Identifier) =
        a {
            "panel-block" |> toHtmlClass
            attr.href href.OriginalString
            attr.target "_blank"

            bulmaPanelIcon id; text title.Value
        }

    [ forEach App.appStudioLinks <| linkNode ]

let routeNodes =
    let routeData = [
        (StudioFeedsPage, "studio feeds")
        (StudioToolsPage, "studio tools")
    ]

    let routeNode (page, caption) =
        navLink NavLinkMatch.All {
            "ActiveClass" => "is-active"
            "panel-block" |> toHtmlClass
            attr.href (ElmishRoutes.router.Link page)
            bulmaPanelIcon Keys.MDI_LINK_VARIANT_24PX.ToAlphanumeric; text caption
        }

    [ forEach routeData <| routeNode ]

let studioLinksNode =

    let navNodes = routeNodes @ linkNodes

    nav {
        "panel" |> App.appBlockChildCssClasses.Prepend |> toHtmlClassFromData

        p { "panel-heading" |> toHtmlClass; text "studio links" }

        forEach navNodes <| id
    }
