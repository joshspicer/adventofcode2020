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
                {
                     Name = m.Groups.[1].Value;
                     Min1 = int m.Groups.[2].Value;
                     Max1 = int m.Groups.[3].Value;
                     Min2 = int m.Groups.[4].Value;
                     Max2 = int m.Groups.[5].Value;
                     Set1 = Set.empty;
                     Set2 = Set.empty;
                 }
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

let rules = generalize_bounds_as_set rules_parsed |> Seq.toList
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


/// part 2
let rec filter_invalid_tickets tickets results =
    match tickets with
            | [] -> results
            | (x :: xs) -> 
                let hasValidNum = Seq.forall (fun t -> master_set.Contains(t)) x
                if hasValidNum then 
                    filter_invalid_tickets xs (x :: results)
                else 
                    filter_invalid_tickets xs results

let nearby_tickets_2 = filter_invalid_tickets nearbyTickets []

let rec check_fields nums (rules: list<Rule>) =
    match nums with 
        | [] -> Seq.map(fun z -> z.Name) rules
        | (n :: ns) -> 
            let newRules = Seq.filter (fun r -> (r.Set1.Contains n) || (r.Set2.Contains n) ) rules |> Seq.toList
            check_fields ns newRules 

let check_all_fields idx = 
    let nums = 
        Seq.map (fun (tt: list<int>) -> tt.[idx]) nearby_tickets_2 
                |> Seq.toList
    let fields = check_fields nums rules |> Seq.toList |> Set.ofList
    (idx, fields)

let possibilities = 
    [0..(nearby_tickets_2.[0].Length - 1)]
                    |> Seq.map (fun x -> check_all_fields x)
                   
    |> Seq.toList

let rec assignFields (sorted: list<int * Set<string>>) (assigned: Set<string>) (result: list<int * string>) =
    match sorted with 
        | [] -> result
        | ((idx, s) :: ts) -> 
            if s.Count = 1 then
                assignFields 
                    ts 
                    (assigned.Add(s.MaximumElement))
                    ((idx, s.MaximumElement) :: result)
            else
                let diff = 
                    s - assigned
                if diff.Count = 1 then
                  assignFields 
                    ts 
                    (assigned.Add(diff.MaximumElement))
                    ((idx, diff.MaximumElement) :: result)
                else
                    failwith "this problem is much easier if I dont hit this!"

let sorted = Seq.sortBy(fun (i, x: Set<string>) -> x.Count) possibilities |> Seq.toList
let assigned = assignFields sorted Set.empty [] 

assigned
    |> Seq.filter(fun (i, n) -> n.StartsWith("departure"))
    |> Seq.map (fun (i, n) -> i)
    |> Seq.map (fun i -> rawMyTicket.[i]) 
        |> Seq.map int64 
        |> Seq.reduce (*)