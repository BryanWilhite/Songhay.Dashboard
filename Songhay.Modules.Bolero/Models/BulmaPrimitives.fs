namespace Songhay.Modules.Bolero.Models

open System

open Songhay.Modules.StringUtility

///<summary>
/// Defines Bulma responsive breakpoints
///</summary>
/// <remarks>
/// 📖 https://bulma.io/documentation/helpers/visibility-helpers/
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

    ///<summary>Returns the <see cref="string" /> representation of the breakpoint name.</summary>
    member this.Value = this.ToString().ToLowerInvariant()

///<summary>
/// Defines the seven Bulma font sizes in <c>rem</c>.
///</summary>
/// <remarks>
/// 📖 https://bulma.io/documentation/helpers/typography-helpers/
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

    ///<summary>Returns the <see cref="string" /> representation of the Bulma font size.</summary>
    member this.Value = this.ToString() |> toNumericString |> Option.get

///<summary>
/// Defines a rule for matching <see cref="BulmaFontSize" /> or a default/inherited size.
///</summary>
type BulmaFontSizeOrDefault =
    /// <summary> match a default/inherited size </summary>
    | DefaultBulmaFontSize
    /// <summary> match <see cref="BulmaFontSize" /> </summary>
    | HasFontSize of BulmaFontSize

    ///<summary>Returns the <see cref="string" /> representation of the Bulma font size or <see cref="String.Empty" />.</summary>
    member this.Value =
        match this with
        | DefaultBulmaFontSize -> String.Empty
        | HasFontSize size -> size.Value

///<summary>
/// Defines all Bulma square dimensions.
///</summary>
/// <remarks>
/// 📖 https://bulma.io/documentation/elements/image/#fixed-square-images
/// </remarks>
type BulmaSquareDimension =
    | Square16
    | Square24
    | Square32
    | Square48
    | Square64
    | Square96
    | Square128

    ///<summary>Returns the CSS class name of the Bulma square dimension.</summary>
    member this.CssClass = this.ToString() |> toNumericString |> Option.get |> fun n -> $"is-{n}x{n}"

    ///<summary>Returns the integer representation of the Bulma square dimension.</summary>
    member this.ToWidthOrHeight = this.ToString() |> toNumericString |> Option.get |> Int32.Parse

///<summary>
/// Defines all Bulma ratios of dimensions for Bulma responsive images.
///</summary>
/// <remarks>
/// 📖 https://bulma.io/documentation/elements/image/#responsive-images-with-ratios
/// </remarks>
type BulmaRatioDimension =
    | Square of BulmaSquareDimension
    /// <summary> a Bulma ratio of dimensions for a Bulma responsive image</summary>
    | FiveByFour
    /// <summary> a Bulma ratio of dimensions for a Bulma responsive image</summary>
    | FourByThree
    /// <summary> a Bulma ratio of dimensions for a Bulma responsive image</summary>
    | ThreeByTwo
    /// <summary> a Bulma ratio of dimensions for a Bulma responsive image</summary>
    | FiveByThree
    /// <summary> a Bulma ratio of dimensions for a Bulma responsive image</summary>
    | SixteenByNine
    /// <summary> a Bulma ratio of dimensions for a Bulma responsive image</summary>
    | TwoByOne
    /// <summary> a Bulma ratio of dimensions for a Bulma responsive image</summary>
    | ThreeByOne
    /// <summary> a Bulma ratio of dimensions for a Bulma responsive image</summary>
    | FourByFive
    /// <summary> a Bulma ratio of dimensions for a Bulma responsive image</summary>
    | ThreeByFour
    /// <summary> a Bulma ratio of dimensions for a Bulma responsive image</summary>
    | TwoByThree
    /// <summary> a Bulma ratio of dimensions for a Bulma responsive image</summary>
    | ThreeByFive
    /// <summary> a Bulma ratio of dimensions for a Bulma responsive image</summary>
    | NineBySixteen
    /// <summary> a Bulma ratio of dimensions for a Bulma responsive image</summary>
    | OneByTwo
    /// <summary> a Bulma ratio of dimensions for a Bulma responsive image</summary>
    | OneByThree

    ///<summary>Returns the CSS class name of the Bulma ratio of dimensions.</summary>
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

///<summary>
/// Defines all Bulma value suffixes for Bulma <c>property-direction</c> combinations.
///</summary>
/// <remarks>
/// 📖 https://bulma.io/documentation/helpers/spacing-helpers/
/// </remarks>
type BulmaValueSuffix =
    /// <summary> a Bulma value suffix </summary>
    | L0
    /// <summary> a Bulma value suffix </summary>
    | L1
    /// <summary> a Bulma value suffix </summary>
    | L2
    /// <summary> a Bulma value suffix </summary>
    | L3
    /// <summary> a Bulma value suffix </summary>
    | L4
    /// <summary> a Bulma value suffix </summary>
    | L5
    /// <summary> a Bulma value suffix </summary>
    | L6
    /// <summary> a Bulma value suffix </summary>
    | AutoSuffix

    ///<summary>Returns the <see cref="string" /> representation of the Bulma value suffix.</summary>
    member this.Value =
        match this with
        | AutoSuffix -> "auto"
        | _ -> this.ToString() |> toNumericString |> Option.get

///<summary>
/// Defines the value of a Bulma spacing helper.
///</summary>
/// <remarks>
/// 📖 https://bulma.io/documentation/helpers/spacing-helpers/
/// </remarks>
type BulmaSpacing =
    /// <summary> a Bulma spacing helper value </summary>
    | BulmaSpacing of CssBoxModel * BulmaValueSuffix

    /// <summary> unwraps the underlying <c>CssBoxModel * BulmaValueSuffix</c> </summary>
    member this.Value = match this with | BulmaSpacing (b, s) -> b, s

///<summary>
/// Defines the 12 Bulma horizontal-space sizes for tiles.
///</summary>
/// <remarks>
/// 📖 https://bulma.io/documentation/layout/tiles/
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

    ///<summary>Returns the CSS class name of the Bulma horizontal-space size.</summary>
    member this.CssClass =
        match this with
        | TileSizeAuto -> String.Empty 
        | _ -> this.ToString() |> toNumericString |> Option.get |> fun s -> $"is-{s}"
