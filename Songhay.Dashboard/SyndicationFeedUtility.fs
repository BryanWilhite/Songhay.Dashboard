namespace Songhay.Dashboard

open System.Text.Json
open Microsoft.FSharp.Core

open FsToolkit.ErrorHandling

open Songhay.Modules.Models
open Songhay.Modules.JsonDocumentUtility
open Songhay.Modules.Publications.SyndicationFeedUtility
open Songhay.Dashboard.Models

module SyndicationFeedUtility =

    let getFeedName (name: string) =
        match name with
        | nameof CodePen -> CodePen
        | nameof Flickr -> Flickr
        | nameof GitHub -> GitHub
        | nameof Studio -> Studio
        | nameof StackOverflow -> StackOverflow
        | _ -> Unknown

    let toFeedImage (feedName: FeedName) (headElementResult: Result<JsonElement,JsonException>) =
            match feedName with
            | CodePen ->
                headElementResult
                |> Result.bind (tryGetProperty "link")
                |> Result.either
                    (fun linkProperty -> Some $"{linkProperty.GetString()}/image/large.png")
                    (fun _ -> None)
            | Flickr ->
                headElementResult
                |> Result.bind (tryGetProperty "enclosure")
                |> Result.bind (tryGetProperty "@url")
                |> Result.either
                    (fun urlProperty -> Some (urlProperty.GetString()))
                    (fun _ -> None)
            | _ -> None

    let tryGetSyndicationFeed (feedName: FeedName) (isRssFeed: bool, element: JsonElement) =
        let feedElementsResult =
            match isRssFeed with
            | true -> element |> tryGetRssChannelItems
            | false -> element |> tryGetAtomEntries

        let headElementResult = feedElementsResult |> Result.map (fun elements -> elements |> List.head)

        let feedImage = headElementResult |> toFeedImage feedName

        let modificationDateResult = element |> tryGetFeedModificationDate isRssFeed

        let feedItemsResult =
            match isRssFeed with
            | true ->
                feedElementsResult
                |> Result.either
                    (fun elements -> elements |> List.map tryGetRssSyndicationFeedItem |> List.sequenceResultM)
                    Error
            | false ->
                feedElementsResult
                |> Result.either
                    (fun elements -> elements |> List.map tryGetAtomSyndicationFeedItem |> List.sequenceResultM)
                    Error

        let feedTitleResult =
            match isRssFeed with
            | true -> tryGetRssChannelTitle element
            | _ -> tryGetAtomChannelTitle element

        [
            feedItemsResult |> Result.map (fun _ -> true)
            feedTitleResult |> Result.map (fun _ -> true)
            modificationDateResult |> Result.map (fun _ -> true)
        ]
        |> List.sequenceResultM
        |> Result.map ( fun _ ->
                {
                   feedImage = feedImage
                   feedItems = feedItemsResult |> Result.valueOr raise
                   feedTitle = feedTitleResult |> Result.valueOr raise
                   modificationDate = modificationDateResult |> Result.valueOr raise
                }
            )

    let fromInput feedsElement =
        [
            CodePen, nameof CodePen
            Flickr, nameof Flickr
            GitHub, nameof GitHub
            StackOverflow, nameof StackOverflow
            Studio, nameof Studio
        ]
        |> List.map
            (
                fun (feedName, elementName) ->
                    feedsElement
                    |> tryGetProperty (elementName.ToLowerInvariant())
                    |> Result.bind tryGetFeedElement 
                    |> Result.bind (tryGetSyndicationFeed feedName)
                    |> Result.map (fun feed -> feedName, feed)
            )
        |> List.sequenceResultM
