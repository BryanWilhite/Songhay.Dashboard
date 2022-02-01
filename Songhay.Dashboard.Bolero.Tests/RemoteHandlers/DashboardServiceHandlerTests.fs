namespace Songhay.Dashboard.Bolero.Tests.RemoteHandlers

module DashboardServiceHandlerTests =

    open System
    open System.IO
    open System.Net.Http
    open System.Reflection

    open Xunit
    open FsUnit.Xunit
    open FsUnit.CustomMatchers

    open Songhay.Modules.Models
    open Songhay.Modules.HttpClientUtility
    open Songhay.Modules.HttpRequestMessageUtility

    open Songhay.Dashboard.Client

    let projectDirectoryInfo =
        Assembly.GetExecutingAssembly()
        |> ProgramAssemblyInfo.getPathFromAssembly "../../../"
        |> DirectoryInfo

    let client = new HttpClient()

    [<Theory>]
    [<InlineData(App.appDataLocation)>]
    let ``runRequest test (async)`` (location) =
        async {
            let uri = Uri(location, UriKind.Absolute)
            let! responseResult =
                client
                |> trySendAsync (get uri)
                |> Async.AwaitTask

            let actual =
                match responseResult with
                | Ok json -> true
                | Error _ -> false

            actual |> should be True
        }

    [<Theory>]
    [<InlineData(App.appDataLocation)>]
    let ``runRequest test (task)`` (location) =
        task {
            let uri = Uri(location, UriKind.Absolute)

            let! responseResult = client |> trySendAsync (get uri)

            let actual =
                match responseResult with
                | Ok json -> true
                | Error _ -> false

            actual |> should be True
        }
