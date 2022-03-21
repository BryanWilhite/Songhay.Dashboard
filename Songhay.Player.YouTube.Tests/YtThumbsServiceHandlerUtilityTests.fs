namespace Songhay.Player.YouTube.Tests

open System.Net

module YtThumbsServiceHandlerUtilityTests =

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

    open Songhay.Player.YouTube.YtUriUtility

    let projectDirectoryInfo =
        Assembly.GetExecutingAssembly()
        |> ProgramAssemblyInfo.getPathFromAssembly "../../../"
        |> Result.valueOr raiseProgramFileError
        |> DirectoryInfo

    let client = new HttpClient()

    [<Theory>]
    [<InlineData("youtube-index-songhay-top-ten")>]
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

