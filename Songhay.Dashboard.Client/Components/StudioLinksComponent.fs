namespace Songhay.Dashboard.Client.Components

open System

open Microsoft.AspNetCore.Components
open Microsoft.AspNetCore.Components.Routing
open Microsoft.JSInterop

open Bolero
open Bolero.Html

open Songhay.Modules.Models
open Songhay.Modules.Bolero.BoleroUtility
open Songhay.Modules.Bolero.Visuals.Bulma.Tile
open Songhay.Modules.Bolero.Visuals.Svg

open Songhay.Dashboard.Client
open Songhay.Dashboard.Client.ElmishTypes

type StudioLinksComponent() =
    inherit ElmishComponent<Model, Message>()

    static let bulmaPanelIcon (id: Identifier) =
        span {
            [ "panel-icon"; "image"; "is-24x24" ] |> toHtmlClassFromList
            svgNode (svgViewBoxSquare 24) svgData[id]
        }

    static let linkNodes =
        let linkNode (title: DisplayText, href: Uri, id: Identifier) =
            a {
                "panel-block" |> toHtmlClass
                attr.href href.OriginalString
                attr.target "_blank"

                bulmaPanelIcon id; text title.Value
            }

        [ forEach App.appStudioLinks <| linkNode ]

    static let routeNodes =
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

    static let studioLinksNode =

        let navNodes = routeNodes @ linkNodes

        nav {
            "panel" |> App.appBlockChildCssClasses.Prepend |> toHtmlClassFromData

            p { "panel-heading" |> toHtmlClass; text "studio links" }

            forEach navNodes <| id
        }

    static member EComp model dispatch =
        ecomp<StudioLinksComponent, _, _> model dispatch { attr.empty() }

    [<Inject>]
    member val JSRuntime = Unchecked.defaultof<IJSRuntime> with get, set

    override this.View _ _ = bulmaColumnTile 0 [ bulmaContentParentTile false [ studioLinksNode ] ]
