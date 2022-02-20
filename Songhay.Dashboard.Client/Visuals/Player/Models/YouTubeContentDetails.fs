namespace Songhay.Player.Models

open System

type YouTubeContentDetails = {
    videoId: string
    videoPublishedAt: DateTime
    duration: string
    dimension: string
    definition: string
    caption: bool
    licensedContent: bool
    projection: string
}
