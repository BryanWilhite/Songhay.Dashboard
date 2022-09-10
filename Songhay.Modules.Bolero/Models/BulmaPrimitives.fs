namespace Songhay.Modules.Bolero.Models

open System

///<summary>
/// Defines Bulma responsive breakpoints
///</summary>
/// <remarks>
/// see: https://bulma.io/documentation/helpers/visibility-helpers/
/// </remarks>
type BulmaBreakpoint =
    ///<summary>up to 768px</summary>
    | Mobile
    ///<summary>between 769px and 1023px</summary>
    | Tablet
    ///<summary>up to 1023px</summary>
    | Touch
    ///<summary>between 1024px and 1215px</summary>
    | Desktop
    ///<summary>between 1216px and 1407px</summary>
    | WideScreen
    ///<summary>1408px and above</summary>
    | FullHD

    member this.Value =
        match this with
        | Mobile -> "mobile"
        | Tablet -> "tablet"
        | Touch -> "touch"
        | Desktop -> "desktop"
        | WideScreen -> "widescreen"
        | FullHD -> "fullhd"

///<summary>
/// Defines the seven Bulma font sizes in <c>rem</c>.
///</summary>
/// <remarks>
/// see: https://bulma.io/documentation/helpers/typography-helpers/
/// </remarks>
type BulmaFontSize =
    ///<summary>3rem</summary>
    | Size1
    ///<summary>2.5rem</summary>
    | Size2
    ///<summary>2rem</summary>
    | Size3
    ///<summary>1.5rem</summary>
    | Size4
    ///<summary>1.25rem</summary>
    | Size5
    ///<summary>1rem</summary>
    | Size6
    ///<summary>0.75rem</summary>
    | Size7

    member this.Value =
        match this with
        | Size1 -> "1"
        | Size2 -> "2"
        | Size3 -> "3"
        | Size4 -> "4"
        | Size5 -> "5"
        | Size6 -> "6"
        | Size7 -> "7"

type BulmaFontSizeOrDefault =
    | DefaultBulmaFontSize
    | HasFontSize of BulmaFontSize

    member this.Value =
        match this with
        | DefaultBulmaFontSize -> String.Empty
        | HasFontSize size -> size.Value

type BulmaSquareDimension =
    | Square16
    | Square24
    | Square32
    | Square48
    | Square64
    | Square96
    | Square128

    member this.CssClass =
        match this with
        | Square16 -> "is-16x16"
        | Square24 -> "is-24x24"
        | Square32 -> "is-32x32"
        | Square48 -> "is-48x48"
        | Square64 -> "is-64x64"
        | Square96 -> "is-96x96"
        | Square128 -> "is-128x128"

    member this.ToWidthOrHeight =
        match this with
        | Square16 -> 16
        | Square24 -> 24
        | Square32 -> 32
        | Square48 -> 48
        | Square64 -> 64
        | Square96 -> 96
        | Square128 -> 128

type BulmaRatioDimension =
    | Square of BulmaSquareDimension
    | FiveByFour
    | FourByThree
    | ThreeByTwo
    | FiveByThree
    | SixteenByNine
    | TwoByOne
    | ThreeByOne
    | FourByFive
    | ThreeByFour
    | TwoByThree
    | ThreeByFive
    | NineBySixteen
    | OneByTwo
    | OneByThree

    member this.CssClass =
        match this with
        | Square value -> value.CssClass
        | FiveByFour -> $"is-{Five.Value}by{Four.Value}"
        | FourByThree -> $"is-{Four.Value}by{Three.Value}"
        | ThreeByTwo -> $"is-{Three.Value}by{Two.Value}"
        | FiveByThree -> $"is-{Five.Value}by{Three.Value}"
        | SixteenByNine -> $"is-{Sixteen.Value}by{Nine.Value}"
        | TwoByOne -> $"is-{Two.Value}by{One.Value}"
        | ThreeByOne -> $"is-{Three.Value}by{One.Value}"
        | FourByFive -> $"is-{Four.Value}by{Five.Value}"
        | ThreeByFour -> $"is-{Three.Value}by{Four.Value}"
        | TwoByThree -> $"is-{Two.Value}by{Three.Value}"
        | ThreeByFive -> $"is-{Three.Value}by{Five.Value}"
        | NineBySixteen -> $"is-{Nine.Value}by{Sixteen.Value}"
        | OneByTwo -> $"is-{One.Value}by{Two.Value}"
        | OneByThree -> $"is-{One.Value}by{Three.Value}"

type BulmaValueSuffix =
    | L0
    | L1
    | L2
    | L3
    | L4
    | L5
    | L6
    | Auto

    member this.Value =
        match this with
        | L0 -> "0"
        | L1 -> "1"
        | L2 -> "2"
        | L3 -> "3"
        | L4 -> "4"
        | L5 -> "5"
        | L6 -> "6"
        | Auto -> "auto"

type BulmaSpacing =
    | BulmaSpacing of CssBoxModel * BulmaValueSuffix

    member this.Value = match this with | BulmaSpacing (b, s) -> b, s

///<summary>
/// Defines the 12 Bulma horizontal-space sizes for tiles.
///</summary>
/// <remarks>
/// see: https://bulma.io/documentation/layout/tiles/
/// </remarks>
type BulmaTileHorizontalSize =
    ///<summary>the available horizontal space</summary>
    | TileSizeAuto
    ///<summary>1/12 of the horizontal space</summary>
    | TileSize1
    ///<summary>2/12 of the horizontal space</summary>
    | TileSize2
    ///<summary>3/12 of the horizontal space</summary>
    | TileSize3
    ///<summary>4/12 of the horizontal space</summary>
    | TileSize4
    ///<summary>5/12 of the horizontal space</summary>
    | TileSize5
    ///<summary>6/12 of the horizontal space</summary>
    | TileSize6
    ///<summary>7/12 of the horizontal space</summary>
    | TileSize7
    ///<summary>8/12 of the horizontal space</summary>
    | TileSize8
    ///<summary>9/12 of the horizontal space</summary>
    | TileSize9
    ///<summary>10/12 of the horizontal space</summary>
    | TileSize10
    ///<summary>11/12 of the horizontal space</summary>
    | TileSize11
    ///<summary>12/12 of the horizontal space</summary>
    | TileSize12

    member this.CssClass =
        match this with
        | TileSizeAuto -> String.Empty 
        | TileSize1 -> "is-1"
        | TileSize2 -> "is-2"
        | TileSize3 -> "is-3"
        | TileSize4 -> "is-4"
        | TileSize5 -> "is-5"
        | TileSize6 -> "is-6"
        | TileSize7 -> "is-7"
        | TileSize8 -> "is-8"
        | TileSize9 -> "is-9"
        | TileSize10 -> "is-10"
        | TileSize11 -> "is-11"
        | TileSize12 -> "is-12"
