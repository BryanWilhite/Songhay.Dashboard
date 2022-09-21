module Songhay.Modules.Bolero.Tests.Models.BulmaPrimitivesTests

open System

open Xunit

open Songhay.Modules.Bolero.Models

let BulmaFontSizeOrDefaultTestData : seq<obj[]> =
    seq {
        yield [| String.Empty; DefaultBulmaFontSize |]
        yield [| "1"; HasFontSize Size1 |]
    }

[<Theory>]
[<MemberData(nameof BulmaFontSizeOrDefaultTestData)>]
let ``BulmaFontSizeOrDefault.Value test`` (expected: string, input: BulmaFontSizeOrDefault) =
    let actual = input.Value
    Assert.Equal(expected, actual)

let BulmaRatioDimensionTestData : seq<obj[]> =
    seq {
        yield [| "is-128x128"; Square Square128 |]
        yield [| "is-16by9"; SixteenByNine |]
    }

[<Theory>]
[<MemberData(nameof BulmaRatioDimensionTestData)>]
let ``BulmaRatioDimension.CssClass test`` (expected: string, input: BulmaRatioDimension) =
    let actual = input.CssClass
    Assert.Equal(expected, actual)

let BulmaValueSuffixTestData : seq<obj[]> =
    seq {
        yield [| "auto"; AutoSuffix |]
        yield [| "3"; L3 |]
    }

[<Theory>]
[<MemberData(nameof BulmaValueSuffixTestData)>]
let ``BulmaValueSuffix.Value test`` (expected: string, input: BulmaValueSuffix) =
    let actual = input.Value
    Assert.Equal(expected, actual)

let BulmaTileHorizontalSizeTestData : seq<obj[]> =
    seq {
        yield [| String.Empty; TileSizeAuto |]
        yield [| "is-7"; TileSize7 |]
    }

[<Theory>]
[<MemberData(nameof BulmaTileHorizontalSizeTestData)>]
let ``BulmaTileHorizontalSize.CssClass test`` (expected: string, input: BulmaTileHorizontalSize) =
    let actual = input.CssClass
    Assert.Equal(expected, actual)

let BulmaVisibilityTestData : seq<obj[]> =
    seq {
        yield [| "is-inline"; DisplayInline |]
        yield [| "is-inline-block"; DisplayInlineBlock |]
        yield [| "is-invisible"; NonDisplayInvisible |]
        yield [| "is-sr-only"; ScreenReaderOnly |]
    }

[<Theory>]
[<MemberData(nameof BulmaVisibilityTestData)>]
let ``BulmaVisibility.CssClass test`` (expected: string, input: BulmaVisibility) =
    let actual = input.CssClass
    Assert.Equal(expected, actual)
