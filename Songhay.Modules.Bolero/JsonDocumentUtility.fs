namespace Songhay.Modules.Bolero

module JsonDocumentUtility =

    open System.Net
    open System.Text.Json
    open Microsoft.Extensions.Logging

    open FsToolkit.ErrorHandling

    open Songhay.Modules.JsonDocumentUtility

    let tryGetJsonElement (logger: ILogger) (jsonResult: Result<string, HttpStatusCode>) = 
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
