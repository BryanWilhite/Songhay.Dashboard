namespace Songhay.Modules.Models

open Songhay.Modules.Models

/// <summary>
/// Defines a group-able visual
/// </summary>
type Groupable =
    {

        /// <summary>
        /// Display text of the Group
        /// </summary>
        groupDisplayText: string

        /// <summary>
        /// Identifier of the Group
        /// </summary>
        groupId: Identifier

        /// <summary>
        /// Returns `true` when group is visually collapsed
        /// </summary>
        isCollapsed: bool

    }
