namespace Songhay.Player.YouTube

open System.Text.Json

open FsToolkit.ErrorHandling

open Songhay.Modules.JsonDocumentUtility
open Songhay.Modules.ProgramTypeUtility
open Songhay.Player.YouTube.Models

module YtItemUtility =

    [<Literal>]
    let YtItemsPropertyName = "items"

    let tryGetYouTubeThumbnail (element: JsonElement) : Result<YouTubeThumbnail, JsonException> =
        let urlResult = element |> tryGetProperty "url" |> Result.map (fun el -> el.GetString())
        let widthResult = element |> tryGetProperty "width" |> Result.map (fun el -> el.GetInt32())
        let heightResult = element |> tryGetProperty "height" |> Result.map (fun el -> el.GetInt32())

        [
            urlResult
            widthResult |> Result.map (fun i -> i.ToString())
            heightResult |> Result.map (fun i -> i.ToString())
        ]
        |> List.sequenceResultM
        |> Result.either
            (
                fun _ ->
                    Ok {
                        url = urlResult |> Result.valueOr raise
                        width = widthResult |> Result.valueOr raise
                        height = heightResult |> Result.valueOr raise
                    }
            )
            Result.Error

    let tryGetYouTubeThumbnails (element: JsonElement) =
        let thumbnailsResult = element |> tryGetProperty "thumbnails" |> Result.map id

        let defaultResult = thumbnailsResult |> Result.bind (tryGetProperty "default") |> Result.map id
        let mediumResult = thumbnailsResult |> Result.bind (tryGetProperty "medium") |> Result.map id
        let highResult = thumbnailsResult |> Result.bind (tryGetProperty "high") |> Result.map id

        [
            defaultResult
            mediumResult
            highResult
        ]
        |> List.sequenceResultM
        |> Result.either
            (
                fun _ ->
                    Ok {
                        ``default`` = defaultResult |> Result.bind tryGetYouTubeThumbnail |> Result.valueOr raise
                        medium = mediumResult |> Result.bind tryGetYouTubeThumbnail |> Result.valueOr raise
                        high = highResult |> Result.bind tryGetYouTubeThumbnail |> Result.valueOr raise
                        standard = None
                        maxres = None
                    }
            )
            Result.Error

    let tryGetYouTubeResourceId (element: JsonElement) : Result<YouTubeResourceId, JsonException> =
        element
        |> tryGetProperty "id"
        |> Result.map (fun el -> { videoId = el.GetString() })

    let tryGetYouTubeSnippet (element: JsonElement)  : Result<YouTubeSnippet, JsonException> =

        let channelIdResult = element |> tryGetProperty "channelId" |> Result.map (fun el -> el.GetString())
        let channelTitleResult = element |> tryGetProperty "channelTitle" |> Result.map (fun el -> el.GetString())
        let descriptionResult = element |> tryGetProperty "description" |> Result.map (fun el -> el.GetString())
        let localizedResult = element |> tryGetProperty "localized" |> Result.map id
        let localizedDescResult =
            localizedResult
            |> Result.bind (tryGetProperty "description")
            |> Result.map (fun el -> el.GetString())
        let localizedTitleResult =
            localizedResult
            |> Result.bind (tryGetProperty "title")
            |> Result.map (fun el -> el.GetString())
        let playlistIdResult = element |> tryGetProperty "playlistId" |> Result.map (fun el -> el.GetString())
        let publishedAtResult =
            element
            |> tryGetProperty "publishedAt"
            |> Result.either
                (
                    fun pubDateElement ->
                        let dateTimeString = pubDateElement.GetString().Trim()

                        match tryParseRfc822DateTime dateTimeString with
                        | Result.Error _ -> resultError "pubDate"
                        | Ok rfc822DateTime -> Ok rfc822DateTime
                )
                Result.Error
        let resourceIdResult = element |> tryGetYouTubeResourceId
        let tagsResult =
            element
            |> tryGetProperty "tags"
            |> Result.map (fun el -> el.EnumerateArray() |> Array.ofSeq |> Array.map (fun i -> i.ToString()))
        let thumbnailsResult = element |> tryGetYouTubeThumbnails
        let titleResult = element |> tryGetProperty "title" |> Result.map (fun el -> el.GetString())

        [
            channelIdResult
            channelTitleResult
            descriptionResult
            localizedDescResult
            localizedTitleResult
            playlistIdResult
            publishedAtResult |> Result.map (fun i -> i.ToString())
            resourceIdResult |> Result.map (fun i -> i.ToString())
            tagsResult |> Result.map (fun i -> i.ToString())
            thumbnailsResult |> Result.map (fun i -> i.ToString())
            titleResult
        ]
        |> List.sequenceResultM
        |> Result.either
            (
                fun _ ->
                    Ok {
                        channelId = channelIdResult |> Result.valueOr raise
                        channelTitle = channelTitleResult |> Result.valueOr raise
                        description = descriptionResult |> Result.valueOr raise
                        localized =
                            {|
                                description = localizedDescResult |> Result.valueOr raise
                                title = localizedTitleResult |> Result.valueOr raise
                            |}
                        playlistId = playlistIdResult |> Result.valueOr raise
                        position = None
                        publishedAt = publishedAtResult |> Result.valueOr raise
                        resourceId = resourceIdResult |> Result.valueOr raise
                        tags = tagsResult |> Result.valueOr raise
                        thumbnails = thumbnailsResult |> Result.valueOr raise
                        title = titleResult |>Result.valueOr raise
                    }
            )
            Result.Error

    let tryGetYtItems (element: JsonElement) =
        element
        |> tryGetProperty YtItemsPropertyName
        |> Result.map (fun el -> el.EnumerateArray() |> List.ofSeq)
