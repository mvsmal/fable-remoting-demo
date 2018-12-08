module Client.Types

open Global

type Model = {
    basic : BasicMovieClient.Types.Model
    demo : Demo.Model
    auth : Auth.Types.Model
    currentPage : Page
}

type Msg = 
    | BasicMsg of BasicMovieClient.Types.Msg
    | DemoMsg of Demo.Msg
    | AuthMsg of Auth.Types.Msg
