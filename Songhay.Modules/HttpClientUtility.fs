namespace Songhay.Modules

open System.Net.Http

module HttpClientUtility =

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
