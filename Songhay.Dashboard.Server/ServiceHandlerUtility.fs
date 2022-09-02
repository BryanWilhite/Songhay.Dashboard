namespace Songhay.Dashboard.Server

open System.Text.Json

open FsToolkit.ErrorHandling

open Songhay.Dashboard.Client.SyndicationFeedUtility

module ServiceHandlerUtility =

    let toAppData (jsonElementResult: Result<JsonElement, JsonException>) =
        jsonElementResult
        |> Result.bind (fun el -> el |> fromInput)
        |> Result.map (fun input -> input |> List.toArray)
        |> Option.ofResult
