module Songhay.Dashboard.Client.Visuals.Tile

open Bolero.Html
open Songhay.Dashboard.Client

let bulmaColumnTile (width: int) nodes =
    let classes =
        match width with
        | 0 -> [ "tile" ]
        | _ -> [ "tile"; $"is-{width}" ]

    div [ attr.classes classes ] nodes

let bulmaContentParentTile nodes =
    div [ attr.classes [ "tile"; "is-parent" ] ] nodes

let studioLogo =
    let spanClasses = [ "title"; "is-2"; "is-hidden-tablet-only" ]

    div [ attr.classes [ "logo" ]; attr.title [ Info.title ] ] [
        span [ attr.classes ([ "has-text-weight-normal"] @ spanClasses) ] [ text "Songhay" ]
        span [ attr.classes spanClasses ] [ text "System" ]
        span [ attr.classes [ "title"; "is-1" ] ] [ text "(::)" ]
    ]

let studioComponentNode =
    bulmaColumnTile 0 [
        bulmaContentParentTile [
            div [ attr.classes [ "card"; "has-background-grey-dark"; "is-child"; "tile" ] ] [
                div [ attr.classes [ "card-content" ] ] [
                    div [ attr.classes ["content"; "has-text-centered" ] ] [ studioLogo ]
                ]
            ]
        ]
    ]
