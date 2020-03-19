open System
open System.IO

open Brainfuck
open Instruction
open Parser
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
        code |> Parser.parse |> machine.Interpret
        machine.Dump(0,10)
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
        let code = File.ReadAllLines(filename) |> String.concat ""
        // use writer = new StreamWriter(new BufferedStream(Console.OpenStandardOutput()))
        // Console.SetOut(writer)
        code |> Parser.parse |> machine.Interpret
        machine.Dump(0,10)
    | _ -> 
        let msg = """
Usage: 
Brainfuck.exe -> REPL
Brainfuck.exe <path-to-bf-file> -> interpret target file
        """
        Console.WriteLine(msg)
        exit 1
    0 // return an integer exit code
