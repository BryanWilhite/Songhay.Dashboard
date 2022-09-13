namespace Songhay.Modules.Bolero.Visuals

open System
open Songhay.Modules.Bolero.Models

module CssDeclaration =
    let fontVariantAlternates (variant: CssFontVariantAlternates) = $"font-variant-alternates: {variant.Value};"
    let fontVariantCaps (variant: CssFontVariantCaps) = $"font-variant-caps: {variant.Value};"
    let fontVariantLigatures (variant: CssFontVariantLigatures) = $"font-variant-ligatures: {variant.Value};"
    let fontVariantNumeric (variant: CssFontVariantNumeric) = $"font-variant-numeric: {variant.Value};"
    let fontVariant (variants: CssFontVariant list) =
        match variants with
        | [ v ] -> $"font-variant: {v.Value};"
        | _ ->
            let s = (String.Empty, variants)
                    ||> List.fold (fun a i -> if String.IsNullOrWhiteSpace a then i.Value else $"{a} {i.Value}")
            $"font-variant: {s};"
