module ContextApi
open Common.ContextExample
open Microsoft.Extensions.Logging
open Microsoft.AspNetCore.Http
open Giraffe
open Fable.Remoting.Giraffe
open Fable.Remoting.Server

exception MyCustomException of string

let errorHandler (ex : exn) (routeInfo : RouteInfo<HttpContext>) =
    let logger = routeInfo.httpContext.GetLogger("Error logger")
    match ex with
    | :? MyCustomException as e ->
        logger.LogError (e, "Logging my custom exception")
        let customError = { errorMsg = e.Data0 }
        Propagate customError
    | e ->
        logger.LogCritical (e, "Unhandled error {MethodName} {Path}", routeInfo.methodName, routeInfo.path)
        Ignore

let loggedApi (logger : ILogger) : ExampleStore = {
    message = fun () ->
        async {
            logger.LogInformation ("Message requested")
            return "Congrats again!"
        }
    brokenMessage = fun () ->
        async {
            raise (MyCustomException "A broken message failed to return")
            return "Broken message"
        }
    unknownError = fun () ->
        async {
            raise (exn("Unknown terrible error"))
            return "Unknown error"
        }
}

let contextApi (ctx : HttpContext) =
    loggedApi (ctx.GetLogger("Context API Logger"))

let api : HttpHandler =
    Remoting.createApi ()
    |> Remoting.fromContext contextApi
    |> Remoting.withErrorHandler errorHandler
    |> Remoting.withRouteBuilder (sprintf "/context/%s/%s")
    |> Remoting.buildHttpHandler

