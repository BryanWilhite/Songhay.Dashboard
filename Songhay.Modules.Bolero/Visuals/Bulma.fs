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

        ///<remarks>
        /// Prevents the text from being selectable
        ///</remarks>
        [<Literal>]
        let elementTextIsUnselectable = "is-unselectable"

        let fontSize (size: BulmaFontSize) = $"is-size-{size.Value}"

        let hidden (breakpoint: BulmaBreakpoint) = $"is-hidden-{breakpoint.Value}"
 
        let imageContainer (dimension: BulmaRatioDimension) = ["image"; dimension.CssClass]

        [<Literal>]
        let imageIsRounded = "is-rounded"

        [<Literal>]
        let levelContainer = "level"

        let level (alignment: CssAlignment) = $"level-{alignment.Value}"

        [<Literal>]
        let levelItem = "level-item"

        [<Literal>]
        let panel = "panel"

        let m (box: CssBox, suffix: BulmaValueSuffix) = $"m{box.Value}-{suffix.Value}"

        let p (box: CssBox, suffix: BulmaValueSuffix) = $"p{box.Value}-{suffix.Value}"

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
 
        let bulmaLoader (padding: int) (margin: int) =
            div {
                [ "has-text-centered"; "loader-container"; $"p-{padding}"] |> toHtmlClassFromList

                div {
                    [ "image"; "is-128x128"; "loader"; $"m-{margin}" ] |> toHtmlClassFromList

                    attr.title "Loading…"
                }
            }

        let bulmaPanelHeading displayText =
            p { "panel-heading" |> toHtmlClass; text displayText }

    module Button =
        let bulmaAnchorIconButton (title: DisplayText, href: Uri, id: Identifier) =
            a {
                (CssClasses [ "level-item"; "has-text-centered" ]).ToHtmlClassAttribute
                attr.href href.OriginalString
                attr.target "_blank"
                attr.title title.Value

                span {
                    attr.``class`` "icon"
                    "aria-hidden" => "true"

                    svgNode (svgViewBoxSquare 24) svgData[id]
                }
            }

    module Svg =
        let bulmaPanelIcon (id: Identifier) =
            span {
                [ "panel-icon"; "image"; "is-24x24" ] |> toHtmlClassFromList
                svgNode (svgViewBoxSquare 24) svgData[id]
            }

    module Tile =
        let bulmaColumnTile width nodes =
            let cssClasses = CssClasses [
                "tile"
                if width > 0 then $"is-{width}"
            ]

            div {
                cssClasses.ToHtmlClassAttribute
                forEach nodes <| id
            }

        let bulmaContentParentTile isVertical nodes =
            let cssClasses = CssClasses [
                "tile"
                "is-parent"
                if isVertical then "is-vertical"
            ]

            div {
                cssClasses.ToHtmlClassAttribute
                forEach nodes <| id
            }
