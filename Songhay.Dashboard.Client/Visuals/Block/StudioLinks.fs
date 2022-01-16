module Songhay.Dashboard.Client.Visuals.Block.StudioLinks

open Microsoft.AspNetCore.Components.Routing
open Bolero.Html
open Songhay.Dashboard.Client
open Songhay.Dashboard.Client.Models
open Songhay.Dashboard.Client.ElmishTypes
open Songhay.Dashboard.Client.Visuals

let bulmaPanelIcon (id: string) =
    span [
        attr.classes [ "panel-icon" ]
    ] [ Svg.svgSpriteNode $"./#{id}" App.appSvgViewBox ]

let linkNodes =
    let linkNode (data: SvgSpriteData) =
        a [
            attr.classes [ "panel-block" ]
            attr.href data.href
            attr.target "_blank"
        ] [ bulmaPanelIcon data.id; text data.title ]

    App.appStudioLinks |> List.map linkNode

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
        ] [ bulmaPanelIcon "mdi_link_variant_24px"; text caption ]

    routeData |> List.map routeNode

let studioLinksNode =

    let navNodes = routeNodes @ linkNodes

    nav
        [ attr.classes ([ "panel" ] @ App.appBlockChildCssClasses) ]
        ([ p [ attr.classes [ "panel-heading" ] ] [ text "studio links" ] ] @ navNodes)
