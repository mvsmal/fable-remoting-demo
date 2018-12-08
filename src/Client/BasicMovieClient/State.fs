module BasicMovieClient.State

open Elmish
open Fable.Remoting.Client
open Common.BasicMovieStore
open Types
open Utils

let movieStore : BasicMovieStore = 
    Remoting.createApi()
    |> Remoting.withBaseUrl "http://localhost:5000/basic"
    |> Remoting.buildProxy<BasicMovieStore>

let init () =
    { movies = []
      selectedMovie = None
      isLoading = false
      newMovieTitle = None
      newMovieYear = None
      newMovieImgUrl = None }, Cmd.none

let update msg model =
    match msg with
    | LoadMovies ->
        { model with isLoading = true },
        Cmd.ofAsync movieStore.allMovies () MoviesLoaded LoadFailed
    | MoviesLoaded movies ->
        { model with movies = movies
                     isLoading = false }, Toast.success "Movies are loaded"
    | LoadMovie id ->
        { model with isLoading = true },
        Cmd.ofAsync movieStore.movieById id MovieLoaded LoadFailed
    | MovieLoaded movie ->
        { model with selectedMovie = movie
                     isLoading = false }, Cmd.none
    | CreateMovie ->
        match model.newMovieTitle, model.newMovieYear, model.newMovieImgUrl with
        | Some title, Some year, Some imgUrl ->
            { model with isLoading = true },
            Cmd.ofAsync movieStore.createMovie (title, year, imgUrl) MovieCreated LoadFailed
        | _ -> model, Toast.warning "Fill in all fields"
    | MovieCreated _ ->
        { model with
            isLoading = false
            newMovieTitle = None
            newMovieYear = None
            newMovieImgUrl = None },
        Cmd.batch [ Toast.success "Movie has been created"
                    Cmd.ofMsg LoadMovies ]
    
    | NewMovieTitleUpdated title ->
        let newTitle =
            match title with
            | "" -> None
            | title -> Some title
        { model with newMovieTitle = newTitle }, Cmd.none
    | NewMovieYearUpdated year ->
        let newYear =
            match year with
            | 0 -> None
            | year -> Some year
        { model with newMovieYear = newYear }, Cmd.none
    | NewMovieImgUrlUpdated url ->
        let newUrl =
            match url with
            | "" -> None
            | url -> Some url
        { model with newMovieImgUrl = newUrl }, Cmd.none

    | LoadFailed ex ->
        { model with isLoading = false }, Toast.error ex.Message
