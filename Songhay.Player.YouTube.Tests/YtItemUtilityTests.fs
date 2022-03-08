namespace Songhay.Player.YouTube.Tests

module YtItemUtilityTests =

    open System
    open System.IO
    open System.Reflection
    open System.Text.Json
    open Xunit
    open FsUnit.Xunit
    open FsUnit.CustomMatchers
    open FsToolkit.ErrorHandling

    open Songhay.Modules.Models
    open Songhay.Modules.ProgramFileUtility

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
