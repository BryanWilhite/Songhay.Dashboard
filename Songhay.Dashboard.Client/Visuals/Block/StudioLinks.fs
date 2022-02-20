module Songhay.Dashboard.Client.Visuals.Block.StudioLinks

open System
open Microsoft.AspNetCore.Components.Routing
open Bolero.Html

open Songhay.Modules.Models
open Songhay.Dashboard.Client
open Songhay.Dashboard.Client.ElmishTypes
open Songhay.Dashboard.Client.Visuals.Svg

let bulmaPanelIcon (id: Identifier) =
    span [
        attr.classes [ "panel-icon" ]
    ] [ svgNode (svgViewBoxSquare 24) svgData[id] ]

let linkNodes =
    let linkNode (title: DisplayText, href: Uri, id: Identifier) =
        a [
            attr.classes [ "panel-block" ]
            attr.href href.OriginalString
            attr.target "_blank"
        ] [ bulmaPanelIcon id; text title.Value ]

    appStudioLinks |> List.map linkNode

let routeNodes =
    let routeData = [
        (StudioFeedsPage, "studio feeds")
        (StudioToolsPage, "studio tools")
    ]

    let routeNode (page, caption) =
        navLink NavLinkMatch.All [
            "ActiveClass" => "is-active"
            attr.classes [ "panel-block" ]
            attr.href (ElmishRoutes.router.Link page)
        ] [ bulmaPanelIcon (Alphanumeric "mdi_link_variant_24px"); text caption ]

    routeData |> List.map routeNode

let studioLinksNode =

    let navNodes = routeNodes @ linkNodes

    nav
        [ attr.classes ([ "panel" ] @ App.appBlockChildCssClasses) ]
        ([ p [ attr.classes [ "panel-heading" ] ] [ text "studio links" ] ] @ navNodes)
