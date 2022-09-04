namespace Songhay.Modules.Bolero.Visuals

open System
open Microsoft.AspNetCore.Components.Web

open Bolero
open Bolero.Html

open Songhay.Modules.Models
open Songhay.Modules.Bolero.BoleroUtility
open Songhay.Modules.Bolero.Visuals.Svg

module Bulma =
    module Block =
        let bulmaDropdownItem
            (isActive: bool)
            (callback: MouseEventArgs -> unit)
            (displayText: string) =
            a {
                attr.href "#"
                [
                    "dropdown-item"
                    if isActive then "is-active"
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
                if isActive then "is-active"
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

        let bulmaLevelCssClasses (margin: int) = [ "level"; $"m-{margin}" ] |> toHtmlClassFromList

        let bulmaLevelItem (cssClasses: CssClasses option) (itemContent: Node) =
            let cssClassName = "level-item"
            div {
                 if cssClasses.IsNone then cssClassName |> toHtmlClass
                 else (cssClassName |> cssClasses.Value.Prepend).ToHtmlClassAttribute

                 itemContent
            }

        let bulmaLevelLeftCssClass = "level-left" |> toHtmlClass

        let bulmaLevelRightCssClass = "level-right" |> toHtmlClass
 
        let bulmaLoader (padding: int) (margin: int) =
            div {
                [ "has-text-centered"; "loader-container"; $"p-{padding}"] |> toHtmlClassFromList

                div {
                    [ "image"; "is-128x128"; "loader"; $"m-{margin}" ] |> toHtmlClassFromList

                    attr.title "Loading…"
                }
            }

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
