namespace Songhay.Modules.Models

open System

/// <summary>
/// Adds temporal properties to an item
/// </summary>
type Temporal = {
    /// <summary>
    /// End/expiration <see cref="DateTime"/> of the item.
    /// </summary>
    endDate: DateTime option

    /// <summary>
    /// Origin <see cref="DateTime"/> of the item.
    /// </summary>
    inceptDate: DateTime option

    /// <summary>
    /// Modification/editorial <see cref="DateTime"/> of the item.
    /// </summary>
    modificationDate: DateTime option
}