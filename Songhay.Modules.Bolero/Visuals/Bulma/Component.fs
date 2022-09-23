namespace Songhay.Modules.Bolero.Visuals.Bulma

open System
open Microsoft.AspNetCore.Components.Routing
open Microsoft.AspNetCore.Components.Web

open Bolero
open Bolero.Html

open Songhay.Modules.Bolero.Models
open Songhay.Modules.Bolero.Visuals.BodyElement

///<summary>
/// Bulma Components
/// “Advanced multi-part components with lots of possibilities…”
/// — https://bulma.io/documentation/components/
///</summary>
module Component =
    /// <summary>
    /// “A simple breadcrumb component to improve your navigation experience…”
    /// </summary>
    /// <remarks>
    /// 📖 https://bulma.io/documentation/components/breadcrumb/
    ///
    /// See:
    /// - <see cref="BulmaBreadcrumbAlignment" />
    /// - <see cref="BulmaBreadcrumbSeparators" />
    /// - <see cref="BulmaBreadcrumbSize" />
    /// - <see cref="unOrderedList" />
    /// </remarks>
    let bulmaBreadcrumbContainer (moreClasses: CssClassesOrEmpty) (ulNode: Node) =
        nav {
            CssClasses [ "breadcrumb" ] |> moreClasses.ToHtmlClassAttribute
            AriaLabel.ToAttr "breadcrumbs"

            ulNode
        }

    /// <summary>
    /// “The card component comprises several elements that you can mix and match…”
    /// This is the main <see cref="CssClass.card" /> container,
    /// declaring the <see cref="CssClass.cardContent" /> element.
    /// </summary>
    /// <remarks>
    /// 📖 https://bulma.io/documentation/components/card/
    /// </remarks>
    let bulmaCard
        (moreContainerClasses: CssClassesOrEmpty)
        (header: HtmlNodeOrEmpty)
        (image: HtmlNodeOrEmpty)
        (footer: HtmlNodeOrEmpty)
        (moreContentClasses: CssClassesOrEmpty)
        (cardContentNode: Node) =
        let cardContainerClasses = CssClasses [ CssClass.card ]

        div {
            cardContainerClasses |> moreContainerClasses.ToHtmlClassAttribute

            header.Value

            image.Value

            div {
                CssClasses [ CssClass.cardContent ] |> moreContentClasses.ToHtmlClassAttribute

                div {
                    CssClass.content |> CssClasses.toHtmlClass

                    cardContentNode
                }
            }

            footer.Value
        }

    /// <summary>
    /// “The card component comprises several elements that you can mix and match…”
    /// This is the <c>card-header</c> container,
    /// declaring the <c>card-header-title</c> element
    /// with an image <see cref="Node" /> optionally wrapped
    /// by a <c>button</c> of CSS class <c>card-header-icon</c>.
    /// </summary>
    /// <remarks>
    /// 📖 https://bulma.io/documentation/components/card/
    /// </remarks>
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

    /// <summary>
    /// “The card component comprises several elements that you can mix and match…”
    /// This is the <see cref="CssClass.cardImage" /> container.
    /// </summary>
    /// <remarks>
    /// 📖 https://bulma.io/documentation/components/card/
    /// </remarks>
    let bulmaCardImageContainer (moreContainerClasses: CssClassesOrEmpty) (imageNode: Node) =
        figure {
            CssClasses [CssClass.cardImage ] |> moreContainerClasses.ToHtmlClassAttribute

            imageNode
        }

    /// <summary>
    /// “The card component comprises several elements that you can mix and match…”
    /// This is the <c>card-footer</c> container.
    /// </summary>
    /// <remarks>
    /// 📖 https://bulma.io/documentation/components/card/
    /// </remarks>
    let bulmaCardFooter (footerNode: Node) =
        footerElement
            (HasClasses (CssClasses [ "card-footer" ]))
            footerNode

    /// <summary>
    /// “…a container for a dropdown button and a dropdown menu…”
    /// This returns an anchor element of class <c>dropdown-item</c>,
    /// representing a single item of the dropdown.
    /// </summary>
    /// <remarks>
    /// 📖 https://bulma.io/documentation/components/dropdown/
    /// </remarks>
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

    /// <summary>
    /// “…a container for a dropdown button and a dropdown menu…”
    /// This returns a container of class <c>dropdown</c>, the main container,
    /// wrapping <c>div</c> elements of <c>dropdown-trigger</c> and <c>dropdown-menu</c>.
    /// </summary>
    /// <remarks>
    /// 📖 https://bulma.io/documentation/components/dropdown/
    /// </remarks>
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
                    AriaHasPopup.ToAttrWithTrueValue
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

    /// <summary>
    /// “A composable panel, for compact controls…”
    /// This returns a container of class <see cref="CssClass.panel" />,
    /// wrapping <c>panel-heading</c>, followed by the specified <see cref="Node" />
    /// for the <c>panel-tabs</c>
    /// and the specified panel block <see cref="Node" />
    /// for the various <c>panel-block</c> elements generated by:
    /// <see cref="bulmaPanelBlock" />, <see cref="bulmaPanelBlockAnchor" />,
    /// <see cref="bulmaPanelBlockLabel" /> and <see cref="bulmaPanelBlockNavLink" />.
    /// </summary>
    /// <remarks>
    /// 📖 https://bulma.io/documentation/components/panel/
    /// </remarks>
    let bulmaPanel headingText (panelTabsNode: HtmlNodeOrEmpty) (panelBlockNode: Node) =
        nav {
            CssClass.panel |> CssClasses.toHtmlClass

            p { "panel-heading" |> CssClasses.toHtmlClass; text headingText }

            panelTabsNode.Value

            panelBlockNode
        }

    /// <summary>
    /// “A composable panel, for compact controls…”
    /// This returns a <c>panel-block</c>, wrapping the specified child node.
    /// </summary>
    /// <remarks>
    /// 📖 https://bulma.io/documentation/components/panel/
    /// </remarks>
    let bulmaPanelBlock
        (state: HtmlElementActiveOrDefault)
        (moreClasses: CssClassesOrEmpty)
        (childNode: Node) =
        let panelBlockClasses = CssClasses [ "panel-block"; if state = ActiveState then CssClass.elementIsActive ]
        div {
            panelBlockClasses |> moreClasses.ToHtmlClassAttribute

            childNode
        }

    /// <summary>
    /// “A composable panel, for compact controls…”
    /// This returns a <c>panel-block</c>, wrapping an anchor, <c>a</c> element,
    /// wrapping the specified child node.
    /// </summary>
    /// <remarks>
    /// 📖 https://bulma.io/documentation/components/panel/
    /// </remarks>
    let bulmaPanelBlockAnchor
        (state: HtmlElementActiveOrDefault)
        (moreClasses: CssClassesOrEmpty)
        (href: Uri)
        (target: HtmlTargetOrEmpty)
        (childNode: Node) =
        let panelBlockClasses = CssClasses [ "panel-block"; if state = ActiveState then CssClass.elementIsActive ]
        a {
            panelBlockClasses |> moreClasses.ToHtmlClassAttribute
            attr.href href.OriginalString
            target.Value

            childNode
        }

    /// <summary>
    /// “A composable panel, for compact controls…”
    /// This returns a <c>label</c> element of CSS class <c>panel-block</c>,
    /// wrapping the specified child node.
    /// </summary>
    /// <remarks>
    /// 📖 https://bulma.io/documentation/components/panel/
    /// </remarks>
    let bulmaPanelBlockLabel
        (state: HtmlElementActiveOrDefault)
        (moreClasses: CssClassesOrEmpty)
        (childNode: Node) =
        let panelBlockClasses = CssClasses [ "panel-block"; if state = ActiveState then CssClass.elementIsActive ]
        label {
            panelBlockClasses |> moreClasses.ToHtmlClassAttribute

            childNode
        }

    /// <summary>
    /// “A composable panel, for compact controls…”
    /// This calls <see cref="navLink" /> to return an anchor element, <c>a</c>,
    /// of CSS class <c>panel-block</c>.
    /// </summary>
    /// <remarks>
    /// 📖 https://bulma.io/documentation/components/panel/
    /// </remarks>
    let bulmaPanelBlockNavLink
        (moreClasses: CssClassesOrEmpty)
        (location: string)
        (navMatch: NavLinkMatch)
        (childNode: Node) =
        let panelBlockClasses = CssClasses [ "panel-block" ]
        navLink navMatch {
            panelBlockClasses |> moreClasses.ToHtmlClassAttribute
            "ActiveClass" => CssClass.elementIsActive
            attr.href location

            childNode
        }

    /// <summary>
    /// “A composable panel, for compact controls…”
    /// This returns an <c>span</c> element of CSS class <c>panel-icon</c>
    /// to be a child of an element of CSS class <c>panel-block</c>.
    /// </summary>
    /// <remarks>
    /// 📖 https://bulma.io/documentation/components/panel/
    /// </remarks>
    let bulmaPanelIcon (moreClasses: CssClassesOrEmpty) (visualNode: Node) =
        span {
            CssClasses [ "panel-icon" ] |> moreClasses.ToHtmlClassAttribute
            AriaHidden.ToAttrWithTrueValue

            visualNode
        }
