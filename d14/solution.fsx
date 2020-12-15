open System
open System.IO
open System.Text.RegularExpressions

let fileName = "example"
let rawInput = File.ReadAllText($"d14/{fileName}").Split("\n") |> Seq.toList

let rawMask = rawInput.Head.Split("=").[1].TrimStart() |> Seq.toList

let rec parse_mask idx zeroes_arr ones_arr = 
    // printfn "%d" idx
    if idx < 0 then
        (zeroes_arr, ones_arr)
    else
        match rawMask.[idx] with
            | '1' -> parse_mask (idx - 1)  zeroes_arr   (int (2F ** (float32 (rawMask.Length - idx - 1)) ) :: ones_arr)
            | '0' -> parse_mask (idx - 1)  (int (2F ** (float32 (rawMask.Length - idx - 1)) )  :: zeroes_arr)   ones_arr
            | _   -> parse_mask (idx - 1)  zeroes_arr   ones_arr


// (zero_masks, ones_masks)
// let mask = parse_mask (rawMask.Length - 1) [] []

// (memory_offset, value)
let instructions =
    rawInput.Tail
            |> Seq.map(fun x -> 

                let m = Regex.Match(x, "mem\[(\d.*)\] = (\d.*)") 
                let offset = m.Groups.[1]
                let value = m.Groups.[2]
                (int offset.Value, int value.Value))
                    |> Seq.toList
let results: Map<int, int> = Map.empty

// formula: (0 | ONES) & ~(ZEROS)
let part1 =
    instructions
        |> Seq.map(fun (offset, value) ->
            let apply_ones_results = Seq.fold (fun output x -> output ||| x ) value (snd mask)
            let apply_both_results = Seq.fold (fun output x -> output &&& ~~~(x)) apply_ones_results (fst mask)
            (offset, apply_both_results)
        )
            |> Seq.fold (fun (map: Map<int,int>) (off, v) -> map.Add (off, v)) results

part1
    |> Seq.fold (fun sum item -> sum + item.Value) 0 