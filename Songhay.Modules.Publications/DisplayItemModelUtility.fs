﻿namespace Songhay.Modules.Publications

open System.Text.Json
open FsToolkit.ErrorHandling

open Songhay.Modules.Models
open Songhay.Modules.JsonDocumentUtility
open Songhay.Modules.Publications.Models

module DisplayItemModelUtility =

    let tryGetDisplayItemFromFragment (element: JsonElement) =
        let fragmentClientIdResult = element |> ClientId.fromInput
        let modDateResult = element |> ModificationDate.fromInput

        [
            fragmentClientIdResult |> Result.map (fun _ -> true)
            modDateResult |> Result.map (fun _ -> true)
        ]
        |> List.sequenceResultM
        |> Result.either
            (
                fun _ ->
                    Ok {
                        id = fragmentClientIdResult |> Result.map(fun i -> i.toIdentifier) |> Result.valueOr raise
                        itemName = None
                        displayText = None
                        resourceIndicator = None
                    }
            )
            Result.Error

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
                        |> List.map (fun i -> i |> tryGetDisplayItemFromFragment)
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
            element
            |> tryGetProperty $"{nameof Document}s"
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
