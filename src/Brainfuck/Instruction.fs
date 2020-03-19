module Brainfuck.Instruction

open Token

type Instruction =
    | RShift
    | LShift
    | Inc
    | Dec
    | Write
    | Read
    | Loop of ResizeArray<Instruction>

[<RequireQualifiedAccess>]
module Instruction =

    let rec private parse' (tokens: Token array) =
        let idxAndTokens = tokens |> Array.indexed

        let mutable loopStartIdx, loopStack = 0, 0
        let res = ResizeArray<Instruction>()
        for (i, token) in idxAndTokens do
            if loopStack = 0 then
                let inst =
                    match token with
                    | TRShift -> Some RShift
                    | TLShift -> Some LShift
                    | TInc -> Some Inc
                    | TDec -> Some Dec
                    | TWrite -> Some Write
                    | TRead -> Some Read
                    | TLoopBegin ->
                        do loopStartIdx <- i
                           loopStack <- loopStack + 1
                        None
                    | TLoopEnd ->
                        let msg = sprintf "loop ending (at [%d]) has not loop begin!" i
                        invalidArg "tokens" msg
                match inst with
                | Some t -> res.Add(t)
                | None -> ()
                ()
            else
                match token with
                | TLoopBegin -> loopStack <- loopStack + 1
                | TLoopEnd ->
                    loopStack <- loopStack - 1
                    // whileがネストしている場合、一番外側のwhileに達してから、内側のwhileを順次パースしていく
                    if loopStack = 0 then
                        let body = tokens.[loopStartIdx + 1..i - 1] |> parse'
                        let loop = Loop(body)
                        res.Add(loop)
                | _ -> ()
        if loopStack <> 0 then invalidArg "tokens" "Brackets is not balanced!"
        res

    let parse (tokens: ResizeArray<Token>) =
        tokens
        |> Seq.toArray
        |> parse'
