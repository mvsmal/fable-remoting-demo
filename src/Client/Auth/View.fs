module Auth.View

open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.Helpers.MaterialUI
open Fable.MaterialUI.Props
open Types
open Common.AuthStore
open Utils.Helpers

let view model dispatch =
    let (AuthToken token) = model.token
    fragment [] [
        div [] [
            span [] [ str "Token: " ]
            str token
        ]
        div [ Class "auth" ] [
            paper [] [
                form [
                    Class "auth-form"
                    HTMLAttr.AutoComplete "off"
                    DOMAttr.OnSubmit (fun e -> e.preventDefault(); Login |> dispatch)
                ] [
                    textField [
                        TextFieldProp.Variant TextFieldVariant.Outlined
                        MaterialProp.Margin FormControlMargin.Normal
                        HTMLAttr.Value (model.username |> Option.defaultValue "")
                        HTMLAttr.Label "Username"
                        HTMLAttr.AutoComplete "off"
                        DOMAttr.OnChange (fun e -> e.Value |> UsernameUpdated |> dispatch)
                    ] []
                    textField [
                        TextFieldProp.Variant TextFieldVariant.Outlined
                        MaterialProp.Margin FormControlMargin.Normal
                        HTMLAttr.Value (model.password |> Option.defaultValue "")
                        HTMLAttr.Label "Password"
                        HTMLAttr.Type "password"
                        HTMLAttr.AutoComplete "off"
                        DOMAttr.OnChange (fun e -> e.Value |> PasswordUpdated |> dispatch)
                    ] []
                    button [
                        HTMLAttr.Type "submit"
                        MaterialProp.Margin FormControlMargin.Normal
                        MaterialProp.Color ComponentColor.Secondary
                        ButtonProp.Variant ButtonVariant.Contained
                    ] [ str "Sign in" ]
                ]
            ]
            paper [] [
                button [
                    MaterialProp.Margin FormControlMargin.Normal
                    MaterialProp.Color ComponentColor.Secondary
                    ButtonProp.Variant ButtonVariant.Contained
                    DOMAttr.OnClick (fun e -> e.preventDefault(); LoadFavoriteMovies |> dispatch)
                ] [ str "Load favorite movies" ]
                list []
                    (model.favMovies
                     |> FSharp.Collections.List.map (fun m ->
                            listItem [] [
                                listItemText [ ListItemTextProp.Primary (m.Title |> toNode) ] []
                            ]))
                
            ]
        ]
    ]