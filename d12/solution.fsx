open System
open System.IO

let fileName = "input"
let rawInput = File.ReadAllText($"d12/{fileName}").Split("\n")

// Can rotate left or right with +/- mod 3
type Compass =
   | North = 270
   | East = 0
   | South = 90
   | West = 180

// Action - oneof: N,S,E,W,L,R,F
type Instruction = { action: string; amount: int }
type Ship = { x: int; y: int; facing: Compass; }

let input = 
    rawInput
        |> Seq.map (fun x -> {action = x.Substring(0,1); amount = x.Substring(1) |> int } ) |> Seq.toList

let init_state = {x = 0; y = 0; facing = Compass.East}

let rec state_machine (instr: Instruction list) (state: Ship) : Ship =
    if instr.IsEmpty then
        state
    else
        let action = instr.Head.action
        let amount = instr.Head.amount
        printfn "%d" (int state.facing)

        match action with
            | "N" -> {state with y = (state.y + amount)}
            | "S" -> {state with y = (state.y - amount)}
            | "E" -> {state with x = (state.x + amount)}
            | "W" -> {state with x = (state.x - amount)}
            | "R" -> {state with facing = enum<Compass>( (amount   +  (int state.facing) + 360) % 360) } 
            | "L" -> {state with facing = enum<Compass>( (-amount  +  (int state.facing) + 360) % 360) }
            | "F" -> 
                match state.facing with
                    | Compass.North ->  {state with y = (state.y + amount)}
                    | Compass.South ->  {state with y = (state.y - amount)}
                    | Compass.East  ->  {state with x = (state.x + amount)}
                    | Compass.West  ->  {state with x = (state.x - amount)}
                    | _ -> failwith($"{state.facing} ...ah!")
            | _ -> failwith("ope!")
            |>
                state_machine instr.Tail

let part1 = state_machine input init_state
printfn "Facing: %s" (part1.facing.ToString())

(abs part1.x) + (abs part1.y)


