namespace Common

module DemoStore =
    type Demo = {
        congrats : unit -> Async<string>
    }