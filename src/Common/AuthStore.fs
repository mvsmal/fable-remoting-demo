namespace Common
open System

module AuthStore =
    type AuthToken = AuthToken of string

    type LoginInfo = {
        Username : string
        Password : string
    }

    type SecureRequest<'t> = {
        Token : AuthToken
        Payload : 't
    }

    type FavoriteMovie = {
        Id : Guid
        Title : string
    }

    type AuthMovieStore = {
        login : LoginInfo -> Async<Result<AuthToken, string>>
        favoriteMovies : AuthToken -> Async<Result<FavoriteMovie list, string>>
    }