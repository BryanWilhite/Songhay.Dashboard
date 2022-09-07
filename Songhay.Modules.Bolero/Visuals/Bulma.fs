namespace Songhay.Modules.Bolero.Visuals

open System
open Microsoft.AspNetCore.Components.Web

open Bolero
open Bolero.Html

open Songhay.Modules.Models
open Songhay.Modules.Bolero.Models
open Songhay.Modules.Bolero.BoleroUtility
open Songhay.Modules.Bolero.Visuals.Svg

///<summary>
/// Bulma CSS modules.
///</summary>
///<remarks>
/// https://bulma.io/documentation/overview/
///</remarks>
module Bulma =

    ///<summary>
    /// Bulma CSS class-name functions and literals.
    ///</summary>
    module CssClass =

        ///<summary>
        /// Bulma CSS class-name literal for Bulma cards.
        ///</summary>
        ///<remarks>
        /// “The card component comprises several elements that you can mix and match…”
        /// — https://bulma.io/documentation/components/card/
        ///</remarks>
        [<Literal>]
        let card = "card"

        ///<summary>
        /// Bulma CSS class-name literal for Bulma cards.
        ///</summary>
        ///<remarks>
        /// This is the container/wrapper for the <see cref="content" /> block.
        /// — https://bulma.io/documentation/components/card/
        ///</remarks>
        [<Literal>]
        let cardContent = "card-content"

        ///<summary>
        /// Bulma CSS class-name literal for Bulma cards.
        ///</summary>
        ///<remarks>
        /// This is the container/wrapper for the Bulma <see cref="image" />.
        /// — https://bulma.io/documentation/components/card/
        ///</remarks>
        [<Literal>]
        let cardImage = "card-image"

        ///<summary>
        /// Bulma CSS class-name literal for Bulma content.
        ///</summary>
        ///<remarks>
        /// Both the card component and the media layout use this class name.
        ///</remarks>
        [<Literal>]
        let content = "content"

        ///<summary>
        /// Bulma CSS class-name literal for Bulma layout.
        ///</summary>
        ///<remarks>
        /// “…a simple utility element that allows you to center content on larger viewports.
        /// It can be used in any context, but mostly as a direct child
        /// of one of the following:
        /// • <c>.navbar</c>
        /// • <c>.hero</c>
        /// • <c>.section</c>
        /// • <c>.footer</c>
        /// ”
        /// — https://bulma.io/documentation/layout/container/
        ///</remarks>
        [<Literal>]
        let container = "container"

        ///<summary>
        /// Bulma CSS class-name function for typography.
        ///</summary>
        ///<remarks>
        /// — https://bulma.io/documentation/helpers/typography-helpers/#font-family
        ///</remarks>
        let elementFontFamily (family: CssFontFamily) =
            let suffix =
                match family with
                | SansSerif | Monospace | Primary | Secondary -> family.Value
                | _ -> "code"

            $"is-family-{suffix}"

        ///<summary>
        /// Bulma CSS class-name function for typography.
        ///</summary>
        ///<remarks>
        /// — https://bulma.io/documentation/helpers/typography-helpers/#text-weight
        ///</remarks>
        let elementFontWeight (weight: CssFontWeight) = $"has-text-weight-{weight.Value}"

        ///<summary>
        /// Bulma CSS class-name literal.
        ///</summary>
        [<Literal>]
        let elementIsActive = "is-active"

        ///<summary>
        /// Bulma CSS class-name literal.
        ///</summary>
        ///<remarks>
        /// Applies <c>cursor: pointer !important</c> to the element.
        ///</remarks>
        [<Literal>]
        let elementIsClickable = "is-clickable"

        ///<summary>
        /// Bulma CSS class-name literal.
        ///</summary>
        [<Literal>]
        let elementIsBlock = "is-block"

        ///<summary>
        /// Bulma CSS class-name literal.
        ///</summary>
        ///<remarks>
        /// Adds overflow hidden
        ///</remarks>
        [<Literal>]
        let elementIsClipped = "is-clipped"

        ///<summary>
        /// Bulma CSS class-name literal.
        ///</summary>
        [<Literal>]
        let elementIsHidden = "is-hidden"

        ///<summary>
        /// Bulma CSS class-name literal.
        ///</summary>
        ///<remarks>
        /// Hide elements visually but keep the element available to be announced by a screen reader
        ///</remarks>
        [<Literal>]
        let elementIsHiddenVisually = "is-sr-only"

        ///<summary>
        /// Bulma CSS class-name literal.
        ///</summary>
        [<Literal>]
        let elementIsInlineBlock = "is-inline-block"

        ///<summary>
        /// Bulma CSS class-name literal.
        ///</summary>
        ///<remarks>
        /// Adds visibility hidden
        ///</remarks>
        [<Literal>]
        let elementIsInvisible = "is-invisible"

        ///<summary>
        /// Bulma CSS class-name literal.
        ///</summary>
        ///<remarks>
        /// Completely covers the first positioned parent
        ///</remarks>
        [<Literal>]
        let elementIsOverlay = "is-overlay"

        ///<summary>
        /// Bulma CSS class-name literal.
        ///</summary>
        [<Literal>]
        let elementIsRelative = "is-relative"

        ///<summary>
        /// Bulma CSS class-name function for typography.
        ///</summary>
        ///<remarks>
        /// — https://bulma.io/documentation/helpers/typography-helpers/#alignment
        ///</remarks>
        let elementTextAlign (alignment: CssAlignment) =
            let suffix =
                match alignment with
                | Center -> "centered"
                | Justify -> "justified"
                | Left | Right -> alignment.Value
                | _ -> "left"

            $"has-text-{suffix}"

        ///<summary>
        /// Bulma CSS class-name function for typography.
        ///</summary>
        ///<remarks>
        /// — https://bulma.io/documentation/helpers/typography-helpers/#text-transformation
        ///</remarks>
        let elementTextTransformation (transformation: CssTextTransformation) =
            let suffix =
                match transformation with
                | TitleCase -> "capitalized"
                | Underline -> "underlined"
                | _ -> transformation.Value

            $"is-{suffix}"

        ///<summary>
        /// Bulma CSS class-name literal.
        ///</summary>
        ///<remarks>
        /// Prevents the text from being selectable
        ///</remarks>
        [<Literal>]
        let elementTextIsUnselectable = "is-unselectable"

        ///<summary>
        /// Bulma CSS class-name function.
        ///</summary>
        let fontSize (size: BulmaFontSize) = $"is-size-{size.Value}"

        ///<summary>
        /// Bulma CSS class-name function.
        ///</summary>
        let hidden (breakpoint: BulmaBreakpoint) = $"is-hidden-{breakpoint.Value}"

        ///<summary>
        /// Bulma CSS class-name for Bulma elements.
        ///</summary>
        ///<remarks>
        /// “A container for responsive images…”
        /// — https://bulma.io/documentation/elements/image/
        ///</remarks>
        [<Literal>]
        let image = "image"
 
        ///<summary>
        /// Bulma CSS class-name function for Bulma elements.
        ///</summary>
        ///<remarks>
        /// Returns the <see cref="image" /> CSS class name
        /// with <see cref="BulmaRatioDimension.CssClass" />.
        /// — https://bulma.io/documentation/elements/image/
        ///</remarks>
        let imageContainer (dimension: BulmaRatioDimension) = [ image; dimension.CssClass ]

        ///<summary>
        /// Bulma CSS class-name literal.
        ///</summary>
        [<Literal>]
        let imageIsRounded = "is-rounded"

        ///<summary>
        /// Bulma CSS class-name literal.
        ///</summary>
        ///<remarks>
        /// “By default, columns are only activated on tablet and desktop.
        /// If you want to use columns on mobile too,
        /// add the is-mobile modifier on the columns container.”
        /// — https://github.com/jgthms/bulma/blob/master/docs/_posts/2016-02-09-blog-launched-new-responsive-columns-new-helpers.md
        ///</remarks>
        [<Literal>]
        let isMobileModifier = "is-mobile"

        ///<summary>
        /// Bulma CSS class-name function for layout.
        ///</summary>
        ///<remarks>
        /// “A multi-purpose horizontal level, which can contain almost any other element…”
        /// — https://bulma.io/documentation/layout/level/
        ///</remarks>
        [<Literal>]
        let levelContainer = "level"

        ///<summary>
        /// Bulma CSS class-name function for layout.
        ///</summary>
        ///<remarks>
        /// Either <c>level-left</c> or <c>level-right</c>.
        /// — https://bulma.io/documentation/layout/level/
        ///</remarks>
        let level (alignment: CssAlignment) =
            match alignment with
            | Left | Right -> $"level-{alignment.Value}"
            | _ -> "level-left"

        ///<summary>
        /// Bulma CSS class-name function for layout.
        ///</summary>
        ///<remarks>
        /// “In a level-item, you can then insert almost anything you want:
        /// a title, a button, a text input, or just simple text.
        /// No matter what elements you put inside a Bulma level,
        /// they will always be vertically centered.”
        /// — https://bulma.io/documentation/layout/level/
        ///</remarks>
        [<Literal>]
        let levelItem = "level-item"

        ///<summary>
        /// Bulma CSS class-name function.
        ///</summary>
        let m (box: CssBoxModel, suffix: BulmaValueSuffix) = $"m{box.Value}-{suffix.Value}"

        ///<summary>
        /// Bulma CSS class-name literal for Bulma layout.
        ///</summary>
        ///<remarks>
        /// “The famous media object prevalent in social media interfaces, but useful in any context…”
        /// — https://bulma.io/documentation/layout/media-object/
        ///</remarks>
        [<Literal>]
        let media = "media"

        ///<summary>
        /// Bulma CSS class-name literal for Bulma layout.
        ///</summary>
        ///<remarks>
        /// Indicates the leftmost container aside the <c>media-content</c> block
        /// usually containing an avatar, ‘branding’ the media content.
        /// — https://bulma.io/documentation/layout/media-object/
        ///</remarks>
        [<Literal>]
        let mediaLeft = "media-left"

        ///<summary>
        /// Bulma CSS class-name literal for Bulma layout.
        ///</summary>
        ///<remarks>
        /// The container for “any other Bulma element, like inputs, textareas, icons, buttons…”
        /// — https://bulma.io/documentation/layout/media-object/
        ///</remarks>
        let mediaContent ="media-content"

        ///<summary>
        /// Bulma CSS class-name function.
        ///</summary>
        let p (box: CssBoxModel, suffix: BulmaValueSuffix) = $"p{box.Value}-{suffix.Value}"

        ///<summary>
        /// Bulma CSS class-name literal for Bulma panels.
        ///</summary>
        ///<remarks>
        /// “A composable panel, for compact controls…”
        /// — https://bulma.io/documentation/components/panel/
        ///</remarks>
        [<Literal>]
        let panel = "panel"

        ///<summary>
        /// Bulma CSS class-name literal for Bulma tiles.
        ///</summary>
        ///<remarks>
        /// “To build intricate 2-dimensional layouts, you only need a single element: the tile…”
        /// — https://bulma.io/documentation/layout/tiles/
        ///</remarks>
        [<Literal>]
        let tile = "tile"

        ///<summary>
        /// Bulma CSS class-name literal for Bulma tiles.
        ///</summary>
        ///<remarks>
        /// “Start with an ancestor tile that will wrap all other tiles…”
        /// — https://bulma.io/documentation/layout/tiles/
        ///</remarks>
        [<Literal>]
        let tileIsAncestor = "is-ancestor"

        ///<summary>
        /// Bulma CSS class-name literal for Bulma tiles.
        ///</summary>
        ///<remarks>
        /// “As soon as you want to add content to a tile, just:
        ///
        /// - add any class you want, like <c>box</c>
        /// - add the <c>is-child</c> modifier on the tile
        /// - add the <c>is-parent</c> modifier on the parent tile”
        /// — https://bulma.io/documentation/layout/tiles/
        ///</remarks>
        [<Literal>]
        let tileIsChild = "is-child"

        ///<summary>
        /// Bulma CSS class-name literal for Bulma tiles.
        ///</summary>
        ///<remarks>
        /// “As soon as you want to add content to a tile, just:
        ///
        /// - add any class you want, like <c>box</c>
        /// - add the <c>is-child</c> modifier on the tile
        /// - add the <c>is-parent</c> modifier on the parent tile”
        /// — https://bulma.io/documentation/layout/tiles/
        ///</remarks>
        [<Literal>]
        let tileIsParent = "is-parent"

        ///<summary>
        /// Bulma CSS class-name literal for Bulma tiles.
        ///</summary>
        ///<remarks>
        /// “If you want to stack tiles vertically, add is-vertical on the parent tile…”
        /// — https://bulma.io/documentation/layout/tiles/
        ///</remarks>
        [<Literal>]
        let tileIsVertical = "is-vertical"

    ///<summary>
    /// Bulma Elements
    /// “Essential interface elements that only require a single CSS class…”
    /// — https://bulma.io/documentation/elements/
    ///</summary>
    module Element = 
        let bulmaAnchorIconButton (title: DisplayText, href: Uri, id: Identifier) =
            a {
                (CssClasses [ CssClass.levelItem; CssClass.elementTextAlign Center ]).ToHtmlClassAttribute
                attr.href href.OriginalString
                attr.target "_blank"
                attr.title title.Value

                span {
                    "icon" |> toHtmlClass
                    "aria-hidden" => "true"

                    svgNode (svgViewBoxSquare 24) svgData[id]
                }
            }

        let bulmaPanelIcon (id: Identifier) =
            span {
                [ "panel-icon" ] @ CssClass.imageContainer (Square Square24) |> toHtmlClassFromList
                svgNode (svgViewBoxSquare 24) svgData[id]
            }

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
                        CssClass.content |> toHtmlClass

                        forEach cardContentNodes <| id
                    }
                }

                footer.Value
            }

        let bulmaCardImage (imageNode: Node) =
            div {
                CssClass.cardImage |> toHtmlClass

                imageNode
            }

        let bulmaCardHeader (titleNode: Node) (imageNode: HtmlChildNodeOrReplaceDefault) =
            let defaultNode (node: Node) =
                button {
                    "card-header-icon" |> toHtmlClass
                    "aria-label" => "card header command"
                    node
                }

            header {
                "card-header" |> toHtmlClass

                p { "card-header-title" |> toHtmlClass; titleNode }
                match imageNode with
                | ReplacementNode node -> node
                | ChildNode node -> node |> defaultNode
            }

        let bulmaCardFooter (footerNodes: Node list) =
            footer {
                "card-footer" |> toHtmlClass

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
                ] |> toHtmlClassFromList
                GlobalEventHandlers.OnClick.PreventDefault
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
                    "dropdown-trigger" |> toHtmlClass

                    button {
                        "button" |> toHtmlClass
                        "aria-haspopup" => "true"; "aria-controls" => "dropdown-menu"
                        on.click callback

                        span { text displayText }
                    }
                }
                div {
                    "dropdown-menu" |> toHtmlClass; "role" => "menu"

                    div {
                        "dropdown-content" |> toHtmlClass
                        dropDownContent
                    }
                }
            }

        let bulmaPanelBlock isActive (childNode: Node) =
            div { [ "panel-block"; if isActive then CssClass.elementIsActive ] |> toHtmlClassFromList; childNode }

        let bulmaPanelBlockAnchor isActive (childNode: Node) =
            a { [ "panel-block"; if isActive then CssClass.elementIsActive ] |> toHtmlClassFromList; childNode }

        let bulmaPanelBlockLabel isActive (childNode: Node) =
            label { [ "panel-block"; if isActive then CssClass.elementIsActive ] |> toHtmlClassFromList; childNode }

        let bulmaPanelHeading displayText =
            p { "panel-heading" |> toHtmlClass; text displayText }

        let bulmaPanel headingText (panelTabsNode: HtmlNodeOrEmpty) (panelBlockNodes: Node list) =
            nav {
                CssClass.panel |> toHtmlClass

                p { "panel-heading" |> toHtmlClass; text headingText }

                panelTabsNode.Value

                forEach panelBlockNodes <| id
            }

    ///<summary>
    /// Bulma Layout
    /// “Design the structure of your webpage…”
    /// — https://bulma.io/documentation/layout/
    ///</summary>
    module Layout =
        let bulmaLoader (margin: CssMargin * BulmaValueSuffix) (padding: CssPadding * BulmaValueSuffix) =

            let marginClass = match margin with | CssMargin b, s -> CssClass.m (b, s) 
            let paddingClass = match padding with | CssPadding b, s -> CssClass.m (b, s) 

            div {
                [ "loader-container"; paddingClass; CssClass.elementTextAlign Center] |> toHtmlClassFromList

                div {
                    [ "loader"; marginClass ] @ CssClass.imageContainer (Square Square128) |> toHtmlClassFromList

                    attr.title "Loading…"
                }
            }

        let bulmaMedia (moreClasses: CssClassesOrEmpty) (mediaLeft: HtmlNodeOrEmpty) (mediaContentNodes: Node list) =
            let mediaContainerClasses = CssClasses [ CssClass.media ]

            div {
                mediaContainerClasses |> moreClasses.ToHtmlClassAttribute

                mediaLeft.Value

                div {
                    CssClass.mediaContent |> toHtmlClass

                    forEach mediaContentNodes <| id
                }
            }

        let bulmaTile (width: BulmaTileHorizontalSize) (moreClasses: CssClassesOrEmpty) (tileContentNodes: Node list) =
            let tileContainerClasses = CssClasses [
                CssClass.tile
                match width with | TileSizeAuto -> () | _ -> width.CssClass
            ]

            div {
                tileContainerClasses |> moreClasses.ToHtmlClassAttribute

                forEach tileContentNodes <| id
            }
