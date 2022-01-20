namespace Songhay.Dashboard.Client

open Microsoft.AspNetCore.Components.WebAssembly.Hosting

open Bolero.Remoting.Client

open Songhay.Dashboard.Client.Components.ContentBlock

module Program =

    [<EntryPoint>]
    let Main args =
        let builder = WebAssemblyHostBuilder.CreateDefault(args)
        builder.RootComponents.Add<ContentBlockComponent>("#content-block")
        builder.Services.AddRemoting(builder.HostEnvironment) |> ignore
        builder.Build().RunAsync() |> ignore
        0
