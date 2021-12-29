module Songhay.Dashboard.Client.App

open System

type FeedName =
    | CodePen
    | Flickr
    | GitHub
    | StackOverflow
    | Studio
    | Unknown

type SvgSpriteData = { title: string; href: string; id: string; viewBox: string }

[<Literal>]
let appTitle = "SonghaySystem(::)"

[<Literal>]
let appDataLocation = "https://songhaystorage.blob.core.windows.net/studio-dash/app.json"

[<Literal>]
let appSvgViewBox = "0 0 24 24"

let appSocial =
    [
        {
            title = "@BryanWilhite on Twitter"
            href = "https://twitter.com/BryanWilhite"
            id = "mdi_twitter_24px"
            viewBox = appSvgViewBox
        }
        {
            title = "Bryan Wilhite on LinkedIn"
            href = "http://www.linkedin.com/in/wilhite"
            id = "mdi_linkedin_24px"
            viewBox = appSvgViewBox
        }
        {
            title = "rasx on StackOverflow"
            href = "http://stackoverflow.com/users/22944/rasx"
            id = "mdi_stack_overflow_24px"
            viewBox = appSvgViewBox
        }
        {
            title = "BryanWilhite on GitGub"
            href = "https://github.com/BryanWilhite"
            id = "mdi_github_circle_24px"
            viewBox = appSvgViewBox
        }
    ]

let appVersions =
    let boleroVersion = Bolero.Node.Empty.GetType().Assembly.GetName().Version.ToString()
    let dotnetRuntimeVersion = $"{Environment.Version.Major:D}.{Environment.Version.Minor:D2}"

    [
        ( $"Bolero {boleroVersion}", "mdi_bolero_dance_24px", boleroVersion )
        ( $".NET Runtime {dotnetRuntimeVersion}", "mdi_dotnet_24px", dotnetRuntimeVersion )
    ]

let toFeedName sInput =
    let mapping (s: string) =
        match s with
        | nameof CodePen        -> CodePen
        | nameof Flickr         -> Flickr
        | nameof GitHub         -> GitHub
        | nameof StackOverflow  -> StackOverflow
        | nameof Studio         -> Studio
        | _                     -> Unknown

    sInput
    |> Option.ofObj
    |> Option.map mapping
    |> Option.defaultWith (fun () -> Unknown)
