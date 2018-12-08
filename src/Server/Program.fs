module Server

open Saturn
open Microsoft.AspNetCore.Cors.Infrastructure
open Giraffe

let appRouter = router {
    forward "/basic" BasicApi.api
    forward "/auth" AuthApi.api
    forward "/context" ContextApi.api
    forward "/demo" DemoApi.api
}

let corsConfig (builder : CorsPolicyBuilder) =
    builder.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader() |> ignore

let app = application {
    url "http://localhost:5000"
    use_router appRouter
    use_cors "CORS Policy" corsConfig
}

[<EntryPoint>]
let main _ =
    printfn "Starting Saturn app"
    run app
    0
