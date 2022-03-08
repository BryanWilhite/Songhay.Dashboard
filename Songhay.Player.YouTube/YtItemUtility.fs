namespace Songhay.Player.YouTube

open System
open System.Text.Json

open FsToolkit.ErrorHandling

open Microsoft.FSharp.Core
open Songhay.Modules.JsonDocumentUtility
open Songhay.Modules.ProgramTypeUtility

open Songhay.Player.YouTube.Models

module YtItemUtility =

    [<Literal>]
    let YtItemsPropertyName = "items"

    let tryGetYouTubeContentDetails (element: JsonElement) : Result<YouTubeContentDetails, JsonException> =
        let videoIdResult = element |> tryGetProperty "videoId" |> Result.map (fun el -> el.GetString())
        let videoPublishedAtResult =
            element
            |> tryGetProperty "videoPublishedAt"
            |> Result.either
                (
                    fun el ->
                        let dateTimeString = el.GetString().Trim()

                        match tryParseRfc822DateTime dateTimeString with
                        | Result.Error _ -> resultError "pubDate"
                        | Ok rfc822DateTime -> Ok rfc822DateTime
                )
                Result.Error
        let durationResult = element |> tryGetProperty "duration" |> Result.map (fun el -> el.GetString())
        let dimensionResult = element |> tryGetProperty "dimension" |> Result.map (fun el -> el.GetString())
        let definitionResult = element |> tryGetProperty "definition" |> Result.map (fun el -> el.GetString())
        let captionResult =
            element
            |> tryGetProperty "caption"
            |> Result.either
                (
                    fun el ->
                        let caption = el.GetString()
                        match Boolean.TryParse caption with
                        | false, _ -> resultError "caption"
                        | true, b -> Ok b
                )
                Result.Error
        let licensedContentResult = element |> tryGetProperty "licensedContent" |> Result.map (fun el -> el.GetBoolean())
        let regionRestrictionResult =
            element
            |> tryGetProperty "regionRestriction"
            |> Result.either
                (
                    fun el ->
                        let blocked = el.EnumerateArray() |> Array.ofSeq |> Array.map (fun i -> i.ToString())
                        Ok {| blocked = blocked |}
                )
                Result.Error
        let projectionResult = element |> tryGetProperty "projection" |> Result.map (fun el -> el.GetString())

        [
            videoIdResult |> Result.map (fun _ -> true)
            videoPublishedAtResult |> Result.map (fun _ -> true)
            durationResult |> Result.map (fun _ -> true)
            dimensionResult |> Result.map (fun _ -> true)
            definitionResult |> Result.map (fun _ -> true)
            captionResult |> Result.map (fun _ -> true)
            licensedContentResult |> Result.map (fun _ -> true)
            regionRestrictionResult |> Result.map (fun _ -> true)
            projectionResult |> Result.map (fun _ -> true)
        ]
        |> List.sequenceResultM
        |> Result.either
            (
                fun _ ->
                    Ok {
                        videoId = videoIdResult |> Option.ofResult
                        videoPublishedAt = videoPublishedAtResult |> Option.ofResult
                        duration = durationResult |> Result.valueOr raise
                        dimension = dimensionResult |> Result.valueOr raise
                        definition = definitionResult |> Result.valueOr raise
                        caption = captionResult |> Result.valueOr raise
                        licensedContent = licensedContentResult |> Result.valueOr raise
                        projection = projectionResult |> Result.valueOr raise
                        regionRestriction = regionRestrictionResult |> Option.ofResult
                    }
            )
            Result.Error

    let tryGetYouTubeThumbnail (element: JsonElement) : Result<YouTubeThumbnail, JsonException> =
        let urlResult = element |> tryGetProperty "url" |> Result.map (fun el -> el.GetString())
        let widthResult = element |> tryGetProperty "width" |> Result.map (fun el -> el.GetInt32())
        let heightResult = element |> tryGetProperty "height" |> Result.map (fun el -> el.GetInt32())

        [
            urlResult |> Result.map (fun _ -> true)
            widthResult |> Result.map (fun _ -> true)
            heightResult |> Result.map (fun _ -> true)
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
        |> tryGetProperty "resourceId"
        |> Result.map (fun el -> { videoId = el.GetString() })

    let tryGetYouTubeSnippet (element: JsonElement)  : Result<YouTubeSnippet, JsonException> =

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
        let channelIdResult = element |> tryGetProperty "channelId" |> Result.map (fun el -> el.GetString())
        let titleResult = element |> tryGetProperty "title" |> Result.map (fun el -> el.GetString())
        let descriptionResult = element |> tryGetProperty "description" |> Result.map (fun el -> el.GetString())
        let thumbnailsResult = element |> tryGetYouTubeThumbnails
        let channelTitleResult = element |> tryGetProperty "channelTitle" |> Result.map (fun el -> el.GetString())
        let playlistIdResult = element |> tryGetProperty "playlistId" |> Result.map (fun el -> el.GetString())
        let positionResult = element |> tryGetProperty "position" |> Result.map (fun el -> el.GetInt32())
        let resourceIdResult = element |> tryGetYouTubeResourceId
        let tagsResult =
            element
            |> tryGetProperty "tags"
            |> Result.map (fun el -> el.EnumerateArray() |> Array.ofSeq |> Array.map (fun i -> i.ToString()))

        let localizedResult = element |> tryGetProperty "localized" |> Result.map id
        let localizedDescResult =
            localizedResult
            |> Result.bind (tryGetProperty "description")
            |> Result.map (fun el -> el.GetString())
        let localizedTitleResult =
            localizedResult
            |> Result.bind (tryGetProperty "title")
            |> Result.map (fun el -> el.GetString())

        [
            publishedAtResult |> Result.map (fun _ -> true)
            channelIdResult |> Result.map (fun _ -> true)
            titleResult |> Result.map (fun _ -> true)
            descriptionResult |> Result.map (fun _ -> true)
            thumbnailsResult |> Result.map (fun _ -> true)
            channelTitleResult |> Result.map (fun _ -> true)
            playlistIdResult |> Result.map (fun _ -> true)
            localizedDescResult |> Result.map (fun _ -> true)
            localizedTitleResult |> Result.map (fun _ -> true)
        ]
        |> List.sequenceResultM
        |> Result.either
            (
                fun _ ->
                    Ok {
                        channelId = channelIdResult |> Result.valueOr raise
                        channelTitle = titleResult |> Result.valueOr raise
                        description = descriptionResult |> Result.valueOr raise
                        localized =
                            {|
                                description = localizedDescResult |> Result.valueOr raise
                                title = localizedTitleResult |> Result.valueOr raise
                            |}
                        playlistId = playlistIdResult |> Result.valueOr raise
                        position = positionResult |> Option.ofResult
                        publishedAt = publishedAtResult |> Result.valueOr raise
                        resourceId = resourceIdResult |> Option.ofResult
                        tags = tagsResult |> Option.ofResult
                        thumbnails = thumbnailsResult |> Result.valueOr raise
                        title = titleResult |> Result.valueOr raise
                    }
            )
            Result.Error

    let tryGetYouTubeItem (element: JsonElement) : Result<YouTubeItem, JsonException> =
        let etagResult = element |> tryGetProperty "etag" |> Result.map (fun el -> el.GetString())
        let idResult = element |> tryGetProperty "id" |> Result.map (fun el -> el.GetString())
        let kindResult = element |> tryGetProperty "kind" |> Result.map (fun el -> el.GetString())
        let snippetResult = element |> tryGetProperty "snippet" |> Result.bind tryGetYouTubeSnippet
        let contentDetailsResult = element |> tryGetProperty "contentDetails" |> Result.bind tryGetYouTubeContentDetails

        [
            etagResult |> Result.map (fun _ -> true)
            idResult |> Result.map (fun _ -> true)
            kindResult |> Result.map (fun _ -> true)
            snippetResult |> Result.map (fun _ -> true)
            contentDetailsResult |> Result.map (fun _ -> true)
        ]
        |> List.sequenceResultM
        |> Result.either
            (
                fun _ ->
                    Ok {
                        etag = etagResult |> Result.valueOr raise
                        id = idResult |> Result.valueOr raise
                        kind = kindResult |> Result.valueOr raise
                        snippet = snippetResult |> Result.valueOr raise
                        contentDetails = contentDetailsResult |> Result.valueOr raise
                    }
            )
            Result.Error

    let tryGetYtItems (element: JsonElement) =
        element
        |> tryGetProperty YtItemsPropertyName
        |> Result.either
            (
                fun el ->
                    let items = el.EnumerateArray() |> Array.ofSeq |> Array.map (fun i -> i |> tryGetYouTubeItem)
                    Ok items
            )
            Result.Error
