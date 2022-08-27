namespace Songhay.Player.YouTube

open System.Net
open Microsoft.Extensions.Logging

open FsToolkit.ErrorHandling

open Songhay.Modules.Bolero.JsonDocumentUtility
open Songhay.Player.YouTube.DisplayItemModelUtility

module DisplayItemModelServiceHandlerUtility =

    let toPublicationIndexData (logger: ILogger) (jsonResult: Result<string, HttpStatusCode>) =
        jsonResult |> tryGetJsonElement logger
        |> Result.bind (fun el -> el |> Index.fromInput)
        |> Option.ofResult
