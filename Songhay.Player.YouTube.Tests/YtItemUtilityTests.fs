namespace Songhay.Player.YouTube.Tests

module YtItemUtilityTests =

    open System.IO
    open System.Reflection
    open System.Text.Json
    open Xunit
    open FsUnit.Xunit
    open FsUnit.CustomMatchers
    open FsToolkit.ErrorHandling

    open Songhay.Modules.Models
    open Songhay.Modules.JsonDocumentUtility
    open Songhay.Modules.ProgramFileUtility

    open Songhay.Player.YouTube.Models
    open Songhay.Player.YouTube.YtItemUtility

    let projectDirectoryInfo =
        Assembly.GetExecutingAssembly()
        |> ProgramAssemblyInfo.getPathFromAssembly "../../../"
        |> Result.valueOr raiseProgramFileError
        |> DirectoryInfo

    let getJsonDocument (fileName: string) =
        let path =
            $"./json/{fileName}"
            |> tryGetCombinedPath projectDirectoryInfo.FullName
            |> Result.valueOr raiseProgramFileError
        JsonDocument.Parse(File.ReadAllText(path))

    [<Theory>]
    [<InlineData("60-minutes.json")>]
    [<InlineData("youtube-index-songhay-top-ten.json")>]
    let ``should have `items` property`` (fileName: string) =
        let videoJsonDocument = fileName |> getJsonDocument
        let actual =
            match videoJsonDocument.RootElement.TryGetProperty YtItemsPropertyName with
            | true, _ -> true
            | _ -> false

        actual |> should be True

    [<Theory>]
    [<InlineData("60-minutes.json")>]
    [<InlineData("youtube-index-songhay-top-ten.json")>]
    let ``tryGetYtContentDetails test`` (fileName: string) =
        let videoJsonDocument = fileName |> getJsonDocument
        let itemElementResult =
            videoJsonDocument.RootElement
            |> tryGetProperty YtItemsPropertyName
            |> Result.either ( fun el -> Ok (el.EnumerateArray() |> List.ofSeq |> List.head) ) Result.Error

        itemElementResult |> should be (ofCase <@ Result<JsonElement, JsonException>.Ok @>)

        let contentDetailsElementResult =
            itemElementResult |> Result.bind (tryGetProperty YtItemContentDetailsPropertyName)

        contentDetailsElementResult |> should be (ofCase <@ Result<JsonElement, JsonException>.Ok @>)

        let actualResult =
            contentDetailsElementResult
            |> Result.valueOr raise
            |> tryGetYtContentDetails

        actualResult |> should be (ofCase <@ Result<YouTubeContentDetails, JsonException>.Ok @>)

    [<Theory>]
    [<InlineData("60-minutes.json")>]
    [<InlineData("youtube-index-songhay-top-ten.json")>]
    let ``tryGetYtResourceId test`` (fileName: string) =
        let videoJsonDocument = fileName |> getJsonDocument
        let itemElementResult =
            videoJsonDocument.RootElement
            |> tryGetProperty YtItemsPropertyName
            |> Result.either ( fun el -> Ok (el.EnumerateArray() |> List.ofSeq |> List.head) ) Result.Error

        itemElementResult |> should be (ofCase <@ Result<JsonElement, JsonException>.Ok @>)

        let snippetElementResult =
            itemElementResult |> Result.bind (tryGetProperty YtItemSnippetPropertyName)
        snippetElementResult |> should be (ofCase <@ Result<JsonElement, JsonException>.Ok @>)

        let actualResult =
            snippetElementResult
            |> Result.valueOr raise
            |> tryGetYtResourceId

        actualResult |> should be (ofCase <@ Result<YouTubeResourceId, JsonException>.Ok @>)

    [<Theory>]
    [<InlineData("60-minutes.json")>]
    [<InlineData("youtube-index-songhay-top-ten.json")>]
    let ``tryGetYtThumbnails test`` (fileName: string) =
        let videoJsonDocument = fileName |> getJsonDocument
        let itemElementResult =
            videoJsonDocument.RootElement
            |> tryGetProperty YtItemsPropertyName
            |> Result.either ( fun el -> Ok (el.EnumerateArray() |> List.ofSeq |> List.head) ) Result.Error

        itemElementResult |> should be (ofCase <@ Result<JsonElement, JsonException>.Ok @>)

        let snippetElementResult =
            itemElementResult |> Result.bind (tryGetProperty YtItemSnippetPropertyName)
        snippetElementResult |> should be (ofCase <@ Result<JsonElement, JsonException>.Ok @>)

        let actualResult =
            snippetElementResult
            |> Result.valueOr raise
            |> tryGetYtThumbnails

        actualResult |> should be (ofCase <@ Result<YouTubeThumbnails, JsonException>.Ok @>)

    [<Theory>]
    [<InlineData("60-minutes.json")>]
    [<InlineData("youtube-index-songhay-top-ten.json")>]
    let ``tryGetYtSnippet test`` (fileName: string) =
        let videoJsonDocument = fileName |> getJsonDocument
        let itemElementResult =
            videoJsonDocument.RootElement
            |> tryGetProperty YtItemsPropertyName
            |> Result.either ( fun el -> Ok (el.EnumerateArray() |> List.ofSeq |> List.head) ) Result.Error

        itemElementResult |> should be (ofCase <@ Result<JsonElement, JsonException>.Ok @>)

        let snippetElementResult =
            itemElementResult |> Result.bind (tryGetProperty YtItemSnippetPropertyName)
        snippetElementResult |> should be (ofCase <@ Result<JsonElement, JsonException>.Ok @>)

        let actualResult =
            snippetElementResult
            |> Result.valueOr raise
            |> tryGetYtSnippet

        actualResult |> should be (ofCase <@ Result<YouTubeSnippet, JsonException>.Ok @>)

    [<Theory>]
    [<InlineData("60-minutes.json")>]
    [<InlineData("youtube-index-songhay-top-ten.json")>]
    let ``tryGetYtItem test`` (fileName: string) =
        let videoJsonDocument = fileName |> getJsonDocument
        let itemElementResult =
            videoJsonDocument.RootElement
            |> tryGetProperty YtItemsPropertyName
            |> Result.either ( fun el -> Ok (el.EnumerateArray() |> List.ofSeq |> List.head) ) Result.Error

        itemElementResult |> should be (ofCase <@ Result<JsonElement, JsonException>.Ok @>)

        let actualResult =
            itemElementResult
            |> Result.valueOr raise
            |> tryGetYtItem

        actualResult |> should be (ofCase <@ Result<YouTubeItem, JsonException>.Ok @>)

    [<Theory>]
    [<InlineData("60-minutes.json")>]
    [<InlineData("youtube-index-songhay-top-ten.json")>]
    let ``fromInput test`` (fileName: string) =

        let videoJsonDocument = fileName |> getJsonDocument
        let actualResult =
            videoJsonDocument.RootElement
            |> fromInput

        actualResult |> should be (ofCase <@ Result<YouTubeItem list, JsonException>.Ok @>)
