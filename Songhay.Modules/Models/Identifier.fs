namespace Songhay.Modules.Models

open System

/// <summary>
///  Defines the conventional identifier or sort ordinal.
/// </summary>
type Identifier = Numeric of int | Alphanumeric of string

module Identifier =
    let toIdentifierInt32 (input: Identifier) =
        match input with
        | Alphanumeric a ->
            match Int32.TryParse(a) with
            | true, n -> n
            | false, _ -> 0
        | Numeric n -> n

    let toIdentifierString (input: Identifier) =
        match input with
        | Alphanumeric a -> a
        | Numeric n -> $"{n}"
