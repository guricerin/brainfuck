module Brainfuck.Instruction

type Instruction =
    | RShift
    | LShift
    | Inc
    | Dec
    | Write
    | Read
    | LoopBegin
    | LoopEnd
