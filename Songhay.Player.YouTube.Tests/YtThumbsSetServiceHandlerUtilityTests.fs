namespace Songhay.Player.YouTube.Tests

open System

open System.Net
open System.IO
open System.Net.Http
open System.Reflection

open Xunit
open FsUnit.Xunit
open FsUnit.CustomMatchers
open FsToolkit.ErrorHandling

open Songhay.Modules.Models
open Songhay.Modules.HttpClientUtility
open Songhay.Modules.HttpRequestMessageUtility
open Songhay.Modules.HttpResponseMessageUtility
open Songhay.Modules.ProgramFileUtility

open Songhay.Player.YouTube
open Songhay.Player.YouTube.YtUriUtility

module YtThumbsSetServiceHandlerUtilityTests =

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
    [<InlineData(
        "https://songhay-system-player.azurewebsites.net/api/Player/v1/video/youtube/playlists/songhay/news",
        "called-yt-set-news")>]
    let ``getYtSetKey test`` (location: string, expected: string) =
        let uri = location |> Uri
        let seed = nameof YouTubeMessage.CalledYtSet
        let cacheKey = uri |> YtThumbsSetServiceHandlerUtility.getYtSetKey seed
        cacheKey |> should equal expected

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