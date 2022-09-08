namespace Songhay.Dashboard.Client.Components

open System

open Microsoft.AspNetCore.Components
open Microsoft.AspNetCore.Components.Routing
open Microsoft.JSInterop

open Bolero
open Bolero.Html

open Songhay.Modules.Models
open Songhay.Modules.Bolero.BoleroUtility
open Songhay.Modules.Bolero.Models
open Songhay.Modules.Bolero.Visuals.Bulma.CssClass
open Songhay.Modules.Bolero.Visuals.Bulma.Component
open Songhay.Modules.Bolero.Visuals.Bulma.Element
open Songhay.Modules.Bolero.Visuals.Bulma.Layout
open Songhay.Modules.Bolero.Visuals.Svg

open Songhay.Dashboard.Client
open Songhay.Dashboard.Client.Visuals.Colors
open Songhay.Dashboard.Client.ElmishTypes

type StudioLinksComponent() =
    inherit ElmishComponent<Model, Message>()

    static let linkNodes =
        let linkNode (title: DisplayText, href: Uri, id: Identifier) =
            bulmaPanelBlockAnchor
                DefaultState
                (HasClasses (CssClasses [ bulmaBackgroundGreyDarkTone ]))
                href TargetBlank
                [ bulmaPanelIcon id; text title.Value ]

        [ forEach App.appStudioLinks <| linkNode ]

    static let routeNodes =
        let routeData = [
            (StudioFeedsPage, "studio feeds")
            (StudioToolsPage, "studio tools")
        ]

        let routeNode (page, caption) =
            bulmaPanelBlockNavLink
                (HasClasses(CssClasses [ bulmaBackgroundGreyDarkTone ]))
                (ElmishRoutes.router.Link page)
                NavLinkMatch.All
                [ bulmaPanelIcon Keys.MDI_LINK_VARIANT_24PX.ToAlphanumeric; text caption ]

        [ forEach routeData <| routeNode ]

    static let studioLinksNode =
        let panelNode =
            bulmaPanel
                "studio links"
                NoNode
                (routeNodes @ linkNodes)

        bulmaTile
            TileSizeAuto
            (HasClasses (CssClasses [ tileIsChild ]))
            [ panelNode ]

    static member EComp model dispatch =
        ecomp<StudioLinksComponent, _, _> model dispatch { attr.empty() }

    [<Inject>]
    member val JSRuntime = Unchecked.defaultof<IJSRuntime> with get, set

    override this.View _ _ =
        bulmaTile
            TileSizeAuto
            NoCssClasses
            [
                bulmaTile
                    TileSizeAuto
                    (HasClasses (CssClasses [tileIsParent]))
                    [ studioLinksNode ]
            ]
