namespace Songhay.Modules

/// <summary>
/// Defines a selectable visual.
/// </summary>
type Selectable = {

    /// <summary>
    /// Gets or sets whether this is default selection.
    /// </summary>
    /// <value>
    /// This is default selection.
    /// </value>
    isDefaultSelection : bool option

    /// <summary>
    /// Gets or sets whether this is enabled.
    /// </summary>
    /// <value>
    /// This is enabled.
    /// </value>
    isEnabled: bool option

    /// <summary>
    /// Gets or sets whether this is selected.
    /// </summary>
    /// <value>
    /// <c>true</c> when this is selected.
    /// </value>
    isSelected: bool

}
