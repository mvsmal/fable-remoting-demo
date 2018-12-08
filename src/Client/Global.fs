module Client.Global

type Page =
    | Demo
    | Basic
    | Auth

let toHash = function
    | Demo -> "#/demo"
    | Basic -> "#/basic"
    | Auth -> "#/auth"