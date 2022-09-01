namespace Songhay.Player.YouTube.Tests

open System.Text.Json

module RemoteHandlerUtilityTests =

    open System.IO
    open System.Net.Http
    open System.Reflection
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

    let projectDirectoryInfo =
        Assembly.GetExecutingAssembly()
        |> ProgramAssemblyInfo.getPathFromAssembly "../../../"
        |> Result.valueOr raiseProgramFileError
        |> DirectoryInfo

    let client = new HttpClient()

    let writeJson (fileName: string) (json:string) =
        let path =
            $"./json/{fileName}"
            |> tryGetCombinedPath projectDirectoryInfo.FullName
            |> Result.valueOr raiseProgramFileError
        File.WriteAllText(path, json)

    [<Theory>]
    [<InlineData(YtIndexSonghay, "songhay-index.json")>]
    let ``request test (async)`` (indexName: string, jsonFileName: string) =
        async {
            let uri = indexName |> Identifier.Alphanumeric |> getPlaylistIndexUri
            let mockLogger = Substitute.For<ILogger>()
            let dataGetter (result: Result<JsonElement, JsonException>) =
                result |> should be (ofCase<@ Result<JsonElement, JsonException>.Ok @>)
                let opt = result |> Option.ofResult
                (jsonFileName, opt.Value.ToString()) ||> writeJson
                opt

            let! responseResult = client |> trySendAsync (get uri) |> Async.AwaitTask

            responseResult |> should be (ofCase<@ Result<HttpResponseMessage,exn>.Ok @>)

            let! handlerResult = responseResult |> toHandlerOutputAsync mockLogger dataGetter |> Async.AwaitTask
            handlerResult |> should be (ofCase<@ Option<unit>.Some @>)
        }
