open System
open System.IO

let fileName = "input"
let input = File.ReadAllText($"9/{fileName}").Split("\n") |>  Seq.map int64 |> Seq.toList 

let preamble_size = 25  // example=5

let rec populate_preamble inputlist (queue: int64 list) counter = 
    if counter = preamble_size then (inputlist, queue) 
        else match inputlist with
                |    (x :: xs) -> populate_preamble xs (queue @ [x]) (counter + 1)
                | _ -> failwith "Can't do it"

let (worklist, queue) = populate_preamble input [] 0

let create_paired (queue: int64 list): int64 list = 
    [ for x = 0 to queue.Length - 1 do
            for y = 0 to queue.Length - 1 do
                match (x,y) with
                    |   (xx,yy) when xx = yy -> -1 |> int64
                    |   _ -> queue.[x] + queue.[y] ]
 
let rec compare_worklist (item: int64) (paired: int64 list) =
    if paired.IsEmpty then
        false
    else 
        if paired |> Seq.contains item then
            true
        else
            match paired with
                | (x :: xs) -> compare_worklist item xs
                | _ -> false

let rec find_imposter (worklist: int64 list) queue : int64=
    let paired = create_paired queue
    if worklist.IsEmpty then
        -2 |> int64
    else
        let hasIntersection = compare_worklist worklist.Head paired
        if hasIntersection then
           match worklist with 
            | (x :: xs) ->  find_imposter xs (queue.Tail @ [x])
            | _ ->   -1 |> int64
        else
            worklist.Head

// let shouldBeTrue = compare_worklist 90 (create_paired [10; 20; 30; 40; 50])
// let shouldBeFalse = compare_worklist 100 (create_paired [10; 20; 30; 40; 50])
find_imposter worklist queue
