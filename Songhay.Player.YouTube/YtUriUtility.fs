namespace Songhay.Player.YouTube

open Songhay.Modules.Models

open Songhay.Player.YouTube.Models.YouTubeScalars

module YtUriUtility =
    let getPlaylistUri (id: Identifier) = $"{YouTubeApiRootUri}{YouTubeApiPlaylistPath}{id.StringValue}"

    let getPresentationUri (id: Identifier) = $"{YouTubeApiRootUri}{YouTubeApiVideosPath}{id.StringValue}"
