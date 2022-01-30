namespace Songhay.Modules

open System.IO
open System.Net
open System.Net.Http
open System.Text.Json

module HttpResponseMessageUtility =

    let isMovedOrRedirected (response: HttpResponseMessage) =
        response.StatusCode = HttpStatusCode.Moved ||
        response.StatusCode = HttpStatusCode.MovedPermanently ||
        response.StatusCode = HttpStatusCode.Redirect

    let tryDownloadToByteArrayAsync (response: HttpResponseMessage) =
        try
            task {
                if response.StatusCode = HttpStatusCode.OK then
                    let! s = response.Content.ReadAsByteArrayAsync().ConfigureAwait(false)

                    return Ok s
                else
                    return Error response.StatusCode
            }
        finally
            response.Dispose()

    let tryDownloadToFileAsync (path: string) (response: HttpResponseMessage) =
        let buffer = Array.init 32768 (fun i -> byte(i*i))
        let mutable bytesRead = -1
        try
            task {
                try
                    use! stream = response.Content.ReadAsStreamAsync().ConfigureAwait(false)
                    use fs = File.Create(path)
                    while (not (bytesRead > 0)) do
                        bytesRead <- stream.Read(buffer, 0, buffer.Length)
                        fs.Write(buffer, 0, bytesRead)

                    return Ok ()
                with
                | exn -> return Error exn
            }
            finally
                response.Dispose()

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

    let tryStreamToInstanceAsync (options: JsonSerializerOptions) (response: HttpResponseMessage) =
        try
            task {
                use! stream = response.Content.ReadAsStreamAsync().ConfigureAwait(false)

                if (stream = null || stream.CanRead = false) then

                    return Error "The expected stream is not here."
                else
                    try
                        let! instance = JsonSerializer.DeserializeAsync<_>(stream, options).ConfigureAwait(false)

                        return Ok instance
                    with
                    | exn -> return Error exn.Message
            }
        finally
            response.Dispose()
