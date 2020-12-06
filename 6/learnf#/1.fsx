let toPigLatin (word: string) =
    let isVowel (c: char) =
        match c with
        | 'a' | 'e' | 'i' |'o' |'u'
        | 'A' | 'E' | 'I' | 'O' | 'U' -> true
        |_ -> false
    
    if isVowel word.[0] then
        word + "yay"
    else
        word.[1..] + string(word.[0]) + "ay"

let mutable a = 10
a <- 20

let items = [1..5]
let items2 : int list = [1..5]

let prefix prefixStr baseStr =
    prefixStr + ", " + baseStr
   
prefix "Hello" "World"

let names = ["Josh"; "Allison"; "Bob"]

let exclaim s = 
    s + "!"

let bigHello = (prefix "Hello") >> exclaim
let bigHelloFlipped = exclaim << (prefix "Hello")

names 
|> Seq.map (prefix "Hello") // Partial application (curried)
|> Seq.map exclaim
|> Seq.sort

// Refactors into...
// Seq.map ((prefix "Hello") >> exclaim) names 

// Lazy evaluation
names
|> Seq.map (fun x -> printfn "Mapped over %s" x; bigHello x)
|> Seq.sort

// unless you provide a Seq.iter!

names
|> Seq.map (fun x -> printfn "Mapped over %s" x; bigHello x)
|> Seq.sort
|> Seq.iter (printfn "%s")