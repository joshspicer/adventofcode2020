open System.IO
open System
open System.Text.RegularExpressions

let fileName = "example"
let msg = File.ReadAllText($"7/{fileName}")

let newline = Environment.NewLine
let separator = [|'\n'|]

"""
  outer        verb  quantity1      inner1  quantityX    innerX
    |             |    |             |     |             |
____|____       __|__  |  ___________|     | ____________|     
light red bags contain 1 bright white bag, 2 muted yellow bags, ...
                       |
                       |
                     | "no other bags."
"""

type Bag =
    struct
        val quantity: int
        val name: string
    end

type Rule =
    struct 
        val outerName: string
        val contents: Bag list // Empty list represents "contains no other bags"
    end


let instructions = 
    msg.Split separator

let outerRegex = new Regex("^(?<bag>.*) bags contain (?<rules>.*).$")
let outerMatch = outerRegex.Matches

instructions
|> Seq.map 
    (fun instr -> outerMatch instr)
