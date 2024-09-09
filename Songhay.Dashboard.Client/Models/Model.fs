namespace Songhay.Dashboard.Client.Models

open System

/// The Elmish application's model.
type Model =
    {
        page: Page
        counter: int
        books: Book[] option
        error: string option
        username: string
        password: string
        signedInAs: option<string>
        signInFailed: bool
    }

    static member initialize () =
        {
            page = Home
            counter = 0
            books = None
            error = None
            username = ""
            password = ""
            signedInAs = None
            signInFailed = false
        }
