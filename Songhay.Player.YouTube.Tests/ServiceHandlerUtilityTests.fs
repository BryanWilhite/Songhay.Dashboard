namespace Songhay.Player.YouTube.Tests

module ServiceHandlerUtilityTests =

    open System
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
    open Songhay.Modules.Bolero.JsonDocumentUtility

    open Songhay.Player.YouTube
    open Songhay.Player.YouTube.Models
    open Songhay.Player.YouTube.ServiceHandlerUtility
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
    [<InlineData(YtIndexSonghay)>]
    let ``getPlaylistIndexUri test`` (idString: string) =
        task {
            let id = Identifier.fromString(idString)
            let uri = id |> getPlaylistIndexUri
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
                $"./json/{idString}-index.json"
                |> tryGetCombinedPath projectDirectoryInfo.FullName
                |> Result.valueOr raiseProgramFileError

            File.WriteAllText(path, json)
        }

    [<Theory>]
    [<InlineData(YtIndexSonghay, "news")>]
    let ``getPlaylistSetUri test`` (indexIdString: string, clientIdString: string) =
        task {
            let indexId = Identifier.fromString(indexIdString)
            let clientId = ClientId.fromString(clientIdString)
            let uri = (indexId, clientId) ||> getPlaylistSetUri
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
                $"./json/{indexIdString}-{clientIdString}-playlist.json"
                |> tryGetCombinedPath projectDirectoryInfo.FullName
                |> Result.valueOr raiseProgramFileError

            File.WriteAllText(path, json)
        }

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
    [<InlineData(
        "https://songhay-system-player.azurewebsites.net/api/Player/v1/video/youtube/playlists/songhay/news",
        "called-yt-set-news")>]
    let ``getYtSetKey test`` (location: string, expected: string) =
        let uri = location |> Uri
        let seed = nameof YouTubeMessage.CalledYtSet
        let cacheKey = uri |> getYtSetKey seed
        cacheKey |> should equal expected

    [<Theory>]
    [<InlineData("youtube-index-songhay-top-ten.json")>]
    let ``toYtItems test`` (fileName: string) =
        let json = fileName |> getJson
        let mockLogger = Substitute.For<ILogger>()

        let actual = Ok json |> tryGetJsonElement mockLogger |> toYtItems
        actual |> should be (ofCase<@ Option<YouTubeItem[]>.Some @>)
