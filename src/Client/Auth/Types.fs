module Auth.Types
open Common.AuthStore

type Model = {
    token : AuthToken
    username : string option
    password : string option
    favMovies : FavoriteMovie list
}

type Msg =
    | UsernameUpdated of string
    | PasswordUpdated of string

    | Login
    | LoginSuccess of AuthToken

    | LoadFavoriteMovies
    | FavoriteMoviesLoaded of FavoriteMovie list

    | Failure of exn