namespace Songhay.Player.YouTube

open System.Net
open Microsoft.Extensions.Logging

open FsToolkit.ErrorHandling

open Songhay.Modules.Bolero.JsonDocumentUtility
open Songhay.Player.YouTube

module YtThumbsServiceHandlerUtility =

    let toYtItems (logger: ILogger) (jsonResult: Result<string, HttpStatusCode>) =
        jsonResult |> tryGetJsonElement logger
        |> Result.bind (fun el -> el |> YtItemUtility.fromInput)
        |> Result.map (fun input -> input |> List.toArray)
        |> Option.ofResult
