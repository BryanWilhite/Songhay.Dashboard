namespace Songhay.Player.YouTube.Tests

open Songhay.Modules.Publications.Models
open Songhay.Player.YouTube.Models

module DisplayItemModelUtilityTests =

    open System.IO
    open System.Reflection
    open System.Text.Json
    open Xunit
    open FsUnit.Xunit
    open FsUnit.CustomMatchers
    open FsToolkit.ErrorHandling

    open Songhay.Modules.Models
    open Songhay.Modules.ProgramFileUtility

    open Songhay.Player.YouTube.DisplayItemModelUtility

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
    [<InlineData("songhay-index.json")>]
    let ``Index.fromInput test`` (fileName: string) =

        let videoJsonDocument = fileName |> getJsonDocument
        let actualResult =
            videoJsonDocument.RootElement
            |> Index.fromInput

        actualResult
        |> should be (ofCase <@ Result<ClientId * Name * (DisplayItemModel * ClientId []) [],JsonException>.Ok @>)

    [<Theory>]
    [<InlineData("songhay-news-playlist.json")>]
    let ``ThumbsSet.fromInput test`` (fileName: string) =

        let videoJsonDocument = fileName |> getJsonDocument
        let actualResult =
            videoJsonDocument.RootElement
            |> ThumbsSet.fromInput

        actualResult
        |> should be (ofCase <@ Result<(DisplayText * YouTubeItem []) list,JsonException>.Ok @>)

        let actual = actualResult |> Result.valueOr raise
        actual |> should not' (be Empty)
