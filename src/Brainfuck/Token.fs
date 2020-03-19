module Brainfuck.Token

type Token =
    | TRShift
    | TLShift
    | TInc
    | TDec
    | TWrite
    | TRead
    | TLoopBegin
    | TLoopEnd

[<RequireQualifiedAccess>]
module Token =

    let private tryToToken (c: char) =
        match c with
        | '>' -> Some TRShift
        | '<' -> Some TLShift
        | '+' -> Some TInc
        | '-' -> Some TDec
        | '.' -> Some TWrite
        | ',' -> Some TRead
        | '[' -> Some TLoopBegin
        | ']' -> Some TLoopEnd
        | _ -> None // ignore

    let lex (code: string) =
        let res = ResizeArray<Token>()
        for c in code do
            match tryToToken c with
            | Some t -> res.Add(t)
            | None -> ()
        res
