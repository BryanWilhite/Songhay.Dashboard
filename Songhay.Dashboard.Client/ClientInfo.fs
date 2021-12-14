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
