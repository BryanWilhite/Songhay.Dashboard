namespace Songhay.Player.YouTube.Models

module YouTubeScalars =

    /// <summary>
    /// base URI representing the b-roll endpoint
    /// for YouTube curation
    /// </summary>
    [<Literal>]
    let YouTubeApiRootUri = "https://songhay-system-player.azurewebsites.net/api/Player/v1/video/youtube/"

    /// <summary>
    /// URI path representing a YouTube channel
    /// and/or YouTube `uploads`
    /// curated as a `playlist`
    /// </summary>
    [<Literal>]
    let YouTubeApiPlaylistPath = "playlist/uploads/"

    /// <summary>
    /// URI path representing sets of YouTube `uploads`
    /// curated as `playlists`
    /// </summary>
    [<Literal>]
    let YouTubeApiPlaylistsPath = "playlists/"

    /// <summary>
    /// URI path representing indices of curated as `playlists`
    /// in @type {GenericWebIndex} format
    /// </summary>
    [<Literal>]
    let YouTubeApiPlaylistsIndexPath = "playlist/index/"

    /// <summary>
    /// URI path representing curated `videos`
    /// or YouTube `items` for a gen-web
    /// </summary>
    [<Literal>]
    let YouTubeApiVideosPath = "videos/"

    /// <summary>
    /// base URI representing YouTube channel browsing
    /// </summary>
    [<Literal>]
    let YouTubeChannelRootUri = "https://www.youtube.com/channel/"

    /// <summary>
    /// base URI representing YouTube item watching
    /// </summary>
    [<Literal>]
    let YouTubeWatchRootUri = "https://www.youtube.com/watch?v="
