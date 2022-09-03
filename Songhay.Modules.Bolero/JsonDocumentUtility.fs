namespace Songhay.Modules.Bolero

open System.Net
open System.Text.Json
open Microsoft.Extensions.Logging

open FsToolkit.ErrorHandling

open Songhay.Modules.JsonDocumentUtility

module JsonDocumentUtility =

    let tryGetJsonElement (logger: ILogger option) (jsonResult: Result<string, HttpStatusCode>) = 
        jsonResult
        |> Result.mapError
               (
                    fun code ->
                        if logger.IsSome then
                            logger.Value.LogError($"The expected {nameof HttpStatusCode}, `{code},` is not here.")
                        exn $"{nameof HttpStatusCode}: {code.ToString()}"
               )
        |> Result.bind (fun json -> json |> tryGetRootElement)
        |> Result.mapError
               (
                   fun err ->
                       if logger.IsSome then
                           logger.Value.LogError(err.Message, err.GetType().FullName, err.Source, err.StackTrace)
                       JsonException err.Message
                )
