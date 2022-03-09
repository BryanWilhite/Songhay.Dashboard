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

    let videoJsonDocumentPath =
        "./json/60-minutes.json"
        |> tryGetCombinedPath projectDirectoryInfo.FullName
        |> Result.valueOr raiseProgramFileError

    let videoJsonDocument =
        JsonDocument.Parse(File.ReadAllText(videoJsonDocumentPath))

    [<Fact>]
    let ``video-yt-bowie0-videos.json should have `items` property`` () =
        let actual =
            match videoJsonDocument.RootElement.TryGetProperty YtItemsPropertyName with
            | true, _ -> true
            | _ -> false

        actual |> should be True

    [<Fact>]
    let ``tryGetYtContentDetails test`` () =
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

    [<Fact>]
    let ``tryGetYtResourceId test`` () =
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

    [<Fact>]
    let ``tryGetYtThumbnails test`` () =
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

    [<Fact>]
    let ``tryGetYtSnippet test`` () =
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

    [<Fact>]
    let ``tryGetYtItem test`` () =
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

    [<Fact>]
    let ``tryGetYtItems test`` () =

        let actualResult =
            videoJsonDocument.RootElement
            |> tryGetYtItems

        actualResult |> should be (ofCase <@ Result<YouTubeItem list, JsonException>.Ok @>)
