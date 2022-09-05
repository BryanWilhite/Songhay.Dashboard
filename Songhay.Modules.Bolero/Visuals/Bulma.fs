namespace Songhay.Modules.Bolero.Visuals

open System
open Microsoft.AspNetCore.Components.Web

open Bolero
open Bolero.Html

open Songhay.Modules.Models
open Songhay.Modules.Bolero.Models
open Songhay.Modules.Bolero.BoleroUtility
open Songhay.Modules.Bolero.Visuals.Svg

module Bulma =
    module CssClass =
        [<Literal>]
        let elementIsActive = "is-active"

        ///<remarks>
        /// Applies <c>cursor: pointer !important</c> to the element.
        ///</remarks>
        [<Literal>]
        let elementIsClickable = "is-clickable"

        [<Literal>]
        let elementIsBlock = "is-block"

        ///<remarks>
        /// Adds overflow hidden
        ///</remarks>
        [<Literal>]
        let elementIsClipped = "is-clipped"

        [<Literal>]
        let elementIsHidden = "is-hidden"

        ///<remarks>
        /// Hide elements visually but keep the element available to be announced by a screen reader
        ///</remarks>
        [<Literal>]
        let elementIsHiddenVisually = "is-sr-only"

        [<Literal>]
        let elementIsInlineBlock = "is-inline-block"

        ///<remarks>
        /// Adds visibility hidden
        ///</remarks>
        [<Literal>]
        let elementIsInvisible = "is-invisible"

        ///<remarks>
        /// Completely covers the first positioned parent
        ///</remarks>
        [<Literal>]
        let elementIsOverlay = "is-overlay"

        [<Literal>]
        let elementIsRelative = "is-relative"

        let elementTextAlign (alignment: CssAlignment) =
            let suffix =
                match alignment with
                | Center -> "centered"
                | Justify -> "justified"
                | _ -> alignment.Value

            $"has-text-{suffix}"

        ///<remarks>
        /// Transforms the first character of each word to Uppercase
        ///</remarks>
        [<Literal>]
        let elementTextIsCapitalized = "is-capitalized"

        ///<remarks>
        /// Sets font family to <c>$family-sans-serif</c>
        ///</remarks>
        [<Literal>]
        let elementTextIsFamilySansSerif = "is-family-sans-serif"

        ///<remarks>
        /// Sets font family to <c>$family-monospace</c>
        ///</remarks>
        [<Literal>]
        let elementTextIsFamilyMonospace = "is-family-monospace"

        ///<remarks>
        /// Sets font family to <c>$family-primary</c>
        ///</remarks>
        [<Literal>]
        let elementTextIsFamilyPrimary = "is-family-primary"

        ///<remarks>
        /// Sets font family to <c>$family-secondary</c>
        ///</remarks>
        [<Literal>]
        let elementTextIsFamilySecondary = "is-family-secondary"

        ///<remarks>
        /// Sets font family to <c>$family-code</c>
        ///</remarks>
        [<Literal>]
        let elementTextIsFamilyCode = "is-family-code"

        ///<remarks>
        /// Transforms all characters to lowercase
        ///</remarks>
        [<Literal>]
        let elementTextIsLowercase = "is-lowercase"

        ///<remarks>
        /// Transforms all characters to UPPERCASE
        ///</remarks>
        [<Literal>]
        let elementTextIsUppercase = "is-uppercase"

        ///<remarks>
        /// Transforms all characters to italic
        ///</remarks>
        [<Literal>]
        let elementTextIsItalic = "is-italic"

        ///<remarks>
        /// Transforms all characters to underlined
        ///</remarks>
        [<Literal>]
        let elementTextIsUnderlined = "is-underlined"

        ///<remarks>
        /// Prevents the text from being selectable
        ///</remarks>
        [<Literal>]
        let elementTextIsUnselectable = "is-unselectable"

        ///<remarks>
        /// Transforms text weight to light
        ///</remarks>
        [<Literal>]
        let elementTextIsWeightLight = "has-text-weight-light"

        ///<remarks>
        /// Transforms text weight to normal
        ///</remarks>
        [<Literal>]
        let elementTextIsWeightNormal = "has-text-weight-normal"

        ///<remarks>
        /// Transforms text weight to medium
        ///</remarks>
        [<Literal>]
        let elementTextIsWeightMedium = "has-text-weight-medium"

        ///<remarks>
        /// Transforms text weight to semibold
        ///</remarks>
        [<Literal>]
        let elementTextIsWeightSemibold = "has-text-weight-semibold"

        ///<remarks>
        /// Transforms text weight to bold
        ///</remarks>
        [<Literal>]
        let elementTextIsWeightBold = "has-text-weight-bold"

        let fontSize (size: BulmaFontSize) = $"is-size-{size.Value}"

        let hidden (breakpoint: BulmaBreakpoint) = $"is-hidden-{breakpoint.Value}"
 
        let imageContainer (dimension: BulmaRatioDimension) = ["image"; dimension.CssClass]

        [<Literal>]
        let imageIsRounded = "is-rounded"

        ///<remarks>
        /// “By default, columns are only activated on tablet and desktop.
        /// If you want to use columns on mobile too,
        /// add the is-mobile modifier on the columns container.”
        /// — https://github.com/jgthms/bulma/blob/master/docs/_posts/2016-02-09-blog-launched-new-responsive-columns-new-helpers.md
        ///</remarks>
        [<Literal>]
        let isMobileModifier = "is-mobile"

        [<Literal>]
        let levelContainer = "level"

        let level (alignment: CssAlignment) = $"level-{alignment.Value}"

        [<Literal>]
        let levelItem = "level-item"

        let m (box: CssBoxModel, suffix: BulmaValueSuffix) = $"m{box.Value}-{suffix.Value}"

        let p (box: CssBoxModel, suffix: BulmaValueSuffix) = $"p{box.Value}-{suffix.Value}"

        [<Literal>]
        let panel = "panel"

        ///<remarks>
        /// “Start with an ancestor tile that will wrap all other tiles…”
        /// — https://bulma.io/documentation/layout/tiles/
        ///</remarks>
        [<Literal>]
        let tileIsAncestor = "is-ancestor"

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

        ///<remarks>
        /// “If you want to stack tiles vertically, add is-vertical on the parent tile…”
        /// — https://bulma.io/documentation/layout/tiles/
        ///</remarks>
        [<Literal>]
        let tileIsVertical = "is-vertical"

    module Block =
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

        let bulmaPanelHeading displayText =
            p { "panel-heading" |> toHtmlClass; text displayText }

        let bulmaTile (width: BulmaTileHorizontalSize) (tileClasses: string list option) nodes =
            let cssClasses = CssClasses [
                "tile"
                match width with | TileSizeAuto -> () | _ -> width.CssClass
            ]

            div {
                if tileClasses.IsSome then
                    tileClasses.Value |> cssClasses.AppendList |> toHtmlClassFromData
                else
                    cssClasses.ToHtmlClassAttribute
                forEach nodes <| id
            }

    module Button =
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

    module Svg =
        let bulmaPanelIcon (id: Identifier) =
            span {
                [ "panel-icon" ] @ CssClass.imageContainer (Square Square24) |> toHtmlClassFromList
                svgNode (svgViewBoxSquare 24) svgData[id]
            }
