open System.IO
open System

let fileName = "input"
let msg = File.ReadAllText($"6/{fileName}")

let separator = [|$"{Environment.NewLine}{Environment.NewLine}"|]
let pieces = 
    msg.Split(separator, System.StringSplitOptions.RemoveEmptyEntries)

let polish (text:string) =
    text.Split([|$"{Environment.NewLine}"|], StringSplitOptions.RemoveEmptyEntries) |> String.Concat

pieces
|> Seq.map(fun p -> 
                (polish p |> Seq.distinct )) 
                                |> Seq.sumBy Seq.length
