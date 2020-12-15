open System
open System.IO
open System.Text.RegularExpressions

let fileName = "input"
let rawInput = File.ReadAllText($"d14/{fileName}").Split("\n") |> Seq.toList


let rec parse_mask (string_mask: string) idx zeroes_arr ones_arr = 
    // printfn "%d" idx
    if idx < 0 then
        (zeroes_arr, ones_arr)
    else
        match string_mask.[idx] with
            | '1' -> parse_mask string_mask (idx - 1)  zeroes_arr   (int (2F ** (float32 (string_mask.Length - idx - 1)) ) :: ones_arr)
            | '0' -> parse_mask string_mask (idx - 1)  (int (2F ** (float32 (string_mask.Length - idx - 1)) )  :: zeroes_arr)   ones_arr
            | _   -> parse_mask string_mask (idx - 1)  zeroes_arr   ones_arr


let parse_instructions =
    rawInput
            |> Seq.fold(fun instructions token -> 
                    // Try to parse mask
                    
                    if token.Contains("mask") then
                        let mask = token.Split("=").[1].TrimStart()
                        let parsed_mask = parse_mask mask 35 [] []

                        [(parsed_mask, [])] @ instructions
                        
                    else
                        let m = Regex.Match(token, "mem\[(\d.*)\] = (\d.*)") 
                        let offset = int m.Groups.[1].Value
                        let value = int m.Groups.[2].Value
                        let head = instructions.Head
                        let mask = (fst head)
                        let previous_instructs = (snd head)
                        (mask,  previous_instructs @ [(offset, value)]) :: instructions.Tail                      
        ) []


// formula: (0 | ONES) & ~(ZEROS)
// let apply prevMap (input: list<int> * list<int> * list<int * int>) =
//     input
//         |> Seq.map(fun (zeros, ones, input) ->
//             let offset = fst input
//             let value = snd input

//             let apply_ones_results = Seq.fold (fun output x -> output ||| x ) value zeros
//             let apply_both_results = Seq.fold (fun output x -> output &&& ~~~(x)) apply_ones_results ones
//             (offset, apply_both_results)
//         )
//             |> Seq.fold (fun (map: Map<int,int>) (off, v) -> map.Add (off, v)) prevMap

// let first (a, _, _) = a
// let second (_,b_,c) = b
// let third (_, _, c) = c


let apply (prevMap: Map<int,int>) (input: (list<int> * list<int>) * list<int * int>) =
    let zeros = fst (fst input)
    let ones = snd (fst input)
    let instructions = snd input
    instructions
        |> Seq.map(fun input ->
            let offset = fst input
            let value = snd input
            let apply_ones_results = Seq.fold (fun output x -> output ||| x ) value ones
            let apply_both_results = Seq.fold (fun output x -> output &&& ~~~(x)) apply_ones_results zeros
            (offset, apply_both_results)
        )
            |> Seq.fold (fun (map: Map<int,int>) (off, v) -> map.Add (off, v)) prevMap


// snd parse_instructions.[0]

parse_instructions
    |> Seq.fold (fun map item -> apply map item) Map.empty 
    |> Seq.fold (fun sum resultsMap -> sum + resultsMap.Value) 0
