namespace Songhay.Modules.Tests

module HttpClientUtilityTests =

    open System
    open System.IO
    open System.Net.Http
    open System.Reflection
    open System.Threading.Tasks

    open Xunit
    open FsUnit.Xunit
    open FsUnit.CustomMatchers

    open Songhay.Modules.Models
    open Songhay.Modules.HttpClientUtility
    open Songhay.Modules.HttpRequestMessageUtility
    open Songhay.Modules.HttpResponseMessageUtility

    let projectDirectoryInfo =
        Assembly.GetExecutingAssembly()
        |> ProgramAssemblyInfo.getPathFromAssembly "../../../"
        |> DirectoryInfo

    let client = new HttpClient()

    [<Literal>]
    let LIVE_API_BASE_URI = "http://jsonplaceholder.typicode.com"

    [<Theory>]
    [<InlineData("/posts")>]
    [<InlineData("/posts/1")>]
    [<InlineData("/posts/1/comments")>]
    [<InlineData("/comments?postId=1")>]
    let ``client should get`` (location) =
        let getJsonResult response = task {

            let! jsonResult = response |> tryDownloadToStringAsync

            match jsonResult with
            |Error _ -> return false
            | Ok json ->
                String.IsNullOrWhiteSpace(json) |> should be False
                return true
        }

        async {
            let uri = Uri($"{LIVE_API_BASE_URI}{location}", UriKind.Absolute)

            let! responseResult = client |> trySendAsync (get uri) |> Async.AwaitTask

            let actualTask =
                match responseResult with
                | Error _ -> Task.FromResult(false)
                | Ok response ->
                    response.EnsureSuccessStatusCode() |> ignore
                    response.RequestMessage.Method |> should equal HttpMethod.Get
                    response |> getJsonResult

            let! actual = actualTask |> Async.AwaitTask

            actual |> should be True
        }