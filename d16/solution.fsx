open System
open System.IO
open System.Text.RegularExpressions

let fileName = "input"
let rawInput = File.ReadAllText($"d16/{fileName}").Split("\n\n") |> Seq.toList
let rawRules = rawInput.[0].Split("\n")
let rawMyTicket = rawInput.[1].Split("\n").[1].Split(",") |> Seq.map int |> Seq.toList
let nearbyTickets = rawInput.[2].Split("\n") 
                            |> Seq.tail 
                            |> Seq.map (fun x -> x.Split(",")) 
                            |> Seq.map (Seq.map int >> Seq.toList)
                            |> Seq.toList

type Rule = 
    { 
      Name: string; 
      Min1: int;
      Max1: int;
      Set1: int Set;
      Min2: int;
      Max2: int;  
      Set2: int Set;
    }
 
// type Ticket = 
//     {
        
//     }

let rules_parsed = 
    rawRules
        |> Seq.map (fun x ->
                let m = Regex.Match(x, "(.*): (\d*)-(\d*) or (\d*)-(\d*)")
                let r = {
                         Name = m.Groups.[1].Value;
                         Min1 = int m.Groups.[2].Value;
                         Max1 = int m.Groups.[3].Value;
                         Min2 = int m.Groups.[4].Value;
                         Max2 = int m.Groups.[5].Value;
                         Set1 = Set.empty;
                         Set2 = Set.empty;
                        }
                r
        )
        |> Seq.toList
 
let generalize_bounds_as_set (input: Rule list) = 
    input
      |> 
        Seq.map (fun x -> 
          let set1 = [x.Min1..x.Max1]
                    |> Seq.fold (fun (state: int Set) num -> state.Add num) Set.empty
          let set2 = [x.Min2..x.Max2]
                    |> Seq.fold (fun (state: int Set) num -> state.Add num) Set.empty
          {x with Set1 = set1; Set2 = set2}      
        )

let rules = generalize_bounds_as_set rules_parsed
let master_set = Seq.fold (fun acc x -> x.Set1 + x.Set2 + acc) Set.empty rules

let part1 nearby = 
    nearby
        |> Seq.fold(fun (acc: int list) nearbyTicket ->
            nearbyTicket
                |> Seq.fold (fun acc2 z ->
                    if not (master_set.Contains(z)) then
                        z :: acc2
                    else 
                        acc2
                ) acc      
        ) []


part1 nearbyTickets
    |> Seq.sum