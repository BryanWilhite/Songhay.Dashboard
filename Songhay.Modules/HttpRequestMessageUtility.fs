namespace Songhay.Modules

open System
open System.Linq
open System.Collections
open System.Globalization
open System.Net.Http
open System.Text

open Songhay.Modules.MimeTypes

module HttpRequestMessageUtility =

    let message (uri: Uri) (method: HttpMethod) =
        new HttpRequestMessage(method, uri)

    let post (uri: Uri) =
        HttpMethod.Post |> message uri

    let get (uri: Uri) =
        HttpMethod.Get |> message uri

    let put (uri: Uri) =
        HttpMethod.Put |> message uri

    let delete (uri: Uri) =
        HttpMethod.Delete |> message uri

    let patch (uri: Uri) =
        HttpMethod.Patch |> message uri

    let withStringContent (body: string) (encoding: Encoding) (mediaType: string) (message: HttpRequestMessage) =
        message.Content <- new StringContent(body, encoding, mediaType)

        message

    let withHtmlFormContent (formData: Hashtable) (message: HttpRequestMessage) =
        let body =
            let sb = StringBuilder()

            formData.OfType<DictionaryEntry>()
            |> Seq.iteri (fun i entry ->
                let s =
                    if i = 0 then String.Format(CultureInfo.InvariantCulture, "{0}={1}", entry.Key, entry.Value)
                    else String.Format(CultureInfo.InvariantCulture, "&{0}={1}", entry.Key, entry.Value)
                sb.Append(s) |> ignore
            )

            sb.ToString()

        message |> withStringContent body Encoding.UTF8 ApplicationFormUrlEncoded

    let withJsonStringContent (body: string) (message: HttpRequestMessage) =
        message |> withStringContent body Encoding.UTF8 ApplicationJson

    let withXmlStringContent (body: string) (message: HttpRequestMessage) =
        message |> withStringContent body Encoding.UTF8 ApplicationXml
