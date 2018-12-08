module Client.State

open Elmish
open Elmish.Browser.UrlParser
open Global
open Types
open Elmish.Browser.Navigation

let pageParser : Parser<Page->Page,Page> =
    oneOf [
        map Page.Demo (s "")
        map Page.Demo (s "demo")
        map Page.Basic (s "basic")
        map Page.Auth (s "auth")
    ]

let urlUpdate (result : Page option) model =
    match result with
    | None ->
        model,Navigation.newUrl (toHash Page.Demo)
    | Some page ->
        { model with currentPage = page }, Cmd.none

let init result =
    let (basicModel, basicClientCmd) = BasicMovieClient.State.init()
    let (authModel, authCmd) = Auth.State.init()
    let (demoModel, demoMsg) = Demo.init()
    let (model, cmd) =
        urlUpdate result
            { basic = basicModel
              demo = demoModel
              auth = authModel
              currentPage = Page.Basic }
    model, Cmd.batch [ cmd
                       basicClientCmd
                       authCmd
                       demoMsg ]

let update msg model =
    match msg with
    | BasicMsg msg ->
        let (basicModel, basicMovieClientMsg) = BasicMovieClient.State.update msg model.basic
        { model with basic = basicModel }, Cmd.map BasicMsg basicMovieClientMsg
    | DemoMsg msg ->
        let (demoModel, demoMsg) = Demo.update msg model.demo
        { model with demo = demoModel }, Cmd.map DemoMsg demoMsg
    | AuthMsg msg ->
        let (authModel, authMsg) = Auth.State.update msg model.auth
        { model with auth = authModel }, Cmd.map AuthMsg authMsg