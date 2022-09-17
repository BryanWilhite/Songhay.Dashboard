module Songhay.Modules.Bolero.Tests.Visuals.CssDeclarationTests

open Xunit
open Songhay.Modules.Bolero.Models
open Songhay.Modules.Bolero.Visuals.CssDeclaration

[<Literal>]
let FontVariantExpected01 = "font-variant: small-caps slashed-zero;"

[<Literal>]
let FontVariantExpected02 = "font-variant: common-ligatures tabular-nums;"

[<Literal>]
let FontVariantExpected03 = "font-variant: no-common-ligatures proportional-nums;"

let fontVariantDictionary =
    dict [
        FontVariantExpected01, [ SmallCaps; Numeric FontVariantNumericSlashedZero ];
        FontVariantExpected02, [ Ligatures FontVariantLigaturesCommon; Numeric FontVariantTabular ];
        FontVariantExpected03, [ Ligatures FontVariantLigaturesNoCommon; Numeric FontVariantProportional ];
    ]

[<Theory>]
[<InlineData(FontVariantExpected01)>]
[<InlineData(FontVariantExpected02)>]
[<InlineData(FontVariantExpected03)>]
let ``fontVariant test`` (expected: string) =
    let actual = fontVariant fontVariantDictionary[expected]
    Assert.Equal(expected, actual)
