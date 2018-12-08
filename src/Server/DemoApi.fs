module DemoApi

open Common.DemoStore
open Giraffe
open Fable.Remoting.Giraffe
open Fable.Remoting.Server

let demo = {
    congrats = fun () -> async {
        return "Happy 20th meetup, FSharpers!"
    }
}

let api : HttpHandler =
    Remoting.createApi ()
    |> Remoting.fromValue demo
    |> Remoting.withRouteBuilder (sprintf "/demo/%s/%s")
    |> Remoting.buildHttpHandler