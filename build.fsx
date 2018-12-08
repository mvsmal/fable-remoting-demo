#load ".fake/build.fsx/intellisense.fsx"
open Fake.DotNet
open Fake.IO
open Fake.Core
open Fake.Core.TargetOperators
open Fake.IO.Globbing.Operators

let serverPath = "./src/Server/"
let clientPath = "./src/Client"
let inline withWorkDir wd =
    DotNet.Options.withWorkingDirectory wd

Target.create "CleanServer" (fun _ ->
    !! ("src/Server/bin")
    ++ ("src/Server/obj")
    |> Shell.cleanDirs)

Target.create "CleanAll" (fun _ ->
    !! ("src/**/bin")
    ++ ("src/**/obj")
    |> Shell.cleanDirs
)

Target.create "BuildServer" (fun _ -> DotNet.build id serverPath)
Target.create "RunServer" (fun _ ->
    DotNet.exec (fun p ->
      { p with WorkingDirectory = serverPath } ) "watch" "run" |> ignore
)

Target.create "CleanClient" (fun _ -> 
    !! ("src/Client/bin")
    ++ ("src/Client/obj")
    |> Shell.cleanDirs
)
Target.create "BuildClient" (fun _ -> DotNet.build id clientPath)
Target.create "RunClient" (fun _ -> DotNet.exec (withWorkDir clientPath) "fable" "yarn-run start" |> ignore)

"CleanServer"
    ==> "BuildServer"
    ==> "RunServer"

"CleanClient"
    ==> "BuildClient"
    ==> "RunClient"

Target.runOrDefault "BuildServer"
