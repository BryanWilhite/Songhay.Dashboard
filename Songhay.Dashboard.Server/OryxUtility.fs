namespace Songhay.Dashboard.Server

module OryxUtility =
    open System.Text.Json

    open Oryx
    open Oryx.SystemTextJson.ResponseReader

    let options = JsonSerializerOptions()

    let requestForJson uri =
        GET
        >=> withUrl uri
        >=> fetch
        >=> json options

    let runRequest context request = request |> runAsync context

    let toHttpContext client =
        HttpContext.defaultContext
        |> HttpContext.withHttpClient client