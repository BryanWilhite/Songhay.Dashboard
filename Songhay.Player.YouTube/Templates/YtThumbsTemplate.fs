module Songhay.Player.YouTube.Templates.YtThumbs

open Bolero

type YtThumbsTemplate = Template<"Templates/yt-thumbs.html">

let viewYtThumbsTemplate jsRuntime model dispatch =
    YtThumbsTemplate().Elt()
