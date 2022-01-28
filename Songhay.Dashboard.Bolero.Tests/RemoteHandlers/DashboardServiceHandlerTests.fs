namespace Songhay.Dashboard.Bolero.Tests.RemoteHandlers

module DashboardServiceHandlerTests =

    open System
    open System.IO
    open System.Net.Http
    open System.Reflection
    open System.Text.Json

    open Oryx

    open Xunit
    open FsUnit.Xunit
    open FsUnit.CustomMatchers

    open Songhay.Modules.Models

    open Songhay.Dashboard.Client
    open Songhay.Dashboard.Server

    let projectDirectoryInfo =
        Assembly.GetExecutingAssembly()
        |> ProgramAssemblyInfo.getPathFromAssembly "../../../"
        |> DirectoryInfo

    let client = new HttpClient()

    [<Theory>]
    [<InlineData(App.appDataLocation)>]
    let ``runRequest test`` (location) =
        task {
            let uri = Uri(location, UriKind.Absolute)
            let ctx = client |> OryxUtility.withHttpContext OryxUtility.contextGetter
            let request = OryxUtility.requestForJson uri.AbsoluteUri
            let! result = request |> OryxUtility.runRequest ctx

            let actual =
                match result with
                | Ok json -> true
                | Error _ -> false

            actual |> should be True
        }
