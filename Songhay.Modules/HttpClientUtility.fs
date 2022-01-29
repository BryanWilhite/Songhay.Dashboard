namespace Songhay.Modules

open System
open System.Net
open System.Net.Http

module HttpClientUtility =
    let message (uri: Uri) (method: HttpMethod) =
        new HttpRequestMessage(method, uri)

    let get (uri: Uri) =
        HttpMethod.Get |> message uri

    let trySendAsync (message: HttpRequestMessage) (client: HttpClient) =
        try
            task {
                try
                    let! response = client.SendAsync(message).ConfigureAwait(false)

                    return Ok response
                with
                | exn -> return Error exn
            }
        finally
            message.Dispose()

    let tryDownloadToStringAsync (response: HttpResponseMessage) =
        try
            task {
                if response.StatusCode = HttpStatusCode.OK then
                    let! s = response.Content.ReadAsStringAsync().ConfigureAwait(false)
                    return Ok s
                else
                    return Error response.StatusCode
            }
        finally
            response.Dispose()
