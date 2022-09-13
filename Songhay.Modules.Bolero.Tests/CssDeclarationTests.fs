module CssDeclarationTests

open Xunit

open FsUnit.Xunit
open FsUnit.CustomMatchers
open FsToolkit.ErrorHandling

open Songhay.Modules.Bolero.Models
open Songhay.Modules.Bolero.Visuals.CssDeclaration

[<Literal>]
let FontVariantExpected01 = "font-variant: small-caps slashed-zero;"

let fontVariantDictionary =
    dict [
        FontVariantExpected01, [ SmallCaps; Numeric FontVariantNumericSlashedZero ];
    ]

[<Theory>]
[<InlineData(FontVariantExpected01)>]
let ``fontVariant test`` (expected: string) =
    let actual = fontVariant fontVariantDictionary[expected]
    Assert.Equal(expected, actual)
