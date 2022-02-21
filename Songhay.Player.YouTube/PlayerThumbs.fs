namespace Songhay.Player.YouTube

open Bolero
open Bolero.Html

type PlayerThumbsComponent() =
    inherit ElmishComponent<Model, Message>()

    override this.ShouldRender(oldModel, newModel) =
        oldModel.YouTubeItems <> newModel.YouTubeItems

    override this.View model dispatch =
        text "[yt component]"