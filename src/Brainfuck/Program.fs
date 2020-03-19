open System
open System.IO

open Brainfuck
open Parser
open Machine

let title = """
  _______
< Welcome >
  -------
         \   ^__^ 
          \  (oo)\_______
             (__)\       )\/\
                 ||----w |
                 ||     ||
"""

let discribe = """
> : increment the pointer
< : decrement the pointer
+ : increment the byte at the pointer
- : increment the byte at the pointer
. : output the byte at the pointer
, : accept one byte of input, and store it in the byte at the pointer
[ : if the byte at the pointer is zero, jump to the matching `]`
] : if the byte at the pointer is nonzero, jump to the matching `[`

if you quit repl, input "quit"
"""

let printPrompt() = Console.Write("Brainfuck>> ")

let machine = Machine()

let rec repl() =
    printPrompt()
    let code = Console.ReadLine().Trim()
    if code = "quit" then
        Console.WriteLine("bye")
        ()
    else
        try
            code |> Parser.parse |> machine.Interpret
        with
        | ex -> Console.WriteLine(ex.Message)
        // 出力していた場合、改行してプロンプトを見やすくする
        if machine.Writed() then
            Console.WriteLine()
        repl()

[<EntryPoint>]
let main argv =
    match argv.Length with
    | 0 -> // repl
        Console.Clear()
        Console.WriteLine(title)
        Console.WriteLine(discribe)
        repl()
    | 1 -> // file load
        let filename = argv.[0]
        let code = File.ReadAllLines(filename) |> String.concat ""
        code |> Parser.parse |> machine.Interpret
    | _ -> 
        let msg = """
Usage: 
Brainfuck.exe -> REPL
Brainfuck.exe <path-to-bf-file> -> interpret target file
        """
        stderr.WriteLine(msg)
        exit 1
    0 // return an integer exit code
