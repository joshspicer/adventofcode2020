open System
open System.IO
open System.Text.RegularExpressions

let fileName = "example1p2"
let rawInput = File.ReadAllText($"d14/{fileName}").Split("\n") |> Seq.toList

let rec parse_mask (string_mask: string) idx zeroes_arr ones_arr = 
    // printfn "%d" idx
    if idx < 0 then
        (zeroes_arr, ones_arr)
    else
        match string_mask.[idx] with
            | '1' -> parse_mask string_mask (idx - 1)  zeroes_arr   (int64 (2F ** (float32 (string_mask.Length - idx - 1)) ) :: ones_arr)
            | '0' -> parse_mask string_mask (idx - 1)  (int64 (2F ** (float32 (string_mask.Length - idx - 1)) )  :: zeroes_arr)   ones_arr
            | _   -> parse_mask string_mask (idx - 1)  zeroes_arr   ones_arr

let parse_instructions mask_parser =
    rawInput
            |> Seq.fold(fun instructions token -> 
                    // Try to parse mask
                    
                    if token.Contains("mask") then
                        let mask = token.Split("=").[1].TrimStart()
                        let parsed_mask = mask_parser mask 35 [] []

                        [(parsed_mask, [])] @ instructions
                        
                    else
                        let m = Regex.Match(token, "mem\[(\d.*)\] = (\d.*)") 
                        let offset = int64 m.Groups.[1].Value
                        let value = int64 m.Groups.[2].Value
                        let head = instructions.Head
                        let mask = (fst head)
                        let previous_instructs = (snd head)
                        (mask,  previous_instructs @ [(offset, value)]) :: instructions.Tail                      
        ) []

let apply prevMap input =
    let zeros = fst (fst input)
    let ones = snd (fst input)
    let instructions = snd input
    instructions
        |> Seq.map(fun input ->
            let offset = fst input
            let value = snd input
            let apply_ones_results = Seq.fold (fun output (x :int64) -> output ||| x ) value ones
            let apply_both_results = Seq.fold (fun output x -> output &&& ~~~(x)) apply_ones_results zeros
            (offset, apply_both_results)
        )
            |> Seq.fold (fun (map: Map<int64,int64>) (off, v) -> map.Add (off, v)) prevMap

// part1

(parse_instructions parse_mask)
    |> Seq.rev
    |> Seq.fold (fun map item -> apply map item) Map.empty 
    |> Seq.fold (fun sum resultsMap -> sum + resultsMap.Value) 0L
