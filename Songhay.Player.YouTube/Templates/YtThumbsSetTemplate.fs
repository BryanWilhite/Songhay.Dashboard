module Songhay.Player.YouTube.Templates.YtThumbsSet

open Bolero

type YtThumbsSetTemplate = Template<"Templates/yt-thumbs-set.html">

let viewYtThumbsSetTemplate jsRuntime model dispatch =
    YtThumbsSetTemplate().Elt()
