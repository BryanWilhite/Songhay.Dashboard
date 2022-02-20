namespace Songhay.Player.Models

type YouTubeThumbnail = {
    url: string
    width: int
    height: int
}

type YouTubeThumbnails = {
    ``default``: YouTubeThumbnail
    medium: YouTubeThumbnail
    high: YouTubeThumbnail
    standard: YouTubeThumbnail
    maxres: YouTubeThumbnail
}
