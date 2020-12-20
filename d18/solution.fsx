open System
open System.IO
open System.Text.RegularExpressions

let fileName = "example"
let rawInput = File.ReadAllText($"d18/{fileName}").Split("\n") |> Seq.toList

type Operator = Add | Mult

type Token =
    | Op of Operator // leaf1
    | Number of int  // leaf2
    | OpenParen
    | CloseParen

type Node = {
    Value : Token
    Left : Tree
    Right : Tree
}
and Tree = 
    | Node of Node
    | Empty

let defaultTree = { Value = (Number -1); Left = Tree.Empty; Right = Tree.Empty }

let rec lexer (equation: char list) tokens = 
    match equation with
        | [] -> tokens
        | (l :: ls) -> match l with
                          | num when Regex.Match(string l, "[0-9]").Success -> 
                            lexer ls ((Token.Number (int (string num))) :: tokens)
                          | _ when Regex.Match(string l, "\+").Success -> lexer ls (Token.Op (Operator.Add) :: tokens)
                          | _ when Regex.Match(string l, "\*").Success -> lexer ls (Token.Op (Operator.Mult) :: tokens)
                          | _ when Regex.Match(string l, "\(").Success -> lexer ls (Token.OpenParen :: tokens)
                          | _ when Regex.Match(string l, "\)").Success -> lexer ls (Token.CloseParen :: tokens)
                          | _ -> lexer ls tokens // Skip
let input = rawInput.[0] |> Seq.toList

let rec parser (tokens: Token list) (currentTree: Tree) (stack: List<Tree>): Tree =
    match tokens with
        | [] -> currentTree
        | (t :: ts) -> 
            match t with
                | Number _ ->  parser 
                                    ts 
                                    (Tree.Node { Value = t; Left = Tree.Empty; Right = Tree.Empty }) 
                                    stack
                
                | Op _ ->    (Tree.Node { Value = t; Left = (parser ts Tree.Empty stack) ; Right = currentTree })
                                    

let myAST = parser (lexer input []) Tree.Empty []

// --------------------------------------------------
let rec leftmost tree = 
    match tree with
        |  Node n when n.Left = Tree.Empty -> tree
        |  Node n -> leftmost n.Left 

let rec rightmost tree = 
    match tree with
        |  Node n when n.Right = Tree.Empty -> tree
        |  Node n -> rightmost n.Right 

let getLeft tree = 
    match tree with 
        | Node n -> n.Left
        | Empty _ -> Tree.Empty

let getRight tree = 
    match tree with 
        | Node n -> n.Right
        | Empty _ -> Tree.Empty

let getRootValue tree =
    match tree with
        | Node n -> n.Value     
        | Empty -> failwith "ah"

let mapOperator op =
 match op with 
                | Add -> (+)
                | Mult -> (*)
                | _ -> failwith "noooo"
let left = leftmost myAST   
let right = rightmost myAST 
// --------------------------------------------------

let rec solve (ast: Tree): int =
    let rootValue = getRootValue ast
    let leftTree = getLeft ast
    let rightTree = getRight ast

    match (rootValue, leftTree, rightTree) with 
        | (Op o, Node lft, Node rgt) -> (mapOperator o) (solve leftTree) (solve rightTree)
        | (Number n, _, _) -> n 


solve myAST