open System
open System.IO

let fileName = "input"
let rawInput = File.ReadAllText($"d11/{fileName}").Split("\n") |> Seq.toList 

// 2D State array 
let populate (input: string list) = 
    Array2D.init (rawInput.Length) (rawInput.Head.Length) (fun x y -> input.[x].[y])

let grid = populate rawInput

let num_adjacement_occupied (state: char [,]) (x: int) (y: int) =
//    printfn "x y: %d %d" x y
   let xBound = (Array2D.length2 state) - 1
   let yBound = (Array2D.length1 state) - 1

   let N =  if y < 1       then false else (Array2D.get state x       (y - 1)) = '#'
   let S =  if y >= xBound then false else (Array2D.get state x       (y + 1)) = '#'
   let E =  if x >= yBound then false else (Array2D.get state (x + 1)  y)      = '#'
   let W =  if x < 1       then false else (Array2D.get state (x - 1)  y)      = '#'

   let NE = if y < 1 || x >= yBound      then false else (Array2D.get state (x + 1) (y - 1)) = '#'
   let NW = if y < 1 || x < 1           then false else (Array2D.get state (x - 1) (y - 1)) = '#'
   let SW = if y >= xBound || x < 1      then false else (Array2D.get state (x - 1) (y + 1)) = '#'
   let SE = if y >= xBound || x >= yBound then false else (Array2D.get state (x + 1) (y + 1)) = '#'

   [N;S;E;W;NE;NW;SW;SE] |> Seq.filter (fun x -> x) |> Seq.length


// If a seat is empty (L) and there are no occupied seats adjacent to it, the seat becomes occupied.
// If a seat is occupied (#) and four or more seats adjacent to it are also occupied, the seat becomes empty.
// Otherwise, the seat's state does not change.
// Floor (.) never changes; seats don't move, and nobody sits on the floor.
let do_one_step (state: char [,]) : char [,] = 
    Array2D.mapi (fun x y value ->
        match value with
            | 'L' -> if (num_adjacement_occupied state x y) = 0 then '#' else 'L'
            | '#' -> if (num_adjacement_occupied state x y) >= 4 then 'L' else '#'
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

let rec part1 prev = 
    let curr = do_one_step prev
    if arr_equal prev curr then
        curr
    else
        part1 curr


count_symbols (part1 grid) '#'

// arr_equal grid grid
