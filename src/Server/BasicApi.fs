module BasicApi

open System
open Giraffe
open Fable.Remoting.Giraffe
open Fable.Remoting.Server
open Common.BasicMovieStore

let mutable movieList = [
    { Id = Guid.Parse "2150E333-8FDC-42A3-9474-1A3956D46DE1"
      Title = "Interstellar"
      Year = 2014
      ImgUrl = "http://www.gstatic.com/tv/thumb/v22vodart/10543523/p10543523_v_v8_aa.jpg" }
    { Id = Guid.Parse "2150E333-8FDC-42A3-9474-1A3956D46DE2"
      Title = "The Godfather"
      Year = 1972
      ImgUrl = "http://www.gstatic.com/tv/thumb/v22vodart/6326/p6326_v_v8_aj.jpg" }
    { Id = Guid.Parse "2150E333-8FDC-42A3-9474-1A3956D46DE3"
      Title = "Forrest Gump"
      Year = 1994
      ImgUrl = "http://www.gstatic.com/tv/thumb/v22vodart/15829/p15829_v_v8_ab.jpg" }
]

let allMovies () =
    async {
        do! Async.Sleep 500 // simulate delay
        let resultList =
            movieList
            |> List.map (fun m ->
                { Id = m.Id
                  Title = m.Title
                  Year = m.Year })
        return resultList
    }

let movieById id =
    async {
        do! Async.Sleep 500 // simulate delay
        let movie =
            movieList |> List.tryFind (fun m -> m.Id = id)
        return movie
    }

let createMovie (title, year, imgAddress) =
    async {
        do! Async.Sleep 500
        let newMovie =
            { Id = Guid.NewGuid()
              Title = title
              Year = year
              ImgUrl = imgAddress }
        movieList <- newMovie :: movieList
        return Some newMovie
    }

let movieStore : BasicMovieStore = {
    allMovies = allMovies
    movieById = movieById
    createMovie = createMovie
}

let api : HttpHandler =
    Remoting.createApi()
    |> Remoting.fromValue movieStore
    |> Remoting.withRouteBuilder (sprintf "/basic/%s/%s")
    |> Remoting.buildHttpHandler
