namespace Songhay.Dashboard.Server.Tests

module DashboardServiceHandlerUtilityTests =

    open System
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
    open Songhay.Modules.ProgramFileUtility

    open Songhay.Dashboard.Client

    let projectDirectoryInfo =
        Assembly.GetExecutingAssembly()
        |> ProgramAssemblyInfo.getPathFromAssembly "../../../"
        |> Result.valueOr raiseProgramFileError
        |> DirectoryInfo

    let client = new HttpClient()

    [<Theory>]
    [<InlineData(App.AppDataLocation)>]
    let ``runRequest test (async)`` location =
        async {
            let uri = Uri(location, UriKind.Absolute)

            let! responseResult = client |> trySendAsync (get uri) |> Async.AwaitTask

            responseResult |> should be (ofCase<@ Result<HttpResponseMessage,exn>.Ok @>)
        }

    [<Theory>]
    [<InlineData(App.AppDataLocation)>]
    let ``runRequest test (task)`` location =
        task {
            let uri = Uri(location, UriKind.Absolute)

            let! responseResult = client |> trySendAsync (get uri)

            responseResult |> should be (ofCase<@ Result<HttpResponseMessage,exn>.Ok @>)
        }
