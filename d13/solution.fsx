open System
open System.IO
let fileName = "input"
let rawInput = File.ReadAllText($"d13/{fileName}").Split("\n") |> Seq.toList
let timeOffset = rawInput |> Seq.head |> int
let buses =
    let rawBus = rawInput.[1]
    rawBus.Split(",") |> Seq.filter (fun x -> not (x = "x")) |> Seq.map (fun x -> int x) |> Seq.toList

let part1 = 
    buses |> Seq.map (fun x -> (x, ((timeOffset / x) + 1) * x - timeOffset)) |> Seq.minBy (fun m -> snd m)

(fst part1) * (snd part1)