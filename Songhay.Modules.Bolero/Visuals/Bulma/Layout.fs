namespace Songhay.Modules.Bolero.Visuals.Bulma

open Bolero
open Bolero.Html

open Songhay.Modules.Bolero.Models

///<summary>
/// Bulma Layout
/// “Design the structure of your webpage…”
/// 📖 https://bulma.io/documentation/layout/
///</summary>
module Layout =

    ///<summary>
    /// Returns a <c>nav</c> element of CSS class <see cref="CssClass.levelContainer" />.
    ///</summary>
    ///<remarks>
    /// “A multi-purpose horizontal level, which can contain almost any other element…”
    /// 📖 https://bulma.io/documentation/layout/level/
    ///</remarks>
    let bulmaLevel (moreClasses: CssClassesOrEmpty) (levelChildNodes: Node list) =
        nav {
            CssClasses [ CssClass.levelContainer ] |> moreClasses.ToHtmlClassAttribute

            forEach levelChildNodes <| id
        }

    ///<summary>
    /// Returns a <c>nav</c> element of CSS class <see cref="CssClass.level" />
    /// from the specified <see cref="CssBoxAlignment" />.
    ///</summary>
    ///<remarks>
    /// “A multi-purpose horizontal level, which can contain almost any other element…”
    /// 📖 https://bulma.io/documentation/layout/level/
    ///</remarks>
    let bulmaLevelChildAligned (alignment: CssBoxAlignment) (moreClasses: CssClassesOrEmpty) (levelChildNodes: Node list) =
        div {
            CssClasses [ (alignment |> CssClass.level) ] |> moreClasses.ToHtmlClassAttribute

            forEach levelChildNodes <| id
        }

    ///<summary>
    /// Returns a <c>nav</c> element of CSS class <see cref="CssClass.levelItem" />.
    ///</summary>
    ///<remarks>
    /// “In a <c>level-item</c>, you can then insert almost anything you want…
    /// No matter what elements you put inside a Bulma <c>level<c/>, they will always be vertically centered.”
    /// 📖 https://bulma.io/documentation/layout/level/
    ///</remarks>
    let bulmaLevelItem (moreClasses: CssClassesOrEmpty) (attributes: HtmlAttributesOrEmpty) (childNodes: Node list) =
        div {
            CssClasses [ CssClass.levelItem ] |> moreClasses.ToHtmlClassAttribute
            attributes.Value

            forEach childNodes <| id
        }

    ///<summary>
    /// Returns a loader container for the specified loader <see cref="Node" />,
    /// provided by <see cref="bulmaLoader" />.
    ///</summary>
    let bulmaLoaderContainer (moreClasses: CssClassesOrEmpty) (loaderNode: Node) =
        div {
            CssClasses [ "loader-container"; CssClass.elementTextAlign Center ] |> moreClasses.ToHtmlClassAttribute

            loaderNode
        }

    ///<summary>
    /// Returns a loader element for <see cref="bulmaLoaderContainer" />.
    ///</summary>
    let bulmaLoader (moreClasses: CssClassesOrEmpty) =
        div {
            CssClasses [ "loader" ] |> moreClasses.ToHtmlClassAttribute

            attr.title "Loading…"
        }

    ///<summary>
    /// Returns a container of CSS class <see cref="CssClass.media" />,
    /// declaring a child container of CSS class <see cref="CssClass.mediaContent" />,
    /// wrapping the specified media content nodes.
    ///</summary>
    ///<remarks>
    /// “The famous media object prevalent in social media interfaces, but useful in any context…”
    /// 📖 https://bulma.io/documentation/layout/media-object/
    ///</remarks>
    let bulmaMedia (moreClasses: CssClassesOrEmpty) (mediaLeft: HtmlNodeOrEmpty) (mediaContentNodes: Node list) =
        let mediaContainerClasses = CssClasses [ CssClass.media ]

        div {
            mediaContainerClasses |> moreClasses.ToHtmlClassAttribute

            mediaLeft.Value

            div {
                CssClass.mediaContent |> CssClasses.toHtmlClass

                forEach mediaContentNodes <| id
            }
        }

    ///<summary>
    /// Returns a container of CSS class <see cref="CssClass.mediaLeft" />,
    /// a child of the container returned by <see cref="bulmaMedia" />.
    ///</summary>
    ///<remarks>
    /// “The famous media object prevalent in social media interfaces, but useful in any context…”
    /// 📖 https://bulma.io/documentation/layout/media-object/
    ///</remarks>
    let bulmaMediaLeft (moreClasses: CssClassesOrEmpty) (attribute: HtmlAttributeOrEmpty) (childNode: Node) =
        figure {
            CssClasses [ CssClass.mediaLeft ] |> moreClasses.ToHtmlClassAttribute
            attribute.Value

            childNode
        }

    ///<summary>
    /// Returns a container of CSS class <see cref="CssClass.tile" />,
    /// wrapping the specified tile content nodes.
    ///</summary>
    ///<remarks>
    /// “The famous media object prevalent in social media interfaces, but useful in any context…”
    /// 📖 https://bulma.io/documentation/layout/tiles/
    ///</remarks>
    let bulmaTile (width: BulmaTileHorizontalSize) (moreClasses: CssClassesOrEmpty) (tileContentNodes: Node list) =
        let tileContainerClasses = CssClasses [
            CssClass.tile
            match width with | TileSizeAuto -> () | _ -> width.CssClass
        ]

        div {
            tileContainerClasses |> moreClasses.ToHtmlClassAttribute

            forEach tileContentNodes <| id
        }
