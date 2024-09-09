namespace Songhay.Dashboard.Server

open Microsoft.AspNetCore
open Microsoft.AspNetCore.Authentication.Cookies
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting

open Bolero.Remoting.Server
open Bolero.Html
open Bolero.Server
open Bolero.Server.Html
open Bolero.Templating.Server

open Songhay.Dashboard.Client.Components

type Startup() =

    let page = doctypeHtml {
        head {
            meta { attr.charset "UTF-8" }
            meta { attr.name "viewport"; attr.content "width=device-width, initial-scale=1.0" }
            title { "Bolero Application" }
            ``base`` { attr.href "/" }
            link { attr.rel "stylesheet"; attr.href "https://cdnjs.cloudflare.com/ajax/libs/bulma/0.7.4/css/bulma.min.css" }
            link { attr.rel "stylesheet"; attr.href "css/index.css" }
            link { attr.rel "stylesheet"; attr.href "Songhay.Dashboard.Client.styles.css" }
        }
        body {
            nav {
                attr.``class`` "navbar is-dark"
                "role" => "navigation"
                attr.aria "label" "main navigation"
                div {
                    attr.``class`` "navbar-brand"
                    a {
                        attr.``class`` "navbar-item has-text-weight-bold is-size-5"
                        attr.href "https://fsbolero.io"
                        img { attr.style "height:40px"; attr.src "https://github.com/fsbolero/website/raw/master/src/Website/img/wasm-fsharp.png" }
                        "  Bolero"
                    }
                }
            }
            div { attr.id <| ContentBlockProgramComponent.Id; comp<ContentBlockProgramComponent> }
            boleroScript
        }
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    member this.ConfigureServices(services: IServiceCollection) =
        services.AddMvc() |> ignore
        services.AddServerSideBlazor() |> ignore
        services
            .AddAuthorization()
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie()
                .Services
            .AddBoleroRemoting<BookService>()
            .AddBoleroHost()
#if DEBUG
            .AddHotReload(templateDir = __SOURCE_DIRECTORY__ + "/../Songhay.Dashboard.Client")
#endif
        |> ignore

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    member this.Configure(app: IApplicationBuilder, env: IWebHostEnvironment) =
        if env.IsDevelopment() then
            app.UseWebAssemblyDebugging()

        app
            .UseAuthentication()
            .UseStaticFiles()
            .UseRouting()
            .UseAuthorization()
            .UseBlazorFrameworkFiles()
            .UseEndpoints(fun endpoints ->
#if DEBUG
                endpoints.UseHotReload()
#endif
                endpoints.MapBoleroRemoting() |> ignore
                endpoints.MapBlazorHub() |> ignore
                endpoints.MapFallbackToBolero(page) |> ignore)
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
