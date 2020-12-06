open System.IO
open System

let fileName = "input"
let msg = File.ReadAllText($"6/{fileName}")

let newline = Environment.NewLine
let separator = [|'\n'|]

let items = 
    msg.Split separator

let revItems =  Seq.toList<string> items |> List.rev
let chunk = fun x y -> if y = "" then [] :: x else ([y] :: x.Head) :: x.Tail
let folded = List.fold chunk [[]] revItems

// Some cleanup
let accString (acc: string) (p:string) =
    acc + p
let extractStr family = List.fold accString "" family

folded
|> Seq.map(fun f -> 
                f |> Seq.map (fun p -> extractStr p)
                |> Seq.map (fun z -> (Set.ofSeq z ))
                |> Set.intersectMany
                |> Seq.length) |> Seq.sum