module Brainfuck.Parser

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

    let lex (code: string) =
        let res = ResizeArray<Instruction>()
        for c in code do
            match tryToToken c with
            | Some t -> res.Add(t)
            | None -> ()
        res

    let createJumpTable (tokens: ResizeArray<Instruction>) =
        let len = tokens.Count
        let res = Array.init len (fun _ -> -1)
        let mutable l = 0
        while l < len do
            if tokens.[l] = LoopBegin then
                let mutable nest = 1
                let mutable r = l + 1
                while nest <> 0 && r < len do
                    match tokens.[r] with
                    | LoopBegin -> nest <- nest + 1
                    | LoopEnd -> nest <- nest - 1
                    | _ -> ()
                    r <- r + 1

                if nest = 0 then
                    r <- if r = l + 1 then r else r - 1
                    res.[l] <- r
                    res.[r] <- l
                else
                    let msg = sprintf "loop-begin-blacket(at %d) is not balanced!" l
                    invalidArg "tokens" msg

            l <- l + 1

        res

    let parse (code: string) =
        let tokens = lex code
        let jumptable = createJumpTable tokens
        tokens, jumptable
