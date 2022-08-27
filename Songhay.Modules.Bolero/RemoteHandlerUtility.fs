namespace Songhay.Modules.Bolero

open System.Net
open System.Net.Http
open System.Threading.Tasks
open Microsoft.Extensions.Logging

open Songhay.Modules.HttpResponseMessageUtility

module RemoteHandlerUtility =

    let toHandlerOutputAsync<'TOutput>
        (logger: ILogger)
        (dataGetter: ILogger -> Result<string, HttpStatusCode> -> 'TOutput option)
        (responseResult: Result<HttpResponseMessage,exn>) : Task<'TOutput option> =
        task {
            logger.LogInformation("processing result...")

            match responseResult with
            | Result.Error err ->
                logger.LogError(err.Message, err.GetType().FullName, err.Source, err.StackTrace)
                return None

            | Result.Ok response ->
                let! jsonResult = response |> tryDownloadToStringAsync
                return jsonResult |> dataGetter logger
        }
