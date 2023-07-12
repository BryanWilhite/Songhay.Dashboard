namespace Songhay.Dashboard.Server

open Microsoft.AspNetCore
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Rewrite
open Microsoft.Extensions.DependencyInjection

open Bolero.Remoting.Server
open Bolero.Server
open Bolero.Server.Html
open Bolero.Templating.Server

open Songhay.Dashboard.Client.Components
open Songhay.Dashboard.Server

type Startup() =

    let htmlNode = doctypeHtml {
        ContentBlockProgramComponent.PComp
        boleroScript
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    member this.ConfigureServices(services: IServiceCollection) =
        services.AddMvc() |> ignore
        services.AddServerSideBlazor() |> ignore
        services.AddHttpClient<DashboardServiceHandler>() |> ignore
        services
            .AddBoleroRemoting<DashboardServiceHandler>()
#if !DEBUG
            .AddBoleroHost(server = false, prerendered = true, devToggle = false)
#endif
#if DEBUG
            .AddBoleroHost(server = false, prerendered = true, devToggle = true)
            .AddHotReload(templateDir = __SOURCE_DIRECTORY__ + "/../Songhay.Dashboard.Client")
#endif
        |> ignore

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    member this.Configure(app: IApplicationBuilder, _: IWebHostEnvironment) =
        app
            .UseRewriter(RewriteOptions().AddRedirectToHttpsPermanent())
            .UseRemoting()
            .UseStaticFiles()
            .UseRouting()
            .UseBlazorFrameworkFiles()
            .UseEndpoints(fun endpoints ->
#if DEBUG
                endpoints.UseHotReload()
#endif
                endpoints.MapBlazorHub() |> ignore
                endpoints.MapFallbackToBolero(htmlNode) |> ignore)
        |> ignore

module Program =

    [<EntryPoint>]
    let main args =
        WebHost
            .CreateDefaultBuilder(args)
            .UseStaticWebAssets()
            .UseStartup<Startup>()
            .Build()
            .Run()
        0
