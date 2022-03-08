namespace Songhay.Player.YouTube.Models

open System

type YouTubeContentDetails = {
    videoId: string option
    videoPublishedAt: DateTime option
    duration: string
    dimension: string
    definition: string
    caption: bool
    licensedContent: bool
    projection: string
    regionRestriction: {| blocked: string[] |} option
}
