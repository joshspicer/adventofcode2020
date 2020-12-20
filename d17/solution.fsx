open System
open System.IO
open System.Text.RegularExpressions

let fileName = "example"
let rawInput = File.ReadAllText($"d17/{fileName}").Split("\n") |> Seq.toList
let init_slice = Array2D.init (rawInput.Length) (rawInput.Head.Length) (fun x y -> rawInput.[x].[y])

let (cube : string [,,]) = Array3D.zeroCreate rawInput.Length rawInput.Length rawInput.Length

let neighbor_coords ((x,y,z): (int*int*int)) = 
    [
        (-1,-1);(0,-1);(1,-1)
        (-1,0); (0,0); (1,0)
        (-1,1); (0,1); (1,1)
    ]
        |> Seq.collect (fun (xx,yy) -> [ (xx,yy,0); (xx,yy,1); (xx,yy,-1) ])
        |> Seq.map (fun (xxx,yyy,zzz) -> (x+xxx, y+yyy, z+zzz))
        |> Seq.except [(x,y,z)]

let num_active range =
    true // TODO

// If a cube is active and exactly 2 or 3 of its neighbors are also active, the cube remains active. Otherwise, the cube becomes inactive.
// If a cube is inactive but exactly 3 of its neighbors are active, the cube becomes active. Otherwise, the cube remains inactive.
let analyze_slice slice = 
    Array2D.mapi (fun x y value ->
        match value with
            | '#' -> if num_active [2..3] then '#' else '.'
            | '.' -> if num_active [3] then '#' else '.'
            | _ -> failwith ("uh oh!") 
    )

    