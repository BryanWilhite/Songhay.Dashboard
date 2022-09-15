namespace Songhay.Modules.Bolero.Visuals.Bulma

open System
open Microsoft.AspNetCore.Components.Routing
open Microsoft.AspNetCore.Components.Web

open Bolero
open Bolero.Html

open Songhay.Modules.Bolero.Models
open Songhay.Modules.Bolero.BoleroUtility

///<summary>
/// Bulma Components
/// “Advanced multi-part components with lots of possibilities…”
/// — https://bulma.io/documentation/components/
///</summary>
module Component =
    let bulmaCard
        (moreContainerClasses: CssClassesOrEmpty)
        (image: HtmlNodeOrEmpty)
        (header: HtmlNodeOrEmpty)
        (footer: HtmlNodeOrEmpty)
        (moreContentClasses: CssClassesOrEmpty)
        (cardContentNodes: Node list) =
        let cardContainerClasses = CssClasses [ CssClass.card ]

        div {
            cardContainerClasses |> moreContainerClasses.ToHtmlClassAttribute

            image.Value

            header.Value

            div {
                CssClasses [ CssClass.cardContent ] |> moreContentClasses.ToHtmlClassAttribute

                div {
                    CssClass.content |> CssClasses.toHtmlClass

                    forEach cardContentNodes <| id
                }
            }

            footer.Value
        }

    let bulmaCardImageContainer (moreContainerClasses: CssClassesOrEmpty) (imageNode: Node) =
        figure {
            CssClasses [CssClass.cardImage ] |> moreContainerClasses.ToHtmlClassAttribute

            imageNode
        }

    let bulmaCardHeader (titleNode: Node) (imageNode: HtmlChildNodeOrReplaceDefault) =
        let defaultNode (node: Node) =
            button {
                "card-header-icon" |> CssClasses.toHtmlClass
                AriaLabel.AttrName => "card header command"
                node
            }

        header {
            "card-header" |> CssClasses.toHtmlClass

            p { "card-header-title" |> CssClasses.toHtmlClass; titleNode }
            match imageNode with
            | ReplacementNode node -> node
            | ChildNode node -> node |> defaultNode
        }

    let bulmaCardFooter (footerNodes: Node list) =
        footer {
            "card-footer" |> CssClasses.toHtmlClass

            forEach footerNodes <| id
        }

    let bulmaDropdownItem
        (isActive: bool)
        (callback: MouseEventArgs -> unit)
        (displayText: string) =
        a {
            attr.href "#"
            [
                "dropdown-item"
                if isActive then CssClass.elementIsActive
            ] |> CssClasses.toHtmlClassFromList
            DomElementEvent.Click.PreventDefault
            on.click callback
            text displayText
        }

    let bulmaDropdown
        (isActive: bool)
        (displayText: string)
        (callback: MouseEventArgs -> unit)
        (dropDownContent: Node) =
        let dropdownClasses = CssClasses [
            "dropdown"
            if isActive then CssClass.elementIsActive
        ]
        div {
            dropdownClasses.ToHtmlClassAttribute

            div {
                "dropdown-trigger" |> CssClasses.toHtmlClass

                button {
                    "button" |> CssClasses.toHtmlClass
                    AriaHasPopup.ToAttr
                    AriaControls.AttrName => "dropdown-menu"
                    on.click callback

                    span { text displayText }
                }
            }
            div {
                "dropdown-menu" |> CssClasses.toHtmlClass; "role" => "menu"

                div {
                    "dropdown-content" |> CssClasses.toHtmlClass
                    dropDownContent
                }
            }
        }

    let bulmaPanel headingText (panelTabsNode: HtmlNodeOrEmpty) (panelBlockNodes: Node list) =
        nav {
            CssClass.panel |> CssClasses.toHtmlClass

            p { "panel-heading" |> CssClasses.toHtmlClass; text headingText }

            panelTabsNode.Value

            forEach panelBlockNodes <| id
        }

    let bulmaPanelBlock
        (state: HtmlElementActiveOrDefault)
        (moreClasses: CssClassesOrEmpty)
        (childNode: Node) =
        let panelBlockClasses = CssClasses [ "panel-block"; if state = ActiveState then CssClass.elementIsActive ]
        div {
            panelBlockClasses |> moreClasses.ToHtmlClassAttribute
            childNode
        }

    let bulmaPanelBlockAnchor
        (state: HtmlElementActiveOrDefault)
        (moreClasses: CssClassesOrEmpty)
        (href: Uri)
        (target: HtmlTargetOrEmpty)
        (childNodes: Node list) =
        let panelBlockClasses = CssClasses [ "panel-block"; if state = ActiveState then CssClass.elementIsActive ]
        a {
            panelBlockClasses |> moreClasses.ToHtmlClassAttribute
            attr.href href.OriginalString
            target.Value

            forEach childNodes <| id
        }

    let bulmaPanelBlockLabel
        (state: HtmlElementActiveOrDefault)
        (moreClasses: CssClassesOrEmpty)
        (childNode: Node) =
        let panelBlockClasses = CssClasses [ "panel-block"; if state = ActiveState then CssClass.elementIsActive ]
        label {
            panelBlockClasses |> moreClasses.ToHtmlClassAttribute
            childNode
        }

    let bulmaPanelBlockNavLink
        (moreClasses: CssClassesOrEmpty)
        (location: string)
        (navMatch: NavLinkMatch)
        (childNodes: Node list) =
        let panelBlockClasses = CssClasses [ "panel-block" ]
        navLink navMatch {
            panelBlockClasses |> moreClasses.ToHtmlClassAttribute
            "ActiveClass" => CssClass.elementIsActive
            attr.href location

            forEach childNodes <| id
        }

    let bulmaPanelIcon (moreClasses: CssClassesOrEmpty) (visualNode: Node) =
        span {
            CssClasses [ "panel-icon" ] |> moreClasses.ToHtmlClassAttribute
            AriaHidden.ToAttr

            visualNode
        }
