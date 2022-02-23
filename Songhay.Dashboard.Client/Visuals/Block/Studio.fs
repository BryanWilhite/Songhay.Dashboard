module Songhay.Dashboard.Client.Visuals.Block.Studio

open Bolero.Html

open Songhay.Modules.Bolero.Visuals.Svg

open Songhay.Dashboard.Client
open Songhay.Dashboard.Models
open Songhay.Dashboard.Client.Visuals.Button

let studioLogo =
    let spanClasses = [ "title"; "is-2"; "is-hidden-tablet-only" ]

    div [ attr.classes [ "logo" ]; attr.title [ App.AppTitle ] ] [
        span [ attr.classes ([ "has-text-weight-normal"] @ spanClasses) ] [ text "Songhay" ]
        span [ attr.classes spanClasses ] [ text "System" ]
        span [ attr.classes [ "title"; "is-1" ] ] [ text "(::)" ]
    ]

let svgLinkNodes =
    App.appSocialLinks
    |> List.map bulmaAnchorIconButton

let svgVersionNode (data: VersionData) =
    let classes = [
        "level-item"
        "is-akyinkyin-base"
        "is-unselectable"
        "has-text-centered"
    ]

    div
        [ attr.classes classes; attr.title data.title ]
        [
            span
                [
                    attr.classes [ "icon" ]
                    "aria-hidden" => "true"
                ]
                [ svgNode (svgViewBoxSquare 24) svgData[data.id] ]
            span [ attr.classes [ "is-size-7" ] ] [ text data.version ]
        ]

let svgVersionNodes = App.appVersions |> List.map svgVersionNode

let studioNode =
    let cssClassesParentLevel = [ "level"; "is-mobile" ]

    let cssClassesSvgLinkNodes =
        cssClassesParentLevel @ [ "ml-6"; "mr-6" ]

    let cssClassesSvgVersionNodes =
        cssClassesParentLevel @ [ "has-text-greys-light-tone"; "mt-6"; "pt-6" ]

    div
        [ attr.classes ([ "card" ] @ App.appBlockChildCssClasses) ]
        [
            div
                [ attr.classes [ "card-content" ] ]
                [
                    div
                        [ attr.classes [ "content"; "has-text-centered" ] ]
                        [ studioLogo ]
                    div [ attr.classes cssClassesSvgLinkNodes ] svgLinkNodes
                    div [ attr.classes cssClassesSvgVersionNodes ] svgVersionNodes
                ]
        ]
