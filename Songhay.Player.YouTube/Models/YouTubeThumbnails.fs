namespace Songhay.Player.YouTube.Models

type YouTubeThumbnail = {
    url: string
    width: int
    height: int
}

type YouTubeThumbnails = {
    ``default``: YouTubeThumbnail
    medium: YouTubeThumbnail
    high: YouTubeThumbnail
    standard: YouTubeThumbnail option
    maxres: YouTubeThumbnail option
}
