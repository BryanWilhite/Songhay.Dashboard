namespace Songhay.Modules.Bolero.Models

open Bolero.Html
open Songhay.Modules.Bolero.BoleroUtility

///<remarks>
/// https://developer.mozilla.org/en-US/docs/Web/CSS/CSS_Box_Alignment
///</remarks>
type CssAlignment =
    // text-align
    | Start
    | End
    | Left
    | Right
    | Center
    | Justify
    | JustifyAll
    | MatchParent
    // vertical-align
    | BaseLine
    | Top
    | Middle
    | Bottom
    | Sub
    | TextTop
    // justify-content
    | SpaceBetween
    | SpaceAround
    | SpaceEvenly
    // align-items (Flexbox, Grid Layout)
    | Normal
    | Stretch
    | FlexStart
    | FlexEnd
    | SelfStart
    | SelfEnd

    member this.Value =
        match this with
        // text-align
        | Start -> "start"
        | End -> "end"
        | Left -> "left"
        | Right -> "right"
        | Center -> "center"
        | Justify -> "justify"
        | JustifyAll -> "justify-all"
        | MatchParent -> "match-parent"
        // vertical-align
        | BaseLine -> "baseline"
        | Top -> "top"
        | Middle -> "middle"
        | Bottom -> "bottom"
        | Sub -> "sub"
        | TextTop -> "text-top"
        // justify-content
        | SpaceBetween -> "space-between"
        | SpaceAround -> "space-around"
        | SpaceEvenly -> "space-evenly"
        // align-items (Flexbox, Grid Layout)
        | Normal -> "normal"
        | Stretch -> "stretch"
        | FlexStart -> "flex-start"
        | FlexEnd -> "flex-end"
        | SelfStart -> "self-start"
        | SelfEnd -> "self-end"

///<summary>
/// Enumerates the CSS Box Model
/// </summary>
/// <remarks>
/// see https://developer.mozilla.org/en-US/docs/Learn/CSS/Building_blocks/The_box_model#parts_of_a_box
/// </remarks>
type CssBoxModel =
    ///<summary>all margins, paddings or borders</summary>
    | All
    ///<summary>left margin, padding or border</summary>
    | L
    ///<summary>right margin, padding or border</summary>
    | R
    ///<summary>left and right margin, padding or border</summary>
    | LR
    ///<summary>top margin, padding or border</summary>
    | T
    ///<summary>bottom margin, padding or border</summary>
    | B
    ///<summary>top and bottom margin, padding or border</summary>
    | TB

    member this.Value =
        match this with
        | All -> System.String.Empty
        | L -> "l"
        | R -> "r"
        | LR -> "x"
        | T -> "t"
        | B -> "b"
        | TB -> "y"

type CssClassesOrEmpty =
    | NoCssClasses
    | HasClasses of CssClasses

    member this.ToHtmlClassAttribute (cssClasses: CssClasses) =
        match this with
        | HasClasses moreCssClasses -> moreCssClasses.Value |> cssClasses.AppendList |> toHtmlClassFromData
        | NoCssClasses -> cssClasses.ToHtmlClassAttribute

    member this.Value = match this with | NoCssClasses -> attr.empty() | HasClasses classes -> classes.ToHtmlClassAttribute

type CssCommonImageAspectRatioNumber =
    | One
    | Two
    | Three
    | Four
    | Five
    | Nine
    | Sixteen

    member this.Value =
        match this with
        | One -> "1"
        | Two -> "2"
        | Three -> "3"
        | Four -> "4"
        | Five -> "5"
        | Nine -> "9"
        | Sixteen -> "16"

type CssFontFamily =
    | SansSerif
    | Monospace
    | Primary
    | Secondary
    | Emoji
    | Math

    member this.Value =
        match this with
        | SansSerif -> "sans-serif"
        | Monospace -> "monospace"
        | Primary -> "primary"
        | Secondary -> "secondary"
        | Emoji -> "emoji"
        | Math -> "math"

type CssFontWeight =
    | Light
    | Normal
    | Medium
    | Semibold
    | Bold

    member this.Value =
        match this with
        | Light -> "light"
        | Normal -> "normal"
        | Medium -> "medium"
        | Semibold -> "semibold"
        | Bold -> "bold"

type CssMargin =
    | CssMargin of CssBoxModel
    member this.Value = match this with | CssMargin m -> m

type CssPadding =
    | CssPadding of CssBoxModel
    member this.Value = match this with | CssPadding m -> m

type CssTextTransformation =
    | Lowercase
    | TitleCase
    | UpperCase
    | Italic
    | Underline

    member this.Value =
        match this with
        | Lowercase -> "lowercase"
        | TitleCase -> "title-case"
        | UpperCase -> "uppercase"
        | Italic -> "italic"
        | Underline -> "underline"
