namespace Songhay.Modules.Tests

module HttpClientUtilityTests =

    open System
    open System.IO
    open System.Net.Http
    open System.Reflection

    open FSharp.Data
    open FsToolkit.ErrorHandling

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

    let isJsonResult isExpectedJson response = task {

        let! jsonResult = response |> tryDownloadToStringAsync

        match jsonResult with
        |Error _ -> return false
        | Ok json ->
            String.IsNullOrWhiteSpace(json) |> should be False
            return isExpectedJson json
    }

    type providerGet = JsonProvider<"""[{ "id": 1, "title": "foo", "body": "bar", "userId": 1 }]""">

    [<Theory>]
    [<InlineData("/posts")>]
    let ``client should get`` (location: string) =

        let isExpectedJson json =
            let docs = json |> providerGet.Parse
            docs.Length |> should be (greaterThan 0)
            docs |> Array.forall (fun doc -> doc.Id > 0 && doc.UserId > 0) |> should be True
            true

        async {
            let uri = Uri($"{LIVE_API_BASE_URI}{location}", UriKind.Absolute)

            let! responseResult =
                client
                |> trySendAsync (get uri)
                |> Async.AwaitTask

            responseResult |> should be (ofCase <@ Result<HttpResponseMessage, exn>.Ok @>)

            let response = responseResult |> Result.valueOr raise
            response.EnsureSuccessStatusCode() |> ignore

            response.RequestMessage.Method.ToString().ToUpperInvariant()
            |> should equal (HttpMethod.Get.ToUpperInvariant())

            let! actual = response |> isJsonResult isExpectedJson |> Async.AwaitTask

            actual |> should be True
        }

    [<Theory>]
    [<InlineData("/posts/1")>]
    let ``client should delete`` (location: string) =

        let isExpectedJson json =
            json |> should equal "{}"
            true

        async {
            let uri = Uri($"{LIVE_API_BASE_URI}{location}", UriKind.Absolute)

            let! responseResult =
                client
                |> trySendAsync (delete uri)
                |> Async.AwaitTask

            responseResult |> should be (ofCase <@ Result<HttpResponseMessage, exn>.Ok @>)

            let response = responseResult |> Result.valueOr raise
            response.EnsureSuccessStatusCode() |> ignore

            response.RequestMessage.Method.ToString().ToUpperInvariant()
            |> should equal (HttpMethod.Delete.ToUpperInvariant())

            let! actual = response |> isJsonResult isExpectedJson |> Async.AwaitTask

            actual |> should be True
        }

    type providerPost = JsonProvider<"""{ "id": 101, "title": "foo", "body": "bar", "userId": 1 }""">

    [<Theory>]
    [<InlineData("/posts", """{ "title": "foo", "body": "bar", "userId": 1 }""")>]
    let ``client should post`` (location: string, data: string) =
        let isExpectedJson json =
            let doc = json |> providerPost.Parse
            (doc.Id > 0 && doc.Title = "foo" && doc.Body = "bar" && doc.UserId = 1) |> should be True
            true

        async {
            let uri = Uri($"{LIVE_API_BASE_URI}{location}", UriKind.Absolute)

            let! responseResult =
                client
                |> trySendAsync (post uri |> withJsonStringContent data)
                |> Async.AwaitTask

            responseResult |> should be (ofCase <@ Result<HttpResponseMessage, exn>.Ok @>)

            let response = responseResult |> Result.valueOr raise
            response.EnsureSuccessStatusCode() |> ignore

            int response.StatusCode
            |> should equal HttpStatusCodes.Created

            response.RequestMessage.Method.ToString().ToUpperInvariant()
            |> should equal (HttpMethod.Post.ToUpperInvariant())

            let! actual = response |> isJsonResult isExpectedJson |> Async.AwaitTask

            actual |> should be True
        }

    type providerPut = JsonProvider<"""{ "id": 1, "title": "foo", "body": "bar", "userId": 1 }""">

    [<Theory>]
    [<InlineData("/posts/1", """{ "id": 1, "title": "foo", "body": "bar", "userId": 1 }""")>]
    let ``client should put`` (location: string, data: string) =
        let isExpectedJson json =
            let doc = json |> providerPost.Parse
            (doc.Id = 1 && doc.Title = "foo" && doc.Body = "bar" && doc.UserId = 1) |> should be True
            true

        async {
            let uri = Uri($"{LIVE_API_BASE_URI}{location}", UriKind.Absolute)

            let! responseResult =
                client
                |> trySendAsync (put uri |> withJsonStringContent data)
                |> Async.AwaitTask

            responseResult |> should be (ofCase <@ Result<HttpResponseMessage, exn>.Ok @>)

            let response = responseResult |> Result.valueOr raise
            response.EnsureSuccessStatusCode() |> ignore

            response.RequestMessage.Method.ToString().ToUpperInvariant()
            |> should equal (HttpMethod.Put.ToUpperInvariant())

            let! actual = response |> isJsonResult isExpectedJson |> Async.AwaitTask

            actual |> should be True
        }
    type providerPatch = JsonProvider<"""{ "body": "bar" }""">

    [<Theory>]
    [<InlineData("/posts/1", """{ "id": 1, "title": "foo", "body": "bar", "userId": 1 }""")>]
    let ``client should patch`` (location: string, data: string) =
        let isExpectedJson json =
            let doc = json |> providerPost.Parse
            (doc.Id = 1 && doc.Body = "bar") |> should be True
            true

        async {
            let uri = Uri($"{LIVE_API_BASE_URI}{location}", UriKind.Absolute)

            let! responseResult =
                client
                |> trySendAsync (patch uri |> withJsonStringContent data)
                |> Async.AwaitTask

            responseResult |> should be (ofCase <@ Result<HttpResponseMessage, exn>.Ok @>)

            let response = responseResult |> Result.valueOr raise
            response.EnsureSuccessStatusCode() |> ignore

            response.RequestMessage.Method.ToString().ToUpperInvariant()
            |> should equal (HttpMethod.Patch.ToUpperInvariant())

            let! actual = response |> isJsonResult isExpectedJson |> Async.AwaitTask

            actual |> should be True
        }