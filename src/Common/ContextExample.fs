namespace Common

module ContextExample =
    type ExampleStore = {
        message : unit -> Async<string>
        brokenMessage : unit -> Async<string>
        unknownError : unit -> Async<string>
    }

    type CustomError = {
        errorMsg : string
    }