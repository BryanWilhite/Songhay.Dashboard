namespace Songhay.Modules.Models

open System
open System.Text.Json

open Songhay.Modules.JsonDocumentUtility

/// <summary>
/// Defines the conventional identifier or sort ordinal.
/// </summary>
type Identifier =
    | Alphanumeric of string
    | Numeric of int

    /// <summary>
    /// Generates an instance of <see cref="Identifier.Numeric" />
    /// from conventional JSON input.
    /// </summary>
    static member fromInt32 i = Numeric i

    /// <summary>
    /// Generates an instance of <see cref="Identifier.Alphanumeric" />
    /// from conventional JSON input.
    /// </summary>
    static member fromString s = Alphanumeric s

    /// <summary>
    /// Reduces this instance
    /// to <c>Some</c> <see cref="int" /> value.
    /// </summary>
    member this.Int32Value =
        match this with
        | Alphanumeric a ->
            match Int32.TryParse(a) with
            | true, n -> Some n
            | false, _ -> None
        | Numeric n -> Some n

    /// <summary>
    /// Reduces this instance
    /// to <c>Some</c> <see cref="string" /> value.
    /// </summary>
    member this.StringValue =
        match this with
        | Alphanumeric a -> a
        | Numeric n -> $"{n}"

/// <summary>
/// Defines the conventional identifier exposed to a Studio Client.
/// </summary>
type ClientId =
    | ClientId of Identifier

    /// <summary>
    /// Generates an instance of this type
    /// from conventional JSON input.
    /// </summary>
    static member fromInput (element: JsonElement) =
        element
        |> tryGetProperty (nameof ClientId)
        |> Result.map (fun el -> ClientId.fromString(el.GetString()))

    /// <summary>
    /// Generates an instance of this type
    /// from a <see cref="string" />.
    /// </summary>
    static member fromIdentifier i = ClientId i

    /// <summary>
    /// Generates an instance of this type
    /// from a <see cref="string" />.
    /// </summary>
    static member fromString s = ClientId (Identifier.fromString(s))

    /// <summary>
    /// Converts this instance
    /// to <see cref="Identifier" />.
    /// </summary>
    member this.toIdentifier = let (ClientId v) = this in v

/// <summary>
/// Defines the display text of an item.
/// </summary>
type DisplayText =
    | DisplayText of string

    /// <summary>
    /// Reduces this instance
    /// to <c>Some</c> <see cref="string" /> value.
    /// </summary>
    member this.Value = let (DisplayText v) = this in v

/// <summary>
/// Defines the Temporal end date of an item.
/// </summary>
type EndDate =
    | EndDate of DateTime

    /// <summary>
    /// Generates an instance of this type
    /// from conventional JSON input.
    /// </summary>
    static member fromInput (element: JsonElement) =
        element
        |> tryGetProperty (nameof EndDate)
        |> Result.map ( fun el -> EndDate (el.GetDateTime()))


/// <summary>
/// Defines the Temporal incept date of an item.
/// </summary>
type InceptDate =
    | InceptDate of DateTime

    /// <summary>
    /// Generates an instance of this type
    /// from conventional JSON input.
    /// </summary>
    static member fromInput (element: JsonElement) =
        element
        |> tryGetProperty (nameof InceptDate)
        |> Result.map ( fun el -> InceptDate (el.GetDateTime()))

/// <summary>
/// Defines the name of an item.
/// </summary>
type ItemName =
    | ItemName of string

    /// <summary>
    /// Reduces this instance
    /// to <c>Some</c> <see cref="string" /> value.
    /// </summary>
    member this.Value = let (ItemName v) = this in v

/// <summary>
/// Defines the Temporal modification date of an item.
/// </summary>
type ModificationDate =
    | ModificationDate of DateTime

    /// <summary>
    /// Generates an instance of this type
    /// from conventional JSON input.
    /// </summary>
    static member fromInput (element: JsonElement) =
        element
        |> tryGetProperty (nameof ModificationDate)
        |> Result.map (fun el -> ModificationDate (el.GetDateTime()))
