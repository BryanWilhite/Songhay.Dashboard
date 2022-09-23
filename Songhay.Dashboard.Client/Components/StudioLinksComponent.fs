namespace Songhay.Dashboard.Client.Components

open System

open Microsoft.AspNetCore.Components
open Microsoft.AspNetCore.Components.Routing
open Microsoft.JSInterop

open Bolero
open Bolero.Html

open Songhay.Modules.Models
open Songhay.Modules.Bolero.Models
open Songhay.Modules.Bolero.Visuals.Bulma.CssClass
open Songhay.Modules.Bolero.Visuals.Bulma.Component
open Songhay.Modules.Bolero.Visuals.Bulma.Element
open Songhay.Modules.Bolero.Visuals.Bulma.Layout
open Songhay.Modules.Bolero.Visuals.BodyElement

open Songhay.Dashboard.Client
open Songhay.Dashboard.Client.App.Colors
open Songhay.Dashboard.Client.ElmishTypes

type StudioLinksComponent() =
    inherit ElmishComponent<Model, Message>()

    static let linkNodes =
        let linkNode (title: DisplayText, href: Uri, id: Identifier) =
            bulmaPanelBlockAnchor
                DefaultState
                (HasClasses (CssClasses [ bulmaBackgroundGreyDarkTone ]))
                href TargetBlank
                (concat {
                    bulmaPanelIcon
                        (HasClasses(CssClasses [m (B, L3)]))
                        (bulmaImageContainer
                            (Square Square24)
                            NoAttr
                            (svgElement (bulmaIconSvgViewBox Square24) (SonghaySvgData.Get(id))))
                    text title.Value
                })

        forEach App.appStudioLinks <| linkNode

    static let routeNodes =
        let routeData = [
            (StudioFeedsPage, "studio feeds")
            (StudioToolsPage, "studio tools")
        ]

        let routeNode (page, caption) =
            let id = SonghaySvgKeys.MDI_LINK_VARIANT_24PX.ToAlphanumeric

            bulmaPanelBlockNavLink
                (HasClasses(CssClasses [ bulmaBackgroundGreyDarkTone ]))
                (ElmishRoutes.router.Link page)
                NavLinkMatch.All
                (concat {
                    bulmaPanelIcon
                        (HasClasses(CssClasses [m (B, L3)]))
                        (bulmaImageContainer
                            (Square Square24)
                            NoAttr
                            (svgElement (bulmaIconSvgViewBox Square24) (SonghaySvgData.Get(id))))
                    text caption
                })

        forEach routeData <| routeNode

    static let studioLinksNode =
        let panelNode =
            bulmaPanel
                "studio links"
                NoNode
                (concat { routeNodes; linkNodes })

        bulmaTile
            HSizeAuto
            (HasClasses (CssClasses [ tileIsChild ]))
            panelNode

    static member EComp model dispatch =
        ecomp<StudioLinksComponent, _, _> model dispatch { attr.empty() }

    [<Inject>]
    member val JSRuntime = Unchecked.defaultof<IJSRuntime> with get, set

    override this.View _ _ =
        bulmaTile
            HSizeAuto
            NoCssClasses
            (bulmaTile
                HSizeAuto
                (HasClasses (CssClasses [tileIsParent]))
                studioLinksNode)
