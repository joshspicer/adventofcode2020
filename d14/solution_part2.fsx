open System
open System.IO
open System.Text.RegularExpressions

let fileName = "input"
let rawInput = File.ReadAllText($"d14/{fileName}").Split("\n") |> Seq.toList

let parse_instructions mask_parser =
    rawInput
            |> Seq.fold(fun instructions token -> 
                    // Try to parse mask
                    
                    if token.Contains("mask") then
                        let mask = token.Split("=").[1].TrimStart()
                        let parsed_mask = mask_parser mask 35 [] [] []

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

// --- part 2 --- //
let rec parse_mask_part2 (string_mask: string) idx zeroes_arr ones_arr floating_arr = 
    // printfn "%d" idx
    if idx < 0 then
        (zeroes_arr, ones_arr, floating_arr)
    else
        match string_mask.[idx] with
            | '1' -> parse_mask_part2 string_mask (idx - 1)  
                                      zeroes_arr   
                                      (int64 (2F ** (float32 (string_mask.Length - idx - 1)) ) :: ones_arr) 
                                      floating_arr

            | '0' -> parse_mask_part2 string_mask (idx - 1)  
                                      (int64 (2F ** (float32 (string_mask.Length - idx - 1)) )  :: zeroes_arr) 
                                      ones_arr 
                                      floating_arr

            | 'X' -> parse_mask_part2 string_mask (idx - 1)  
                                      zeroes_arr
                                      ones_arr 
                                      (int64 (2F ** (float32 (string_mask.Length - idx - 1)) )  :: floating_arr) 

            | _   -> failwith("invalid bitmask item")
 

let getA (a,_,_) = a
let getB (_,b,_) = b
let getC (_,_,c) = c

let allCombinations lst =
    let rec comb accLst elemLst =
        match elemLst with
        | h::t ->
            let next = [h]::List.map (fun el -> h::el) accLst @ accLst
            comb next t
        | _ -> accLst
    comb [] lst

let permute num (floatings: int64 list): int64 list  = 
    printfn "num: %d" num
    printfn "floatings: %A" floatings

    let minSimilar = Seq.fold (fun acc inp -> acc &&& ~~~inp) num floatings

    let floating_masks =
        let combos = allCombinations floatings
                         |> Seq.map (fun s -> Seq.sum s)
        combos 
            |> Seq.append [0L]
            |> Seq.toList
    
    printfn "f_masks: %A" floating_masks

    floating_masks
        |>
        Seq.fold (fun acc m -> 
                        [minSimilar ||| m] @ [minSimilar &&& ~~~m] @ acc
                  ) []  
                        |> Set.ofSeq
                        |> Seq.toList

let apply prevMap input =
    // let zeros = getA (fst input)
    let ones = getB (fst input)
    let floatings = getC (fst input)

    let instructions = snd input
    instructions
        |> Seq.map(fun input ->
            let offset = fst input
            let value = snd input

            // (1) - If the bitmask bit is 1, the corresponding memory address bit is overwritten with 1.
            let mutated = Seq.fold (fun output (x :int64) -> output ||| x ) offset ones
            // (2) - If the bitmask bit is X, the corresponding memory address bit is floating.
            let all_memory_addresses_to_set: int64 list = permute mutated floatings

            printfn "all_mem: %A" all_memory_addresses_to_set


            (all_memory_addresses_to_set, value)
        )
            |> Seq.fold (fun (resultsMap: Map<int64,int64>) (mem_addresses, v) -> 
                                                        mem_addresses
                                                            |> Seq.fold(fun rr addr -> 
                                                                        rr.Add (addr, v) 
                                                              ) resultsMap)
                                                              prevMap


// part2 
(parse_instructions parse_mask_part2)
    |> Seq.rev
    |> Seq.toList
    |> Seq.fold (fun map item -> apply map item) Map.empty 
    |> Seq.fold (
        fun sum resultsMap -> sum + resultsMap.Value) 0L