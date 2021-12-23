module Songhay.Dashboard.Client.Visuals.Block.StudioLinks

open Bolero.Html
open Songhay.Dashboard.Client.ElmishRoutes
open Songhay.Dashboard.Client.ElmishTypes

let studioLinksNode =
    let data = [
        (Page.StudioFeedsPage, "studio feeds")
        (Page.StudioToolsPage, "studio tools")
    ]

    let node (page, caption) =
        a [
            attr.classes [ "panel-block" ]
            attr.href (router.Link page)
        ] [ text caption ]

    let nodes = data |> List.map node

    nav
        [ attr.classes [ "panel"; "has-background-greys-dark-tone"; "is-child"; "tile" ] ]
        ([ p [ attr.classes [ "panel-heading" ] ] [ text "studio links" ] ] @ nodes)
