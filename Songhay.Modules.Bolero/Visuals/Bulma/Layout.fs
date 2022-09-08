namespace Songhay.Modules.Bolero.Visuals.Bulma

open Bolero
open Bolero.Html

open Songhay.Modules.Bolero.BoleroUtility
open Songhay.Modules.Bolero.Models

///<summary>
/// Bulma Layout
/// “Design the structure of your webpage…”
/// — https://bulma.io/documentation/layout/
///</summary>
module Layout =
    ///<summary>
    /// Bulma CSS layout function for <see cref="CssClass.levelContainer" />.
    ///</summary>
    ///<remarks>
    /// “A multi-purpose horizontal level, which can contain almost any other element…”
    /// — https://bulma.io/documentation/layout/level/
    ///</remarks>
    let bulmaLevel (moreClasses: CssClassesOrEmpty) (levelChildNodes: Node list) =
        nav {
            CssClasses [ CssClass.levelContainer ] |> moreClasses.ToHtmlClassAttribute

            forEach levelChildNodes <| id
        }

    ///<summary>
    /// Bulma CSS layout function for <see cref="CssClass.level" />.
    ///</summary>
    ///<remarks>
    /// “A multi-purpose horizontal level, which can contain almost any other element…”
    /// — https://bulma.io/documentation/layout/level/
    ///</remarks>
    let bulmaLevelChildAligned (alignment: CssAlignment) (moreClasses: CssClassesOrEmpty) (levelChildNodes: Node list) =
        div {
            CssClasses [ (alignment |> CssClass.level) ] |> moreClasses.ToHtmlClassAttribute

            forEach levelChildNodes <| id
        }

    ///<summary>
    /// Bulma CSS layout function for <see cref="CssClass.level" />.
    ///</summary>
    ///<remarks>
    /// “In a <c>level-item</c>, you can then insert almost anything you want…
    /// No matter what elements you put inside a Bulma <c>level<c/>, they will always be vertically centered.”
    /// — https://bulma.io/documentation/layout/level/
    ///</remarks>
    let bulmaLevelItem (moreClasses: CssClassesOrEmpty) (childNode: Node) =
        div {
            CssClasses [ CssClass.levelItem ] |> moreClasses.ToHtmlClassAttribute

            childNode
        }

    let bulmaLoader (margin: CssMargin * BulmaValueSuffix) (padding: CssPadding * BulmaValueSuffix) =

        let marginClass = match margin with | CssMargin b, s -> CssClass.m (b, s) 
        let paddingClass = match padding with | CssPadding b, s -> CssClass.m (b, s) 

        div {
            [ "loader-container"; paddingClass; CssClass.elementTextAlign Center] |> toHtmlClassFromList

            div {
                [ "loader"; marginClass ] @ CssClass.imageContainer (Square Square128) |> toHtmlClassFromList

                attr.title "Loading…"
            }
        }

    let bulmaMedia (moreClasses: CssClassesOrEmpty) (mediaLeft: HtmlNodeOrEmpty) (mediaContentNodes: Node list) =
        let mediaContainerClasses = CssClasses [ CssClass.media ]

        div {
            mediaContainerClasses |> moreClasses.ToHtmlClassAttribute

            mediaLeft.Value

            div {
                CssClass.mediaContent |> toHtmlClass

                forEach mediaContentNodes <| id
            }
        }

    let bulmaTile (width: BulmaTileHorizontalSize) (moreClasses: CssClassesOrEmpty) (tileContentNodes: Node list) =
        let tileContainerClasses = CssClasses [
            CssClass.tile
            match width with | TileSizeAuto -> () | _ -> width.CssClass
        ]

        div {
            tileContainerClasses |> moreClasses.ToHtmlClassAttribute

            forEach tileContentNodes <| id
        }
