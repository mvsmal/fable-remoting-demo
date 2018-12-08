module Demo

open Elmish
open Common.DemoStore
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.Helpers.MaterialUI
open Fable.MaterialUI.Props
open Fable.Remoting.Client
open Fable.Import
open Utils

type Model = {
    text : string
}

type Msg =
    | LoadCongrats
    | CongratsLoaded of string
    | LoadFailed of exn

let proxy : Demo =
    Remoting.createApi ()
    |> Remoting.withBaseUrl "http://localhost:5000/demo"
    |> Remoting.buildProxy

let update msg model =
    match msg with
    | LoadCongrats ->
        model, Cmd.ofAsync proxy.congrats () CongratsLoaded LoadFailed
    | CongratsLoaded congrats -> 
        { model with text = congrats }, Cmd.none
    | LoadFailed ex ->
        model, Toast.error ex.Message

let init () =
    { text = "No congrats yet" }, Cmd.none

let view model dispatch =
    div [] [
        div [] [
            button [ 
                MaterialProp.Color ComponentColor.Primary
                ButtonProp.Variant ButtonVariant.Contained
                OnClick (fun _ -> LoadCongrats |> dispatch)
            ] [ str "Load congrats" ]
        ]
        div [ Class "congrats" ] [ str model.text ]
    ]