module BasicMovieClient.View

open Fable.Core
open Fable.Helpers.React
open Fable.Helpers.MaterialUI
open Fable.MaterialUI.Props
open Fable.MaterialUI.Themes
open Common.BasicMovieStore
open Types
open Fable.Helpers.React.Props
open Utils.Helpers

let moviesListItems dispatch (movies : Movie list) =
    movies
    |> FSharp.Collections.List.map (fun m ->
        listItem [
            ListItemProp.Button true
            DOMAttr.OnClick (fun _ -> m.Id |> LoadMovie |> dispatch)
        ] [
            listItemText [
                ListItemTextProp.Primary (m.Title |> toNode)
                ListItemTextProp.Secondary (m.Year |> string |> toNode)
            ] []
    ])

let movieList model dispatch =
    match model.movies with
    | [] ->
        str "No movies loaded"
    | movies ->
        list [] (movies |> moviesListItems dispatch)

let movieDetails (movie : MovieDetails option) =
    match movie with
    | None -> str "No movie selected"
    | Some movie ->
        div [ Class "movie-details" ] [
            div [ Class "movie-details-img" ] [
                img [ Src movie.ImgUrl ]
            ]
            div [ Class "movie-details-text" ] [
                typography [ TypographyProp.Variant TypographyVariant.H5 ] [ str movie.Title ]
                typography [ TypographyProp.Variant TypographyVariant.Subtitle1 ] [ str (string movie.Year) ]
            ]
        ]

let newMovieForm model dispatch =
    form [
        DOMAttr.OnSubmit (fun e -> e.preventDefault(); CreateMovie |> dispatch )
    ] [
        typography [ TypographyProp.Variant TypographyVariant.H5 ] [ str "New Movie" ]
        textField [
            TextFieldProp.Variant TextFieldVariant.Outlined
            HTMLAttr.Label "Title"
            HTMLAttr.Value (model.newMovieTitle |> Option.defaultValue "")
            DOMAttr.OnChange (fun e -> e.Value |> NewMovieTitleUpdated |> dispatch)
        ] []
        textField [
            TextFieldProp.Variant TextFieldVariant.Outlined
            HTMLAttr.Label "Year"
            HTMLAttr.Type "number"
            MaterialProp.Value (model.newMovieYear |> Option.map string |> Option.defaultValue "")
            DOMAttr.OnChange (fun e -> e.Value |> int |> NewMovieYearUpdated |> dispatch)
        ] []
        textField [
            TextFieldProp.Variant TextFieldVariant.Outlined
            HTMLAttr.Label "Img URL"
            HTMLAttr.Value (model.newMovieImgUrl |> Option.defaultValue "")
            DOMAttr.OnChange (fun e -> e.Value |> NewMovieImgUrlUpdated |> dispatch)
        ] []
        div [] [
            button [
                HTMLAttr.Type "submit"
                MaterialProp.Color ComponentColor.Primary
                ButtonProp.Variant ButtonVariant.Contained
            ] [ str "Create" ]
        ]
    ]

let view model dispatch =
    div [ Class "basic" ] [
        div [ Class "basic-actions"] [
            yield button [
                MaterialProp.Color ComponentColor.Secondary
                ButtonProp.Variant ButtonVariant.Contained
                DOMAttr.OnClick (fun _ -> LoadMovies |> dispatch)
            ] [ str "Load Movies" ]
            if model.isLoading then
                yield span [ Class "fa fa-spin fa-spinner spinner" ] [ ]
        ]
        div [ Class "basic-split" ] [
            paper [ Classes [ ClassNames.Root "movie-list" ] ] [
                movieList model dispatch
            ]
            paper [ Classes [ ClassNames.Root "selected-movie" ] ] [
                movieDetails model.selectedMovie
            ]
        ]
        paper [ Classes [ ClassNames.Root "new-movie-form" ] ] [
            newMovieForm model dispatch
        ]
    ]


