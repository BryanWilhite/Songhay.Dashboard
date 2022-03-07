namespace Songhay.Player.YouTube.Models

open System

/// <summary>
/// defines YouTube thumb snippet
/// </summary>
type YouTubeSnippet = {

    channelId: string

    channelTitle: string

    description: string

    /// <summary>
    /// localized version of description and title
    /// </summary>
    localized: {| description: string; title: string |}

    playlistId: string

    position: int option

    publishedAt: DateTime

    resourceId: YouTubeResourceId

    tags: string[]

    thumbnails: YouTubeThumbnails

    title: string
}
