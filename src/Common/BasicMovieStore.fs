namespace Common

module BasicMovieStore =
    open System

    type MovieDetails = {
        Id : Guid
        Title : string
        Year : int
        ImgUrl : string
    }

    type Movie = {
        Id : Guid
        Title : string
        Year : int
    }

    type BasicMovieStore = {
        allMovies : unit -> Async<Movie list>
        movieById : Guid -> Async<MovieDetails option>
        createMovie : (string * int * string) -> Async<MovieDetails option>
    }
