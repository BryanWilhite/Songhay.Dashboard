module Songhay.Dashboard.Client.Visuals.Block.Studio

open Bolero.Html
open Songhay.Dashboard.Client
open Songhay.Dashboard.Client.Visuals.Button
open Songhay.Dashboard.Client.Visuals.Types

let studioLogo =
    let spanClasses = [ "title"; "is-2"; "is-hidden-tablet-only" ]

    div [ attr.classes [ "logo" ]; attr.title [ Info.title ] ] [
        span [ attr.classes ([ "has-text-weight-normal"] @ spanClasses) ] [ text "Songhay" ]
        span [ attr.classes spanClasses ] [ text "System" ]
        span [ attr.classes [ "title"; "is-1" ] ] [ text "(::)" ]
    ]

[<Literal>]
let viewBox = "0 0 24 24"

let data =
    [
        {
            title = "@BryanWilhite on Twitter"
            href = "https://twitter.com/BryanWilhite"
            id = "mdi_twitter_24px"
            viewBox = viewBox
        }
        {
            title = "Bryan Wilhite on LinkedIn"
            href = "http://www.linkedin.com/in/wilhite"
            id = "mdi_linkedin_24px"
            viewBox = viewBox
        }
        {
            title = "rasx on StackOverflow"
            href = "http://stackoverflow.com/users/22944/rasx"
            id = "mdi_stack_overflow_24px"
            viewBox = viewBox
        }
        {
            title = "BryanWilhite on GitGub"
            href = "https://github.com/BryanWilhite"
            id = "mdi_github_circle_24px"
            viewBox = viewBox
        }
    ]

let spriteNodes =
    data
    |> List.map bulmaAnchorIconButton
