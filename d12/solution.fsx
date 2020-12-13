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
        // printfn "%d" (int state.facing)

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

let part1 = 
    let ss = state_machine input init_state
    (abs ss.x) + (abs ss.y)


// -- part 2

type Ship_With_Waypoint = { x: int; y: int; waypoint_x: int; waypoint_y: int }
let init_state_2 = {x = 0; y = 0; waypoint_x = 10; waypoint_y = 1 }

let rec clockwise_rotate degrees (a,b)=
    if degrees = 0 then
        (a, b)
    else 
        clockwise_rotate (degrees - 90) (b, -a)

let rec counter_clockwise_rotate degrees (a,b)=
    if degrees = 0 then
        (a, b)
    else 
        counter_clockwise_rotate (degrees - 90) (-b, a)

let rec state_machine_part2 (instr: Instruction list) (ship: Ship_With_Waypoint) : Ship_With_Waypoint =
    // printfn "pos: %d %d waypoint: %d %d" ship.x ship.y ship.waypoint_x ship.waypoint_y
    if instr.IsEmpty then
        ship
    else
        let action = instr.Head.action
        let amount = instr.Head.amount
        // printfn "instruction: %s %d" action amount 

        match action with
            | "N" -> { ship with waypoint_y = (ship.waypoint_y + amount)}
            | "S" -> { ship with waypoint_y = (ship.waypoint_y - amount)}
            | "E" -> { ship with waypoint_x = (ship.waypoint_x + amount)}
            | "W" -> { ship with waypoint_x = (ship.waypoint_x - amount)}
            | "R" -> 
                let rot = clockwise_rotate amount (ship.waypoint_x, ship.waypoint_y)
                { ship with waypoint_x = (fst rot); waypoint_y = (snd rot)}
            | "L" ->
                let rot = counter_clockwise_rotate amount (ship.waypoint_x, ship.waypoint_y)
                { ship with waypoint_x = (fst rot); waypoint_y = (snd rot)}                
            | "F" -> 
                    let xDelta = (ship.waypoint_x * amount)
                    let yDelta = (ship.waypoint_y * amount)
                    { 
                        ship with x = ship.x + xDelta; 
                                  y = ship.y + yDelta; 
                    }
            | _ -> failwith("ope!")
            |>
                state_machine_part2 instr.Tail 

// test part 2
let t1 = clockwise_rotate 90 (10,4)
let t2 = counter_clockwise_rotate 270 (10,4)
let t3 = counter_clockwise_rotate 360 (4,-10)
let t4 = clockwise_rotate 360 (4,-10)

let part2 = 
    let ss = state_machine_part2 input init_state_2
    (abs ss.x) + (abs ss.y)


