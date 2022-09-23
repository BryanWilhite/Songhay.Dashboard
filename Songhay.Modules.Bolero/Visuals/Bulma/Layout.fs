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
    /// Returns a <c>div</c> element of CSS class <see cref="CssClass.container" />.
    ///</summary>
    let bulmaContainer (width: BulmaContainerWidth) (moreClasses: CssClassesOrEmpty) (childNode: Node) =
        div {
            CssClasses [ CssClass.container width ] |> moreClasses.ToHtmlClassAttribute

            childNode
        }

    ///<summary>
    /// Returns a <c>section</c> element of CSS class <c>hero</c>.
    ///</summary>
    ///<remarks>
    /// “The hero component allows you to add a full width banner to your webpage,
    /// which can optionally cover the full height of the page as well…”
    /// 📖 https://bulma.io/documentation/layout/hero/
    ///</remarks>
    let bulmaHero
        (moreClasses: CssClassesOrEmpty)
        (headerNode: HtmlNodeOrEmpty)
        (bodyNode: Node)
        (footerNode: HtmlNodeOrEmpty) =
        div {
            CssClasses [ "hero" ] |> moreClasses.ToHtmlClassAttribute

            headerNode.Value

            bodyNode

            footerNode.Value
        }

    ///<summary>
    /// Returns an element of CSS class <c>hero-head</c>.
    ///</summary>
    ///<remarks>
    /// <see cref="bulmaHero" />
    ///</remarks>
    let bulmaHeroHeader
        (moreClasses: CssClassesOrEmpty)
        (childNode: Node) =
        div {
            CssClasses [ "hero-head" ] |> moreClasses.ToHtmlClassAttribute

            childNode
        }

    ///<summary>
    /// Returns an element of CSS class <c>hero-body</c>.
    ///</summary>
    ///<remarks>
    /// <see cref="bulmaHero" />
    ///</remarks>
    let bulmaHeroBody
        (moreClasses: CssClassesOrEmpty)
        (childNode: Node) =
        div {
            CssClasses [ "hero-body" ] |> moreClasses.ToHtmlClassAttribute

            childNode
        }

    ///<summary>
    /// Returns an element of CSS class <c>hero-foot</c>.
    ///</summary>
    ///<remarks>
    /// <see cref="bulmaHero" />
    ///</remarks>
    let bulmaHeroFoot
        (moreClasses: CssClassesOrEmpty)
        (childNode: Node) =
        div {
            CssClasses [ "hero-foot" ] |> moreClasses.ToHtmlClassAttribute

            childNode
        }

    ///<summary>
    /// Returns a <c>nav</c> element of CSS class <see cref="CssClass.levelContainer" />.
    ///</summary>
    ///<remarks>
    /// “A multi-purpose horizontal level, which can contain almost any other element…”
    /// 📖 https://bulma.io/documentation/layout/level/
    ///</remarks>
    let bulmaLevel (moreClasses: CssClassesOrEmpty) (levelChildNode: Node) =
        nav {
            CssClasses [ CssClass.levelContainer ] |> moreClasses.ToHtmlClassAttribute

            levelChildNode
        }

    ///<summary>
    /// Returns a <c>div</c> element of CSS class <see cref="CssClass.level" />
    /// from the specified <see cref="CssBoxAlignment" />.
    ///</summary>
    ///<remarks>
    /// “A multi-purpose horizontal level, which can contain almost any other element…”
    /// 📖 https://bulma.io/documentation/layout/level/
    ///</remarks>
    let bulmaLevelChildAligned (alignment: CssBoxAlignment) (moreClasses: CssClassesOrEmpty) (levelChildNode: Node) =
        div {
            CssClasses [ (alignment |> CssClass.level) ] |> moreClasses.ToHtmlClassAttribute

            levelChildNode
        }

    ///<summary>
    /// Returns a <c>nav</c> element of CSS class <see cref="CssClass.levelItem" />.
    ///</summary>
    ///<remarks>
    /// “In a <c>level-item</c>, you can then insert almost anything you want…
    /// No matter what elements you put inside a Bulma <c>level<c/>, they will always be vertically centered.”
    /// 📖 https://bulma.io/documentation/layout/level/
    ///</remarks>
    let bulmaLevelItem (moreClasses: CssClassesOrEmpty) (attributes: HtmlAttributeOrEmpty) (childNode: Node) =
        div {
            CssClasses [ CssClass.levelItem ] |> moreClasses.ToHtmlClassAttribute
            attributes.Value

            childNode
        }

    ///<summary>
    /// Returns a loader element for <see cref="bulmaLoaderContainer" />.
    ///</summary>
    ///<remarks>
    /// The officially-documented use of Bulma loaders is on the Sass level:
    /// 
    /// 📖 https://bulma.io/documentation/utilities/mixins/#loader
    /// 📰 https://github.com/jgthms/bulma/issues/847
    ///</remarks>
    let bulmaLoader (moreClasses: CssClassesOrEmpty) =
        div {
            CssClasses [ "loader"; DisplayInlineBlock.CssClass ] |> moreClasses.ToHtmlClassAttribute

            attr.title "Loading…"
        }

    ///<summary>
    /// Returns a container of CSS class <see cref="CssClass.media" />,
    /// declaring a child container of CSS class <see cref="CssClass.mediaContent" />,
    /// wrapping the specified media content node.
    ///</summary>
    ///<remarks>
    /// “The famous media object prevalent in social media interfaces, but useful in any context…”
    /// 📖 https://bulma.io/documentation/layout/media-object/
    ///</remarks>
    let bulmaMedia (moreClasses: CssClassesOrEmpty) (mediaLeft: HtmlNodeOrEmpty) (mediaContentNode: Node) =
        let mediaContainerClasses = CssClasses [ CssClass.media ]

        div {
            mediaContainerClasses |> moreClasses.ToHtmlClassAttribute

            mediaLeft.Value

            div {
                CssClass.mediaContent |> CssClasses.toHtmlClass

                mediaContentNode
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
    let bulmaMediaLeft (moreClasses: CssClassesOrEmpty) (attributes: HtmlAttributeOrEmpty) (childNode: Node) =
        figure {
            CssClasses [ CssClass.mediaLeft ] |> moreClasses.ToHtmlClassAttribute
            attributes.Value

            childNode
        }

    /// <summary>
    /// “A simple container to divide your page into sections…”
    /// </summary>
    /// <remarks>
    /// 📖 https://bulma.io/documentation/layout/section/
    ///
    /// <see cref="BulmaSectionModifiers" />
    /// </remarks>
    let bulmaSection (moreClasses: CssClassesOrEmpty) (attributes: HtmlAttributeOrEmpty) (childNode: Node) =
        section {
            CssClasses [ "section" ] |> moreClasses.ToHtmlClassAttribute
            attributes.Value

            childNode
        }

    ///<summary>
    /// Returns a container of CSS class <see cref="CssClass.tile" />,
    /// wrapping the specified tile content node.
    ///</summary>
    ///<remarks>
    /// “The famous media object prevalent in social media interfaces, but useful in any context…”
    /// 📖 https://bulma.io/documentation/layout/tiles/
    ///</remarks>
    let bulmaTile (width: BulmaTileHorizontalSize) (moreClasses: CssClassesOrEmpty) (tileContentNode: Node) =
        let tileContainerClasses = CssClasses [
            CssClass.tile
            match width with | TileSizeAuto -> () | _ -> width.CssClass
        ]

        div {
            tileContainerClasses |> moreClasses.ToHtmlClassAttribute

            tileContentNode
        }
