open System
open System.IO

let fileName = "input"
let input = File.ReadAllText($"10/{fileName}").Split("\n") |> Seq.map int |> Seq.toList

// Input values are all distinct. (Checked with my 👀)
let sortedInput = input |> Seq.sort |> Seq.toList

// Any given adapter can take an input 1, 2, or 3 jolts LOWER than its rating.
// Device can handle 3 jolts HIGHER than the highest-rated adapter in your bag
// outlet == 0 jolts
let OUTLET = 0
let ADAPTER_TOLERANCES = [1..3]
let DEVICE_TOLERANCE = 3

let rec calculate_differences (adapters: int list) seen differences currJolts =
    // Find smallest adapter that can interface with OUTLET given the TOLERANCES
    let possible = adapters |> Seq.map (fun m -> m) |> Seq.filter (fun f ->  (not(List.contains f seen)) && f <= currJolts + (Seq.max ADAPTER_TOLERANCES))
    printfn "\nADAPTERS: %A" (adapters |> Seq.toList)
    printfn "POSSIBLE: %A" (possible |> Seq.toList)
    if possible |> Seq.isEmpty then
        differences
    else
        let min = (possible |> Seq.min)
        printfn "NUM: %d" min
        // [-1]
        calculate_differences adapters (min :: seen) ((min - seen.Head) :: differences) (currJolts + min)

// part 1
let diffs = calculate_differences sortedInput [0] [] OUTLET
let ones = diffs |> Seq.filter (fun x -> x = 1) |> Seq.length
let threes = (diffs |> Seq.filter (fun x -> x = 3) |> Seq.length) + 1 // add one for my device always being 3 higher
let part1 = ones * threes
