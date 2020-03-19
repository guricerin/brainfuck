open System
open System.IO

open Brainfuck
open Token
open Instruction
open Machine

let printPrompt() = Console.Write("Brainfuck>> ")

let machine = Machine()

let rec repl() =
    printPrompt()
    let code = Console.ReadLine()
    if String.IsNullOrEmpty(code) then
        Console.WriteLine("bye")
        ()
    else
        code |> Token.lex |> Instruction.parse |> machine.Run
        Console.WriteLine()
        repl()

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

[<EntryPoint>]
let main argv =
    match argv.Length with
    | 0 -> 
        Console.WriteLine(title)
        repl()
    | 1 -> 
        let filename = argv.[0]
        let lines = File.ReadLines(filename)
        for line in lines do
            line |> Token.lex |> Instruction.parse |> machine.Run
    | _ -> 
        let msg = """
Usage: 
Brainfuck.exe -> REPL
Brainfuck.exe <path-to-file> -> interpret target file
        """
        Console.WriteLine(msg)
        exit 1
    0 // return an integer exit code
