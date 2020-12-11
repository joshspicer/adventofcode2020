open System
open System.IO
open System.Collections.Generic

let fileName = "input"
let rawInput = File.ReadAllText($"10/{fileName}").Split("\n") |> Seq.map int |> Seq.toList

// Input values are all distinct. (Checked with my 👀)
let sortedInput = rawInput |> Seq.sort |> Seq.toList

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

// part 2
// Dynamic Programming Question :O
let sortedInputWithFinalDevice = sortedInput @ [(sortedInput |> Seq.last) + 3]

// Return all the possible adapter candidates from `input` given a `joltage`
let rec possible_adapter joltage input = 
    match input with 
            | [] -> [] 
            | x :: xs when x - joltage <= (Seq.max ADAPTER_TOLERANCES)
                -> (x, xs) :: (possible_adapter joltage xs)
            | _ -> []

// Calculate all paths, memoizing for each joltage how many paths we have uncovered.
let rec calculate_all_paths joltage input (lookup: Dictionary<int, int64>) =
    match joltage with
        | j when lookup.ContainsKey j -> lookup.[j] 
        | _ ->
            match input with
                | [_] -> lookup.Add (joltage, 1L) 
                         1L // With no other adapters left, there is only 1 distinct way to order.
                | list
                    ->  // From all the possible adapters that are < 3 from curr joltage, how many paths are there in the remaining list of adapters?
                    let ans =
                        possible_adapter joltage input
                            |> Seq.map (fun (fst, rst) -> calculate_all_paths fst rst lookup)
                            |> Seq.sum
                    lookup.Add (joltage, ans)
                    ans

let part2 = calculate_all_paths 0 sortedInputWithFinalDevice (new Dictionary<int, int64>())