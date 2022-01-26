namespace Songhay.Dashboard.Server

module OryxUtility =
    open System.Text.Json

    open Oryx
    open Oryx.SystemTextJson.ResponseReader

    let options = JsonSerializerOptions()

    let contextGetter() =
        HttpContext.defaultContext

    let requestForJson uri =
        GET
        >=> withUrl uri
        >=> fetch
        >=> json options

    let runRequest context request = request |> runAsync context

    let withHttpContext contextGetter client =
        contextGetter()
        |> HttpContext.withHttpClient client
