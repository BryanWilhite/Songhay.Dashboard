namespace Songhay.Dashboard.Client.Components

open System

open Bolero
open Bolero.Html

open Microsoft.AspNetCore.Components
open Songhay.Dashboard.Models
open Songhay.Modules.Models
open Songhay.Modules.Bolero.BoleroUtility
open Songhay.Modules.Bolero.Models
open Songhay.Modules.Bolero.SvgUtility
open Songhay.Modules.Bolero.Visuals.BodyElement
open Songhay.Modules.Bolero.Visuals.HeadElement
open Songhay.Modules.Bolero.Visuals.SvgElement
open Songhay.Modules.Bolero.Visuals.Bulma.Component
open Songhay.Modules.Bolero.Visuals.Bulma.Element
open Songhay.Modules.Bolero.Visuals.Bulma.Layout
open Songhay.Modules.Bolero.Visuals.Bulma.CssClass

open Songhay.Dashboard.Client
open Songhay.Dashboard.Client.App.Colors

type HtmlDocumentComponent() =
    inherit Component()

    let headChildElements (rootCompId: string) =
        concat {
            newLine; indent 2
            HtmlCharSet.ToMetaElement

            newLine; indent 2
            HtmlMetaElement.ViewPortInitialScale1.ToMetaElement

            newLine; indent 2
            baseElement None

            newLine; indent 2
            linkRelElement
                RelStylesheet
                NoAttr
                (($"css/{rootCompId}.min.css", UriKind.Relative) |> Uri)

            newLine; indent 2
            linkRelElement
                RelIcon
                NoAttr
                (("favicon.ico", UriKind.Relative) |> Uri)

            newLine; indent 2
            script { attr.src "js/songhay-dashboard.min.js" }

            newLine; indent 2
            App.AppTitle |> titleElement

            newLine
        }

    let studioAnchor (title: DisplayText, href: Uri, id: Identifier) =
        anchorElement
            (HasClasses <| CssClasses [ levelItem; elementTextAlign AlignCentered ])
            (HasAttr <| attrs { attr.href href; TargetBlank.Value ; attr.title title.Value })
            (bulmaIcon
                (((svgViewBoxSquare 24), SonghaySvgData.Get(id)) ||> svgElement))

    let studioLogo = span { (title (HasFontSize Size1)) |> CssClasses.toHtmlClassFromList; text "(::)" }

    let studioLogoContainer (navbarBurger: Node) =
        let spanClasses = (CssClasses <| title (HasFontSize Size2)).Append (hidden Touch)
        concat {
            div {
                [ "logo"; p (LR, L6); p (TB, L3) ] |> CssClasses.toHtmlClassFromList
                attr.title App.AppTitle

                span { ((elementFontWeight Normal) |> spanClasses.Prepend).ToHtmlClassAttribute; text "Songhay" }
                span { spanClasses.ToHtmlClassAttribute; text "System" }
                studioLogo
            }
            navbarBurger
        }

    let svgVersionNode (data: VersionData) =
        let classes = CssClasses [
            levelItem
            bulmaAkyinkyinBase
            elementTextIsUnselectable
            elementTextAlign AlignCentered
        ]

        bulmaLevelItem
            (HasClasses classes)
            (HasAttr (attr.title data.title.Value))
            (concat {
                bulmaIcon (((svgViewBoxSquare 24), SonghaySvgData.Get(data.id)) ||> svgElement)
                span { fontSize Size7 |> CssClasses.toHtmlClass; text data.version }
            })

    let navbarNode =
        let navBarMenuItems =
            let _, _, lastId = App.appStudioLinks |> List.last
            let linkNode (title: DisplayText, href: Uri, id: Identifier) =
                span {
                    [
                        "icon-text"
                        m (All, L3)
                        if lastId = id then p (R, L6)
                    ] |> CssClasses.toHtmlClassFromList
                    attr.title title.Value
                    span {
                        anchorElement
                            (HasClasses <| CssClasses [ "is-flex" ] )
                            (HasAttr <| attrs { attr.href href; TargetBlank.Value ; attr.title title.Value })
                            (concat {
                                bulmaIcon (svgElement (bulmaIconSvgViewBox Square24) (SonghaySvgData.Get(id)))
                                span { "display-text" |> CssClasses.toHtmlClass; text title.Value }
                            })
                    }
                }

            forEach App.appStudioLinks <| linkNode
        let navbarMenuId = ("navMenu" |> Identifier.fromString) |> Some
        bulmaNavbarContainer
            (HasClasses (CssClasses [ NavbarFixedTop.CssClass; bulmaBackgroundGreys ]))
            (studioLogoContainer (bulmaNavbarBurger false navbarMenuId.Value))
            navbarMenuId
            (HasNode (bulmaNavbarMenuEnd NoCssClasses navBarMenuItems))

    let footerNode =
        let cssClassesParentLevel = CssClasses [ levelContainer; isMobileModifier ]
        let cssClassesSvgVersionNodes = [ bulmaTextGreyLightTone ] |> cssClassesParentLevel.AppendList
        let versionsNode =
            div {
                cssClassesSvgVersionNodes.ToHtmlClassAttribute
                forEach App.appVersions <| svgVersionNode
            }
        let socialNode =
            bulmaLevel
                (HasClasses <| CssClasses [ isMobileModifier ])
                (forEach App.appSocialLinks <| studioAnchor)
        let signatureNode =
            concat {
                paragraphElement
                    NoCssClasses
                    NoAttr
                    (anchorElement
                        (HasClasses <| CssClasses [fontSize Size7])
                        (HasAttr <| attrs { TargetBlank.Value; ("privacy.html", UriKind.Relative) |> Uri |> attr.href })
                        (text "Privacy Policy"))
                paragraphElement
                    NoCssClasses
                    NoAttr
                    (anchorElement
                        (HasClasses <| CssClasses [fontSize Size7])
                        (HasAttr <| attrs { TargetBlank.Value; ("https://www.youtube.com/t/terms", UriKind.Absolute) |> Uri |> attr.href })
                        (text "YouTube™ Terms of Service"))
                paragraphElement
                    NoCssClasses
                    NoAttr
                    (span {
                        [ "copyright"; fontSize Size7 ] |> CssClasses.toHtmlClassFromList
                        rawHtml $"© Bryan D. Wilhite {DateTime.Now.Year}"
                    })
            }

        bulmaContainer
            ContainerWidthFluid
            NoCssClasses
            (concat {
                bulmaColumnsContainer
                    NoCssClasses
                    (concat {
                        bulmaColumn
                            (HasClasses <| CssClasses [ HSize4.CssClass; elementTextAlign AlignCentered ])
                            studioLogo
                        bulmaColumn
                            (HasClasses <| CssClasses [ HSize4.CssClass ])
                            socialNode
                        bulmaColumn
                            (HasClasses <| CssClasses [ HSize4.CssClass; elementTextAlign AlignCentered ])
                            signatureNode
                    })
                bulmaColumnsContainer
                    (HasClasses <| CssClasses [ AlignCentered.CssClass ])
                    (concat {
                        bulmaColumn
                            (HasClasses <| CssClasses [ HSize4.CssClass ])
                            versionsNode
                    })
            })

    let bodyElements (rootCompContainer: Node) =
        let svgSymbolsBlock = SonghaySvgData.Array |> svgSymbolsContainer
        concat {
            newLine; indent 2
            svgSymbolsBlock

            newLine; indent 2
            navbarNode

            newLine; indent 2
            rootCompContainer

            newLine; indent 2

            newLine; indent 2
            footerElement
                (HasClasses <| CssClasses [ nameof footer ])
                footerNode

            newLine
        }

    let docElements (rootCompId: string) (rootCompContainer: Node) =
        concat {
            newLine; indent 1
            head { headChildElements rootCompId }

            newLine; indent 1
            body {
                NavbarAncestorHasFixedTop.CssClass |> CssClasses.toHtmlClass

                rootCompContainer |> bodyElements
            }

            newLine
        }

    static member BComp (rootCompId: string) (rootCompContainer: Node) =
        comp<HtmlDocumentComponent> {
            "ContentBlockProgramComponentId" => rootCompId
            "RootCompContainer" => rootCompContainer
        }

    [<Parameter>]
    member val ContentBlockProgramComponentId = String.Empty with get, set

    [<Parameter>]
    member val RootCompContainer = empty() with get, set

    override this.Render() =
        (this.ContentBlockProgramComponentId, this.RootCompContainer) ||> docElements