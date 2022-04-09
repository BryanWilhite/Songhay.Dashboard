namespace Songhay.Player.YouTube

open System.Text.Json
open FsToolkit.ErrorHandling

open Songhay.Modules.Models
open Songhay.Modules.JsonDocumentUtility
open Songhay.Modules.Publications.Models

open Songhay.Player.YouTube
open Songhay.Player.YouTube.Models
open Songhay.Player.YouTube.YtItemUtility

module DisplayItemModelUtility =

    module Index =

        let tryGetClientIdFromFragment (element: JsonElement) =
            let fragmentClientIdResult = element |> ClientId.fromInput
            let modDateResult = element |> ModificationDate.fromInput

            [
                fragmentClientIdResult |> Result.map (fun _ -> true)
                modDateResult |> Result.map (fun _ -> true)
            ]
            |> List.sequenceResultM
            |> Result.either ( fun _ -> Ok fragmentClientIdResult |> Result.valueOr raise ) Result.Error

        let tryGetDisplayTupleFromDocument (element: JsonElement) =
            let documentClientIdResult = element |> ClientId.fromInput
            let titleResult = element |> Name.fromInput PublicationItem.Document
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
                                displayText = titleResult |> Result.map(fun i -> Some (DisplayText i.Value)) |> Result.valueOr raise
                                resourceIndicator = None
                            },
                            displayItemModelsResult |> Result.valueOr raise
                        )
                )
                Result.Error

        let fromInput (element: JsonElement) =
            let segmentClientIdResult = element |> ClientId.fromInput
            let segmentNameResult = element |> Name.fromInput PublicationItem.Segment
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
                            displayItemModelsResult |> Result.valueOr raise
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
                                    |> Result.map (fun l -> DisplayText (l |> List.head).snippet.channelTitle, l)
                            )
                        |> List.sequenceResultM
                )
