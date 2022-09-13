namespace Songhay.Modules.Bolero.Visuals

open System
open Songhay.Modules.Bolero.Models

module CssDeclarations =
    let fontVariantAlternates (variant: CssFontVariantAlternates) = $"font-variant-alternates: {variant.Value};"
    let fontVariantCaps (variant: CssFontVariantCaps) = $"font-variant-caps: {variant.Value};"
    let fontVariantLigatures (variant: CssFontVariantLigatures) = $"font-variant-ligatures: {variant.Value};"
    let fontVariantNumeric (variant: CssFontVariantNumeric) = $"font-variant-numeric: {variant.Value};"
    let fontVariant (variants: CssFontVariant list) =
        match variants with
        | [ v ] -> $"font-variant: {v.Value};"
        | _ ->
            let s = (String.Empty, variants)
                    ||> List.fold (fun s v -> if String.IsNullOrWhiteSpace s then v.Value else $" {v.Value}")
            $"font-variant: {s};"
