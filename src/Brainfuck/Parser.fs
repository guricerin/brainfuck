module Brainfuck.Parser

open System
open System.Collections.Generic
open Instruction

[<RequireQualifiedAccess>]
module Parser =

    let private tryToToken (c: char) =
        match c with
        | '>' -> Some RShift
        | '<' -> Some LShift
        | '+' -> Some Inc
        | '-' -> Some Dec
        | '.' -> Some Write
        | ',' -> Some Read
        | '[' -> Some LoopBegin
        | ']' -> Some LoopEnd
        | _ -> None // ignore

    let private lex (code: string) =
        let res = ResizeArray<Instruction>()
        for c in code do
            match tryToToken c with
            | Some t -> res.Add(t)
            | None -> ()
        res

    let private createJumpTable (tokens: ResizeArray<Instruction>) =
        let len = tokens.Count
        let res = Array.init len (fun _ -> Int32.MinValue)
        let begins = Stack<int>()
        for i in 0 .. len - 1 do
            match tokens.[i] with
            | LoopBegin -> begins.Push(i)
            | LoopEnd ->
                if begins.Count < 1 then
                    let msg = sprintf "loop-end-bracket(at %d) has not pair!" i
                    invalidArg "tokens" msg
                let b = begins.Pop()
                res.[b] <- i
                res.[i] <- b
            | _ -> ()

        if begins.Count <> 0 then invalidArg "tokens" "blacket is not balanced!"
        res

    let parse (code: string) =
        let tokens = lex code
        let jumptable = createJumpTable tokens
        tokens, jumptable
