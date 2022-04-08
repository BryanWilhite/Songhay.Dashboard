namespace Songhay.Modules.Models

open System

open Songhay.Modules.Models

type DisplayText =
    | DisplayText of string
    member this.Value = let (DisplayText v) = this in v

type ItemName =
    | ItemName of string
    member this.Value = let (ItemName v) = this in v

/// <summary>
/// Conventional Model for a UI display item
/// </summary>
type DisplayItemModel =
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        id: Identifier

        /// <summary>
        /// Gets or sets the item name.
        /// </summary>
        /// <value>The item name.</value>
        itemName: ItemName option

        /// <summary>
        /// Gets or sets the display text.
        /// </summary>
        /// <value>The display text.</value>
        displayText: DisplayText option

        /// <summary>
        /// Gets or sets the resource indicator.
        /// </summary>
        /// <value>
        /// The resource indicator.
        /// </value>
        resourceIndicator: Uri option
    }
