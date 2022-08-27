namespace Songhay.Dashboard.Server

module ServiceHandlerUtility =

    open System.Text.Json

    open FsToolkit.ErrorHandling

    open Songhay.Dashboard.Client.SyndicationFeedUtility

    let toAppData (jsonElementResult: Result<JsonElement, JsonException>) =
        jsonElementResult
        |> Result.bind (fun el -> el |> fromInput)
        |> Result.map (fun input -> input |> List.toArray)
        |> Option.ofResult
