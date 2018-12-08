module Auth.State

open Elmish
open Types
open Common.AuthStore
open Fable.Remoting.Client
open Utils

let proxy =
    Remoting.createApi ()
    |> Remoting.withBaseUrl "http://localhost:5000/auth/"
    |> Remoting.buildProxy<AuthMovieStore>

let init () =
    { token = AuthToken ""
      username = None
      password = None
      favMovies = [] }, Cmd.none

let stringToOption = function
    | "" -> None
    | str -> Some str

let login loginInfo =
    async {
        let! loginResult = proxy.login loginInfo
        match loginResult with
        | Ok authToken -> return authToken
        | Error error -> return failwith error
    }

let loadFavoriteMovies token =
    async {
        match! proxy.favoriteMovies token with
        | Ok movies -> return movies
        | Error error -> return failwith error
    }

let update msg model =
    match msg with
    | UsernameUpdated username ->
        { model with username = stringToOption username }, Cmd.none
    | PasswordUpdated password ->
        { model with password = stringToOption password }, Cmd.none
    | Login ->
        match model.username, model.password with
        | Some username, Some password ->
            let loginInfo = { Username = username; Password = password }
            model, Cmd.ofAsync login loginInfo LoginSuccess Failure
        | _ ->
            model, Toast.error "Please, enter credentials"
    | LoginSuccess token ->
        { model with token = token }, Cmd.none
    | LoadFavoriteMovies ->
        model, Cmd.ofAsync loadFavoriteMovies model.token FavoriteMoviesLoaded Failure
    | FavoriteMoviesLoaded movies ->
        { model with favMovies = movies }, Toast.success "Favorite movies loaded"
    | Failure error ->
        model, Toast.error error.Message