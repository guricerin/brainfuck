#if !FAKE
#load ".fake/build.fsx/intellisense.fsx"
#r "netstandard"
#endif

open Fake.Core
open Fake.DotNet
open Fake.DotNet.Testing
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open Fake.Core.TargetOperators

let dotnet cmd arg = DotNet.exec id cmd arg |> ignore

Target.create "Tool" (fun _ ->
    dotnet "tool" "update paket"
    dotnet "tool" "update fake-cli")

Target.create "Clean" (fun _ -> !!"src/**/bin" ++ "src/**/obj" ++ "tests/**/bin" ++ "tests/**/obj" |> Shell.cleanDirs)

Target.create "Build" (fun _ -> !!"src/**/*.*proj" ++ "tests/**/*.*proj" |> Seq.iter (DotNet.build id))

Target.create "All" ignore

"Clean" ==> "Build" ==> "All"

"Tool"
"Tool"

Target.runOrDefault "All"
