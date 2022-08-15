namespace Songhay.Player.YouTube

open System.Text.Json
open FsToolkit.ErrorHandling

open Songhay.Modules.Models
open Songhay.Modules.JsonDocumentUtility
open Songhay.Modules.Publications.Models
open Songhay.Modules.Publications.DisplayItemModelUtility

open Songhay.Player.YouTube
open Songhay.Player.YouTube.Models

module DisplayItemModelUtility =

    module Index =

        let tryGetClientIdFromFragment (element: JsonElement) = element |> ClientId.fromInput false

        let tryGetDisplayTupleFromDocument (element: JsonElement) =
            let documentClientIdResult = element |> ClientId.fromInput false
            let titleResult = element |> defaultDisplayTextGetter None Document false
            let displayItemModelsResult =
                element
                |> tryGetProperty $"{nameof Fragment}s"
                |> Result.bind
                    (
                        fun el ->
                            el.EnumerateArray()
                            |> List.ofSeq
                            |> List.map (fun i -> i |> tryGetClientIdFromFragment)
                            |> List.sequenceResultM
                    )

            [
                documentClientIdResult |> Result.map (fun _ -> true)
                titleResult |> Result.map (fun _ -> true)
            ]
            |> List.sequenceResultM
            |> Result.either
                (
                    fun _ ->
                        Ok (
                            {
                                id = documentClientIdResult |> Result.map(fun i -> i.toIdentifier) |> Result.valueOr raise
                                itemName = None
                                displayText = titleResult |> Result.map id |> Result.valueOr raise
                                resourceIndicator = None
                            },
                            displayItemModelsResult |> Result.map( fun l -> l |> Array.ofList ) |> Result.valueOr raise
                        )
                )
                Result.Error

        let fromInput (element: JsonElement) =
            let segmentClientIdResult = element |> ClientId.fromInput false
            let segmentNameResult = element |> Name.fromInput PublicationItem.Segment false
            let displayItemModelsResult =
                element |> tryGetProperty $"{nameof Document}s"
                |> Result.bind
                    (
                        fun el ->
                            el.EnumerateArray()
                            |> List.ofSeq
                            |> List.map (fun i -> i |> tryGetDisplayTupleFromDocument)
                            |> List.sequenceResultM
                    )
            [
                segmentClientIdResult |> Result.map(fun _ -> true)
                segmentNameResult |> Result.map(fun _ -> true)
            ]
            |> List.sequenceResultM
            |> Result.either
                (
                    fun _ ->
                        Ok (
                            segmentClientIdResult |> Result.valueOr raise,
                            segmentNameResult |> Result.valueOr raise,
                            displayItemModelsResult |> Result.map (fun input -> input |> Array.ofList) |> Result.valueOr raise
                        )
                )
                Result.Error

    module ThumbsSet =
        let fromInput (element: JsonElement) =
            element |> tryGetProperty "set"
            |> Result.bind
                (
                    fun el ->
                        el.EnumerateArray()
                        |> List.ofSeq
                        |> List.map ( fun el ->
                                    el |> YtItemUtility.fromInput
                                    |> Result.map (
                                        fun l ->
                                            DisplayText (l |> List.head).snippet.channelTitle, l |> Array.ofList
                                        )
                            )
                        |> List.sequenceResultM
                )
