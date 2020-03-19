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

    member private __.Eval(inst: Instruction): unit =
        match inst with
        | RShift -> _ptr <- _ptr + 1
        | LShift -> _ptr <- _ptr - 1
        | Inc -> _memory.[_ptr] <- get() + 1uy
        | Dec -> _memory.[_ptr] <- get() - 1uy
        | Write -> write()
        | Read -> read()
        | Loop insts ->
            while get() <> 0uy do
                __.Run(insts)

    member __.Run(insts: ResizeArray<Instruction>): unit =
        for inst in insts do
            __.Eval(inst)

    /// [l, r]
    member __.Dump(l: int, r: int) =
        sprintf "ptr: %d" _ptr |> stderr.WriteLine
        String.Join(" ", _memory.[l..r]) |> stderr.WriteLine
