namespace Utils

open Elmish.Toastr
open Fable.Core
open Fable.Helpers.React

module Helpers =
    let toNode s =
        s |> str |> U2.Case1 |> U3.Case1

module Toast =
    let baseToast message =
        Toastr.message message
        |> Toastr.position BottomRight

    let error message =
        baseToast message
        |> Toastr.title "Error"
        |> Toastr.error
    
    let success message =
        baseToast message
        |> Toastr.success
    
    let warning message =
        baseToast message
        |> Toastr.title "Warning"
        |> Toastr.warning
