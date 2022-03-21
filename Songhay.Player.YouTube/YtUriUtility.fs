namespace Songhay.Player.YouTube

open System
open Songhay.Modules.Models

open Songhay.Player.YouTube.Models.YouTubeScalars

module YtUriUtility =
    let getPlaylistUri (id: Identifier) =
        Uri($"{YouTubeApiRootUri}{YouTubeApiPlaylistPath}{id.StringValue}", UriKind.Absolute)

    let getPresentationUri (id: Identifier) =
        Uri($"{YouTubeApiRootUri}{YouTubeApiVideosPath}{id.StringValue}", UriKind.Absolute)
