open System
open System.IO
let fileName = "example5"
let rawInput = File.ReadAllText($"d13/{fileName}").Split("\n") |> Seq.toList
let timeOffset = rawInput |> Seq.head |> int
let buses_part1 =
    let rawBus = rawInput.[1]
    rawBus.Split(",") |> Seq.filter (fun x -> not (x = "x")) |> Seq.map (fun x -> int x) |> Seq.toList

let part1 = 
    let p = buses_part1 |> Seq.map (fun x -> (x, ((timeOffset / x) + 1) * x - timeOffset)) |> Seq.minBy (fun m -> snd m)
    (fst p) * (snd p)

// part 2

let buses_part2 = 
    let rawBus = rawInput.[1]
    rawBus.Split(",") 
        |> Seq.mapi (fun tOffset item -> (tOffset, item)) 
        |> Seq.filter (fun x -> not (snd x = "x")) 
        |> Seq.map (fun x -> (int64 (fst x), (int64 (snd x))))
        |> Seq.toList

let maxBus = 
    buses_part2
        |> Seq.maxBy (fun (x, y) -> y)

let absoluteOffset = fst maxBus
let maxDivisor = snd maxBus
let relativeBusNumbers = 
    buses_part2
        |> Seq.map (fun (x,y) -> ((x - absoluteOffset), y))
        |> Seq.toList
        // We're checking in chunks relative in time to the largest divisor (for the example: 59)
        // So we can remove that one from the list
        // And order by descending bus numbers as an optimization, checking the largest divisors and 
        // failing if any in that chain don't work
        |> Seq.sortByDescending (fun (x,y) -> y)
        |> Seq.toList
        |> function
            | (x :: xs) -> xs
            | _ -> failwith ("nope!") 

let rec part2 baseDivisor currTime (schedule: list<int64 * int64>): int64 = 
    let mutable idx = 0
    // Only do the modulo operations we need to do, starting with the least likely first (largest divisors)
    // fst --> offset; snd --> busNum
    while ( (idx < schedule.Length) && (currTime + fst (schedule.[idx])) % (snd (schedule.[idx])) = 0L) do
        // printfn "idx: %d" idx
        // printfn "currTime: %d" currTime
        // printfn "len: %d\n ---" schedule.Length
        // printfn "ANS: %d" (currTime + smallestAbsOffset)
        idx <- idx + 1

    if (idx = schedule.Length) then
       // Get the earliest of all the timestamps
       // aka: the smallest absolute offset in schedule
       let smallestAbsOffset = schedule |> Seq.minBy (fun (x,y) -> x) |> fst
       currTime + smallestAbsOffset
    else
        // Not everything was properly divisible
        // Do a simple addition to increment to the next largest divisor that could work
        part2 baseDivisor (currTime + baseDivisor) schedule


// do the thing.
part2 maxDivisor maxDivisor relativeBusNumbers