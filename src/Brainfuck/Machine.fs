module Brainfuck.Machine

open System
open Instruction

[<Literal>]
let MemSize = 30000

type Machine() =
    let mutable _ptr = 0
    let _memory: byte array = Array.zeroCreate MemSize

    let get() = _memory.[_ptr]

    let write() =
        get()
        |> char
        |> Console.Write

    let read() =
        let input = stdin.Read() |> byte
        _memory.[_ptr] <- input

    member __.Interpret(insts: ResizeArray<Instruction>, jumpTable: int array): unit =
        let mutable pc = 0
        let len = insts.Count
        while pc < len do
            let inst = insts.[pc]
            match inst with
            | RShift -> _ptr <- _ptr + 1
            | LShift -> _ptr <- _ptr - 1
            | Inc -> _memory.[_ptr] <- get() + 1uy
            | Dec ->
                if _ptr < 0 then
                    let msg = sprintf "ptr: %d, pc: %d" _ptr pc
                    failwithf "%s" msg
                _memory.[_ptr] <- get() - 1uy
            | Write -> write()
            | Read -> read()
            | LoopBegin
            | LoopEnd ->
                if get() <> 0uy then pc <- jumpTable.[pc]

            pc <- pc + 1

    /// [l, r]
    member __.Dump(l: int, r: int) =
        sprintf "ptr: %d" _ptr |> stderr.WriteLine
        String.Join(" ", _memory.[l..r]) |> stderr.WriteLine
