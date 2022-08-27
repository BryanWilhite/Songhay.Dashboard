namespace Songhay.Player.YouTube.Tests

module YtThumbsServiceHandlerUtilityTests =

    open System.Net
    open System.IO
    open System.Net.Http
    open System.Reflection
    open Microsoft.Extensions.Logging

    open NSubstitute

    open Xunit
    open FsUnit.Xunit
    open FsUnit.CustomMatchers
    open FsToolkit.ErrorHandling

    open Songhay.Modules.Models
    open Songhay.Modules.HttpClientUtility
    open Songhay.Modules.HttpRequestMessageUtility
    open Songhay.Modules.HttpResponseMessageUtility
    open Songhay.Modules.ProgramFileUtility

    open Songhay.Player.YouTube.Models
    open Songhay.Player.YouTube.YtThumbsServiceHandlerUtility
    open Songhay.Player.YouTube.YtUriUtility

    let projectDirectoryInfo =
        Assembly.GetExecutingAssembly()
        |> ProgramAssemblyInfo.getPathFromAssembly "../../../"
        |> Result.valueOr raiseProgramFileError
        |> DirectoryInfo

    let getJson (fileName: string) =
        let path =
            $"./json/{fileName}"
            |> tryGetCombinedPath projectDirectoryInfo.FullName
            |> Result.valueOr raiseProgramFileError
        File.ReadAllText(path)

    let client = new HttpClient()

    [<Theory>]
    [<InlineData(YtIndexSonghayTopTen)>]
    let ``getPlaylistUri test`` (idString: string) =
        task {
            let id = Identifier.fromString(idString)
            let uri = id |> getPlaylistUri
            let! responseResult = client |> trySendAsync (get uri)
            responseResult |> should be (ofCase<@ Result<HttpResponseMessage,exn>.Ok @>)
            let response = responseResult |> Result.valueOr raise

            let! jsonResult = response |> tryDownloadToStringAsync
            jsonResult |> should be (ofCase<@ Result<string,HttpStatusCode>.Ok @>)
            let json =
                jsonResult
                |> Result.mapError ( fun code -> exn $"{nameof HttpStatusCode}: {code.ToString()}" )
                |> Result.valueOr raise

            let path =
                $"./json/{idString}.json"
                |> tryGetCombinedPath projectDirectoryInfo.FullName
                |> Result.valueOr raiseProgramFileError

            File.WriteAllText(path, json)
        }

    [<Theory>]
    [<InlineData("youtube-index-songhay-top-ten.json")>]
    let ``toDomainData test`` (fileName: string) =
        let json = fileName |> getJson
        let mockLogger = Substitute.For<ILogger>()

        let actual = Ok json |> toYtItems mockLogger
        actual |> should be (ofCase<@ Option<YouTubeItem[]>.Some @>)
