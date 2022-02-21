namespace Songhay.Player.YouTube.Models

/// <summary>
/// defines YouTube API item
/// of the specified kind
/// </summary>
/// <remarks>
/// the `item` is the central JSON shape of the YouTube API
/// </remarks>
type YouTubeItem = {

    /// <summary>
    /// entity tag to detect content changes
    /// </summary>
    etag: string

    /// <summary>
    /// unique ID for item
    /// </summary>
    id: string

    /// <summary>
    /// the kind of the item
    /// </summary>
    /// <remarks>
    /// the kinds used so far:
    /// - `youtube#video`
    /// - `youtube#videoListResponse`
    /// </remarks>
    kind: string

    /// <summary>
    /// YouTube API snippet
    /// </summary>
    snippet: YouTubeSnippet

    /// <summary>
    /// YouTube item content details
    /// </summary>
    contentDetails: YouTubeContentDetails

}
