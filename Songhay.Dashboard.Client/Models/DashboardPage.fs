namespace Songhay.Dashboard.Client.Models

open Bolero

type DashboardPage =
    | [<EndPoint "/">] StudioToolsPage
    | [<EndPoint "/feeds">] StudioFeedsPage
    | [<EndPoint "/yt/figure">] YouTubeFigurePage
