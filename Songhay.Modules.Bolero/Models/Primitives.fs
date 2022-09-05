namespace Songhay.Modules.Bolero.Models

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

type CssBox =
    | All
    | L
    | R
    | LR
    | T
    | B
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
