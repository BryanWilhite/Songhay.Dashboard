namespace Songhay.Player.YouTube.Tests

open System.IO
open System.Net.Http
open System.Reflection
open System.Text.Json
open Microsoft.Extensions.Logging
open Xunit

open FsUnit.Xunit
open FsUnit.CustomMatchers
open FsToolkit.ErrorHandling

open NSubstitute

open Songhay.Modules.Models
open Songhay.Modules.HttpClientUtility
open Songhay.Modules.HttpRequestMessageUtility
open Songhay.Modules.ProgramFileUtility
open Songhay.Modules.Bolero.RemoteHandlerUtility
open Songhay.Player.YouTube.YtUriUtility

module RemoteHandlerUtilityTests =

    let projectDirectoryInfo =
        Assembly.GetExecutingAssembly()
        |> ProgramAssemblyInfo.getPathFromAssembly "../../../"
        |> Result.valueOr raiseProgramFileError
        |> DirectoryInfo

    let client = new HttpClient()

    let writeJsonAsync (fileName: string) (json:string) =
        let path =
            $"./json/{fileName}"
            |> tryGetCombinedPath projectDirectoryInfo.FullName
            |> Result.valueOr raiseProgramFileError
        File.WriteAllTextAsync(path, json)

    [<Theory>]
    [<InlineData(YtIndexSonghay, "songhay-index.json")>]
    let ``request test (async)`` (indexName: string, jsonFileName: string) =
        async {
            let uri = indexName |> Identifier.Alphanumeric |> getPlaylistIndexUri
            let mockLogger = Substitute.For<ILogger>() |> Some
            let dataGetter (result: Result<JsonElement, JsonException>) =
                result |> should be (ofCase<@ Result<JsonElement, JsonException>.Ok @>)
                result |> Option.ofResult

            let! responseResult = client |> trySendAsync (get uri) |> Async.AwaitTask

            responseResult |> should be (ofCase<@ Result<HttpResponseMessage,exn>.Ok @>)

            let! handlerResult = responseResult |> toHandlerOutputAsync mockLogger dataGetter |> Async.AwaitTask
            handlerResult |> should be (ofCase<@ Option<JsonElement>.Some @>)

            (jsonFileName, handlerResult.Value.ToString()) ||> writeJsonAsync |> Async.AwaitTask |> ignore
        }
