module Songhay.Dashboard.Client.Visuals.Block.Studio

open System
open Bolero.Html
open Songhay.Dashboard.Client
open Songhay.Dashboard.Client.Visuals.Button
open Songhay.Dashboard.Client.Visuals.Svg
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

let svgLinkNodes =
    data
    |> List.map bulmaAnchorIconButton

let svgVersionNode (title, id, ver) =
    let classes = [
        "level-item"
        "is-akyinkyin-base"
        "is-unselectable"
        "has-text-centered"
    ]

    div
        [ attr.classes classes; attr.title title ]
        [
            span [ attr.classes [ "icon"; ] ] [ svgSpriteNode $"./#{id}" viewBox ]
            span [ attr.classes [ "is-size-7"; ] ] [ text ver ]
        ]

let boleroVersion = Bolero.Node.Empty.GetType().Assembly.GetName().Version.ToString()
let dotnetRuntimeVersion = $"{Environment.Version.Major:D}.{Environment.Version.Minor:D2}"

let svgVersionNodes =
    [
        ( $"Bolero {boleroVersion}", "mdi_bolero_dance_24px", boleroVersion )
        ( $".NET Runtime {dotnetRuntimeVersion}", "mdi_dotnet_24px", dotnetRuntimeVersion )
    ]
    |> List.map svgVersionNode
