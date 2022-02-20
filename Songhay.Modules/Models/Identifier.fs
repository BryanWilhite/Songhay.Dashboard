namespace Songhay.Modules.Models

open System

/// <summary>
///  Defines the conventional identifier or sort ordinal.
/// </summary>
type Identifier =
    | Alphanumeric of string
    | Numeric of int

    member this.Int32Value =
        match this with
        | Alphanumeric a ->
            match Int32.TryParse(a) with
            | true, n -> Some n
            | false, _ -> None
        | Numeric n -> Some n

    member this.StringValue =
        match this with
        | Alphanumeric a -> a
        | Numeric n -> $"{n}"

