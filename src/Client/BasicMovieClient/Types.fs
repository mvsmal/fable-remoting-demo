module BasicMovieClient.Types

open System
open Common.BasicMovieStore

type Model = {
    movies : Movie list
    selectedMovie : MovieDetails option
    isLoading : bool
    newMovieTitle : string option
    newMovieYear : int option
    newMovieImgUrl : string option
}

type Msg =
    | LoadMovies
    | MoviesLoaded of Movie list

    | LoadMovie of Guid
    | MovieLoaded of MovieDetails option

    | CreateMovie
    | MovieCreated of MovieDetails option

    | LoadFailed of exn

    | NewMovieTitleUpdated of string
    | NewMovieYearUpdated of int
    | NewMovieImgUrlUpdated of string
