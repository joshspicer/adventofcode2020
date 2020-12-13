open System
open System.IO

let fileName = "input"
let rawInput = File.ReadAllText($"d11/{fileName}").Split("\n") |> Seq.toList 

// 2D State array 
let populate (input: string list) = 
    Array2D.init (rawInput.Length) (rawInput.Head.Length) (fun x y -> input.[x].[y])

let grid = populate rawInput
let xBound = (Array2D.length2 grid) - 1
let yBound = (Array2D.length1 grid) - 1

let rec occupied_at_slope state x y dx dy =
    let newX = x + dx
    let newY = y + dy
    try
        let seat = Array2D.get state newX newY
        if (seat = 'L') then
            false
        else    
            if (seat = '#') then
                true
            else
                occupied_at_slope state newX newY dx dy
        with
            _ -> false

let num_adjacement_occupied_part2 (state: char [,]) (x: int) (y: int) =
//    printfn "x y: %d %d" x y
   let N =  if y < 1       then false else if (occupied_at_slope state x y 0 -1) then true else false
   let S =  if y >= xBound then false else if (occupied_at_slope state x y 0 1)  then  true else false
   let E =  if x >= yBound then false else if (occupied_at_slope state x y 1 0)  then  true else false
   let W =  if x < 1       then false else if (occupied_at_slope state x y -1 0) then true else false

   let NE = if y < 1 || x >= yBound       then false else if (occupied_at_slope state x y 1 -1)  then true else false
   let NW = if y < 1 || x < 1             then false else if (occupied_at_slope state x y -1 -1) then true else false
   let SW = if y >= xBound || x < 1       then false else if (occupied_at_slope state x y -1 1)  then true else false
   let SE = if y >= xBound || x >= yBound then false else if (occupied_at_slope state x y 1 1)   then true else false

   [N;S;E;W;NE;NW;SW;SE] |> Seq.filter (fun x -> x) |> Seq.length

// If a seat is empty (L) and there are no occupied seats in view to it, the seat becomes occupied.
// If a seat is occupied (#) and FIVE or more seats in view to it are also occupied, the seat becomes empty.
// Otherwise, the seat's state does not change.
// Floor (.) never changes; seats don't move, and nobody sits on the floor.
let do_one_step_part2 (state: char [,]) : char [,] = 
    Array2D.mapi (fun x y value ->
        match value with
            | 'L' -> if (num_adjacement_occupied_part2 state x y) = 0 then '#' else 'L'
            | '#' -> if (num_adjacement_occupied_part2 state x y) >= 5 then 'L' else '#'
            | '.' -> '.'
            |  _  -> failwith("oh no!") ) state 

let arr_equal arr1 arr2 : bool =
    let mutable isEqual = true
    Array2D.iteri (fun x y item -> 
            if ((Array2D.get arr2 x y) = item) |> not then
                isEqual <- false
            ) arr1
    isEqual

let count_symbols arr chr : int =
    let mutable count = 0
    Array2D.iter (fun item -> 
            if item = chr then
                count <- count + 1
            ) arr
    count

let rec part2 prev = 
    let curr = do_one_step_part2 prev
    if arr_equal prev curr then
        curr
    else
        part2 curr

//// Tests
// let testStr = File.ReadAllText($"d11/testStr").Split("\n") |> Seq.toList |> populate
// let step = (do_one_step_part2 (do_one_step_part2 (do_one_step_part2 grid)))
// arr_equal testStr step

count_symbols (part2 grid) '#'

