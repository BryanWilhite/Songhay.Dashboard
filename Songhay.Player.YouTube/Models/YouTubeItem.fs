namespace Songhay.Player.YouTube.Models

open System
open Songhay.Modules.ProgramTypeUtility

/// <summary>
/// defines YouTube API item
/// of the specified kind
/// </summary>
/// <remarks>
/// the `item` is the central JSON shape of the YouTube API
/// </remarks>
type YouTubeItem =
    {

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

    member this.getPublishedAt = this.snippet.publishedAt

    member this.tryGetDuration =
        this.contentDetails.duration |> tryParseIso8601Duration

    member this.tryGetUri : Result<Uri, exn> =
        let videoIdResult =
            match this.kind with
            | "youtube#video" -> Ok this.id
            | _ ->
                match this.snippet.resourceId with
                | Some id -> Ok id.videoId
                | _ -> Error (exn "The expected Snippet Resource ID is not here.")

        videoIdResult |> Result.map (fun videoId -> $"{YouTubeScalars.YouTubeWatchRootUri}{videoId}" |> Uri)
