namespace Songhay.Modules

open System
open System.Net
open System.Net.Http

module HttpClientUtility =

    let sendAsync (uri: Uri) (method: HttpMethod) (client: HttpClient) =
        task {
            use request = new HttpRequestMessage(method, uri)
            let! response = client.SendAsync(request).ConfigureAwait(false)

            return response
        }

    let getAsync (uri: Uri) (client: HttpClient) =
        task {
            let! response = client |> sendAsync uri HttpMethod.Get

            return response
        }

    let tryDownloadToStringAsync (response: HttpResponseMessage) =
        let contentResult =
            task {
                if response.StatusCode = HttpStatusCode.OK then
                    let! s = response.Content.ReadAsStringAsync().ConfigureAwait(false)
                    return Ok s
                else
                    return Error response.StatusCode
            }

        contentResult
