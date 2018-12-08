module App.View

open Elmish
open Elmish.Browser.Navigation
open Elmish.Browser.UrlParser
open Elmish.React
open Elmish.Debug
open Elmish.HMR
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.Helpers.MaterialUI
open Fable.MaterialUI.Props
open Fable.Core.JsInterop
open Client.Global
open Client.State
open Client.Types
open Fable.Core

importAll "../../public/main.sass" 

let view model dispatch =
    let pageHtml = function
        | Page.Demo ->
            Demo.view model.demo (DemoMsg >> dispatch)
        | Page.Basic ->
            BasicMovieClient.View.view model.basic (BasicMsg >> dispatch)
        | Page.Auth ->
            Auth.View.view model.auth (AuthMsg >> dispatch)

    div [] [
        appBar [
            AppBarProp.Position AppBarPosition.Static
            Style [ Display "flex"; CSSProp.AlignItems "flex-start" ]
        ] [ toolbar [] [
            button [
                HTMLAttr.Href (Demo |> toHash)
                MaterialProp.Color ComponentColor.Inherit
                MaterialProp.Component ("a" |> U3.Case1)
            ] [ str "Demo" ]
            button [
                HTMLAttr.Href (Basic |> toHash)
                MaterialProp.Color ComponentColor.Inherit
                MaterialProp.Component ("a" |> U3.Case1)
            ] [ str "Basic" ]
            button [
                HTMLAttr.Href (Auth |> toHash)
                MaterialProp.Color ComponentColor.Inherit
                MaterialProp.Component ("a" |> U3.Case1)
            ] [ str "Auth" ]
        ]]

        div [ Class "main" ] [
            pageHtml model.currentPage 
        ]
    ]

// App
Program.mkProgram init update view
|> Program.toNavigable (parseHash pageParser) urlUpdate
#if DEBUG
|> Program.withDebugger
#endif
|> Program.withReact "app"
|> Program.run