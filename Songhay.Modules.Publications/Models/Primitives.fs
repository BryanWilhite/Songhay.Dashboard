module Songhay.Modules.Publications.Models

open System.Text.Json

open Songhay.Modules.Models
open Songhay.Modules.JsonDocumentUtility

type PublicationItem =
    | Segment
    | Document
    | Fragment

type Id =
    | Id of Identifier

    static member fromInput (itemType: PublicationItem) (element: JsonElement) =
        let getId idString =
            Id(Identifier.fromString(idString))
        match itemType with
        | Segment -> element |> tryGetProperty $"{nameof Segment}{nameof Id}" |> Result.map (fun el -> el.GetString() |> getId)
        | Document -> element |> tryGetProperty $"{nameof Document}{nameof Id}" |> Result.map (fun el -> el.GetString() |> getId)
        | Fragment -> element |> tryGetProperty $"{nameof Fragment}{nameof Id}" |> Result.map (fun el -> el.GetString() |> getId)

type Title =
    | Title of string

    static member fromInput (element: JsonElement) =
        element
        |> tryGetProperty (nameof Title)
        |> Result.map (fun el -> Title (el.GetString()))

type Name =
    | Name of string

    static member fromInput (itemType: PublicationItem) (element: JsonElement) =
        match itemType with
        | Segment -> element |> tryGetProperty $"{nameof Segment}{nameof Name}" |> Result.map (fun el -> Name (el.GetString()))
        | Document -> element |> tryGetProperty $"{nameof Title}" |> Result.map (fun el -> Name (el.GetString()))
        | Fragment -> element |> tryGetProperty $"{nameof Fragment}{nameof Name}" |> Result.map (fun el -> Name (el.GetString()))

    member this.Value = let (Name v) = this in v

type Path =
    | Path of string

    static member fromInput (element: JsonElement) =
        element
        |> tryGetProperty (nameof Path)
        |> Result.map (fun el -> Path (el.GetString()))

type FileName =
    | FileName of string

    static member fromInput (element: JsonElement) =
        element
        |> tryGetProperty (nameof FileName)
        |> Result.map (fun el -> FileName (el.GetString()))

type IsActive =
    | IsActive of bool

    static member fromInput (element: JsonElement) =
        element
        |> tryGetProperty (nameof IsActive)
        |> Result.map (fun el -> IsActive (el.GetBoolean()))
