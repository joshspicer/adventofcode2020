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
            | '1' -> parse_mask (idx - 1)  zeroes_arr   ( (2F ** (float32 (rawMask.Length - idx - 1)) ) :: ones_arr)
            | '0' -> parse_mask (idx - 1)  ( (2F ** (float32 (rawMask.Length - idx - 1)) )  :: zeroes_arr)   ones_arr
            | _   -> parse_mask (idx - 1)  zeroes_arr   ones_arr

let mask = parse_mask (rawMask.Length - 1) [] []

let instruction =
    rawInput.Tail
            |> Seq.map(fun x -> 

                let m = Regex.Match(x, "mem\[(\d.*)\] = (\d.*)") 
                let offset = m.Groups.[1]
                let value = m.Groups.[2]
                (int offset.Value, int value.Value))
                    |> Seq.toList
