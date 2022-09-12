namespace Songhay.Modules.Bolero.Visuals

open Songhay.Modules.Bolero.Models

module CssDeclarations =
    let fontVariantAlternates (variant: CssFontVariantAlternates) = $"font-variant-alternates: {variant.Value}"
    let fontVariantCaps (variant: CssFontVariantCaps) = $"font-variant-caps: {variant.Value}"
    let fontVariantLigatures (variant: CssFontVariantLigatures) = $"font-variant-ligatures: {variant.Value}"
    let fontVariantNumeric (variant: CssFontVariantNumeric) = $"font-variant-numeric: {variant.Value}"
