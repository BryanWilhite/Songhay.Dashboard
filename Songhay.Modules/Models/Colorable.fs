namespace Songhay.Modules.Models

type HexValue =
    | HexValue of string
    member this.Value = let (HexValue v) = this in v

type BackgroundColorHexValue = 
    | BackgroundColorHexValue of HexValue
    static member fromString s = BackgroundColorHexValue (HexValue s)
    member this.Value = let (BackgroundColorHexValue v) = this in v
    member this.Unwrapped = this.Value.Value

type ForegroundColorHexValue = 
    | ForegroundColorHexValue of HexValue
    static member fromString s = ForegroundColorHexValue (HexValue s)
    member this.Value = let (ForegroundColorHexValue v) = this in v
    member this.Unwrapped = this.Value.Value

/// <summary>
/// Defines a colorable visual.
/// </summary>
type Colorable =
    {
        /// <summary>
        /// Gets or sets the background hexadecimal value.
        /// </summary>
        /// <value>The background hexadecimal value.</value>
        backgroundHex: BackgroundColorHexValue

        /// <summary>
        /// Gets or sets the foreground hexadecimal value.
        /// </summary>
        /// <value>The foreground hexadecimal value.</value>
        foregroundHex: ForegroundColorHexValue
    }
