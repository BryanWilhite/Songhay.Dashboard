module Songhay.Dashboard.Client.Visuals.Block.Studio

open Bolero.Html

open Songhay.Modules.Bolero.BoleroUtility
open Songhay.Modules.Bolero.Visuals.Svg

open Songhay.Dashboard.Client
open Songhay.Dashboard.Models
open Songhay.Dashboard.Client.Visuals.Button

let studioLogo =
    let spanClasses = CssClasses [ "title"; "is-2"; "is-hidden-tablet-only" ]

    div {
        "logo" |> toHtmlClass
        attr.title App.AppTitle

        span { "has-text-weight-normal" |> spanClasses.Prepend |> toHtmlClassFromData; text "Songhay" }
        span { spanClasses.ToHtmlClassAttribute; text "System" }
        span { "title is-1" |> toHtmlClass; text "(::)" }
    }

let svgVersionNode (data: VersionData) =
    let classes = CssClasses [
        "level-item"
        "is-akyinkyin-base"
        "is-unselectable"
        "has-text-centered"
    ]

    div {
        classes.ToHtmlClassAttribute
        attr.title data.title.Value

        span {
            "icon" |> toHtmlClass
            "aria-hidden" => "true"

            svgNode (svgViewBoxSquare 24) svgData[data.id]
        }
        span { "is-size-7" |> toHtmlClass; text data.version }
    }

let studioNode =
    let cssClassesParentLevel = CssClasses [ "level"; "is-mobile" ]

    let cssClassesSvgLinkNodes = [ "ml-6"; "mr-6" ] |> cssClassesParentLevel.AppendList

    let cssClassesSvgVersionNodes =
        [ "has-text-greys-light-tone"; "mt-6"; "pt-6" ] |> cssClassesParentLevel.AppendList

    div {
        "card" |> App.appBlockChildCssClasses.Prepend |> toHtmlClassFromData
        div {
            "card-content" |> toHtmlClass
            div {
                [ "content"; "has-text-centered" ] |> toHtmlClassFromList
                studioLogo
            }
            div {
                cssClassesSvgLinkNodes.ToHtmlClassAttribute
                forEach App.appSocialLinks <| bulmaAnchorIconButton
            }
            div {
                cssClassesSvgVersionNodes.ToHtmlClassAttribute
                forEach App.appVersions <| svgVersionNode
            }
        }
    }
