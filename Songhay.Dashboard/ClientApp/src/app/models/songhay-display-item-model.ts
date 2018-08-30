/**
 * Model for display item
 * @remarks
 * This class was originally developed
 * to compensate for RIA Services not supporting composition
 * of Entity Framework entities
 * where an Entity is the property of another object.
 */
export interface DisplayItemModel {
    /**
     * Gets or sets the description.
     * @value  The description.
     */
    description: string;

    /**
     * Gets or sets the display text.
     * @value  The display text.
     */
    displayText: string;

    /**
     * Gets or sets the id.
     * @value  The id.
     */
    id: number;

    /**
     * Gets or sets the item name.
     * @value  The item name.
     */
    itemName: string;

    /**
     * Gets or sets the resource indicator.
     * @value
     * The resource indicator.
     */
    resourceIndicator: string;

    /**
     * Gets or sets the item category.
     * @value  The item category.
     */
    itemCategory: string;

    /**
     * Gets or sets the sort ordinal.
     * @value  The sort ordinal.
     */
    sortOrdinal: number;

    /**
     * Gets or sets the tag.
     * @value
     * The tag.
     */
    tag: string;
}
