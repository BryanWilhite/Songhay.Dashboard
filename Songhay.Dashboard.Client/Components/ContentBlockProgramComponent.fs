namespace Songhay.Dashboard.Client.Components

open Elmish

open Bolero
open Bolero.Html
open Bolero.Remoting
open Bolero.Remoting.Client
open Bolero.Templating.Client

open Songhay.Dashboard.Client.Models

type ContentBlockProgramComponent() =
    inherit ProgramComponent<Model, Message>()
    let update remote message model =
        let onSignIn = function
            | Some _ -> Cmd.ofMsg GetBooks
            | None -> Cmd.none
        match message with
        | SetPage page ->
            { model with page = page }, Cmd.none

        | Increment ->
            { model with counter = model.counter + 1 }, Cmd.none
        | Decrement ->
            { model with counter = model.counter - 1 }, Cmd.none
        | SetCounter value ->
            { model with counter = value }, Cmd.none

        | GetBooks ->
            let cmd = Cmd.OfAsync.either remote.getBooks () GotBooks Error
            { model with books = None }, cmd
        | GotBooks books ->
            { model with books = Some books }, Cmd.none

        | SetUsername s ->
            { model with username = s }, Cmd.none
        | SetPassword s ->
            { model with password = s }, Cmd.none
        | GetSignedInAs ->
            model, Cmd.OfAuthorized.either remote.getUsername () RecvSignedInAs Error
        | RecvSignedInAs username ->
            { model with signedInAs = username }, onSignIn username
        | SendSignIn ->
            model, Cmd.OfAsync.either remote.signIn (model.username, model.password) RecvSignIn Error
        | RecvSignIn username ->
            { model with signedInAs = username; signInFailed = Option.isNone username }, onSignIn username
        | SendSignOut ->
            model, Cmd.OfAsync.either remote.signOut () (fun () -> RecvSignOut) Error
        | RecvSignOut ->
            { model with signedInAs = None; signInFailed = false }, Cmd.none

        | Error RemoteUnauthorizedException ->
            { model with error = Some "You have been logged out."; signedInAs = None }, Cmd.none
        | Error exn ->
            { model with error = Some exn.Message }, Cmd.none
        | ClearError ->
            { model with error = None }, Cmd.none

    /// Connects the routing system to the Elmish application.
    let router = Router.infer SetPage (fun model -> model.page)

    let homePage model dispatch =
        ContentBlockTemplate.Home().Elt()

    let counterPage model dispatch =
        ContentBlockTemplate.Counter()
            .Decrement(fun _ -> dispatch Decrement)
            .Increment(fun _ -> dispatch Increment)
            .Value(model.counter, fun v -> dispatch (SetCounter v))
            .Elt()

    let dataPage model (username: string) dispatch =
        ContentBlockTemplate.Data()
            .Reload(fun _ -> dispatch GetBooks)
            .Username(username)
            .SignOut(fun _ -> dispatch SendSignOut)
            .Rows(cond model.books <| function
                | None ->
                    ContentBlockTemplate.EmptyData().Elt()
                | Some books ->
                    forEach books <| fun book ->
                        tr {
                            td { book.title }
                            td { book.author }
                            td { book.publishDate.ToString("yyyy-MM-dd") }
                            td { book.isbn }
                        })
            .Elt()

    let signInPage model dispatch =
        ContentBlockTemplate.SignIn()
            .Username(model.username, fun s -> dispatch (SetUsername s))
            .Password(model.password, fun s -> dispatch (SetPassword s))
            .SignIn(fun _ -> dispatch SendSignIn)
            .ErrorNotification(
                cond model.signInFailed <| function
                | false -> empty()
                | true ->
                    ContentBlockTemplate.ErrorNotification()
                        .HideClass("is-hidden")
                        .Text("Sign in failed. Use any username and the password \"password\".")
                        .Elt()
            )
            .Elt()

    let menuItem (model: Model) (page: Page) (text: string) =
        ContentBlockTemplate.MenuItem()
            .Active(if model.page = page then "is-active" else "")
            .Url(router.Link page)
            .Text(text)
            .Elt()

    let view model dispatch =
        ContentBlockTemplate()
            .Menu(concat {
                menuItem model Home "Home"
                menuItem model Counter "Counter"
                menuItem model Data "Download data"
            })
            .Body(
                cond model.page <| function
                | Home -> homePage model dispatch
                | Counter -> counterPage model dispatch
                | Data ->
                    cond model.signedInAs <| function
                    | Some username -> dataPage model username dispatch
                    | None -> signInPage model dispatch
            )
            .Error(
                cond model.error <| function
                | None -> empty()
                | Some err ->
                    ContentBlockTemplate.ErrorNotification()
                        .Text(err)
                        .Hide(fun _ -> dispatch ClearError)
                        .Elt()
            )
            .Elt()

    static member val Id = "content-block" with get

    override _.CssScope = nameof(ContentBlockProgramComponent)

    override this.Program =
        let bookService = this.Remote<BookService>()
        let update = update bookService
        Program.mkProgram (fun _ -> Model.initialize(), Cmd.ofMsg GetSignedInAs) update view
        |> Program.withRouter router
#if DEBUG
        |> Program.withHotReload
#endif
