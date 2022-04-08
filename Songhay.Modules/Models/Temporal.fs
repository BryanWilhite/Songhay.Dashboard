namespace Songhay.Modules.Models

/// <summary>
/// Adds temporal properties to an item
/// </summary>
type Temporal = {
    /// <summary>
    /// End/expiration <see cref="DateTime"/> of the item.
    /// </summary>
    endDate: EndDate option

    /// <summary>
    /// Origin <see cref="DateTime"/> of the item.
    /// </summary>
    inceptDate: InceptDate option

    /// <summary>
    /// Modification/editorial <see cref="DateTime"/> of the item.
    /// </summary>
    modificationDate: ModificationDate option
}
