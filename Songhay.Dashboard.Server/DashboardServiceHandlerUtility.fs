namespace Songhay.Dashboard.Server

open System.Net
open System.Text.Json
open Microsoft.Extensions.Logging

open FsToolkit.ErrorHandling

open Songhay.Modules.JsonDocumentUtility
open Songhay.Dashboard.Client.SyndicationFeedUtility

module DashboardServiceHandlerUtility =
    let toAppData (logger: ILogger) (jsonResult: Result<string, HttpStatusCode>) =
        jsonResult
        |> Result.mapError
               (
                    fun code ->
                        logger.LogError($"The expected {nameof HttpStatusCode}, `{code},` is not here.")
                        exn $"{nameof HttpStatusCode}: {code.ToString()}"
               )
        |> Result.bind (fun json -> json |> tryGetRootElement)
        |> Result.mapError
               (
                   fun err ->
                       logger.LogError(err.Message, err.GetType().FullName, err.Source, err.StackTrace)
                       JsonException err.Message
                )
        |> Result.bind (fun el -> el |> fromInput)
        |> Result.map (fun input -> input |> List.toArray)
        |> Option.ofResult
