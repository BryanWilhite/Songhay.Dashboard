namespace Songhay.Modules.Bolero.Visuals

open Bolero
open Bolero.Html

open Songhay.Modules.MimeTypes
open Songhay.Modules.Bolero.BoleroUtility

module Document =
    module Head =
        let metaElements =
            [
                meta { attr.charset "UTF-8" }
                meta { attr.name "viewport"; attr.content "width=device-width, initial-scale=1.0" }
            ]

        let robotsMetaElement = meta { attr.name "robots"; attr.content "index, follow" }

        let baseElement (href: string option) = ``base`` { attr.href (href |> Option.defaultWith (fun _ -> "/")) }

        let canonicalLinkElement (href: string) = link { attr.rel "canonical"; attr.href href }
     
        let atomAlternateLinkElement (appTitle: string) (href: string) =
            link { attr.rel "alternate"; attr.``type`` ApplicationAtomXml; attr.title appTitle; attr.href href }

        let studioLinkElements (rootCompId: string) =
            [
                link { attr.rel "stylesheet"; attr.href $"css/{rootCompId}.min.css" }
                link { attr.rel "icon"; attr.``type`` "image/x-icon"; attr.href "favicon.ico" }
            ]

        let studioScriptElement = script { attr.src "js/songhay.min.js" }

        let titleElement (appTitle: string) = title { text appTitle }

        let headElement (headElements: Node list) = head { forEach ((2, headElements) ||> wrapn) <| id }
