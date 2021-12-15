module Songhay.Dashboard.Client.Info

[<Literal>]
let appTitle = "SonghaySystem(::)"

[<Literal>]
let appDataLocation = "https://songhaystorage.blob.core.windows.net/studio-dash/app.json"

type FeedName =
    | CodePen
    | Flickr
    | GitHub
    | StackOverflow
    | Studio
    | Unknown

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
