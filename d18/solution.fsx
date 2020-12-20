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

let rec parser (tokens: Token list) (ast: Tree) =
    if tokens = [] then
        ast
    else   
        let newAST = 
            match tokens with
                | [] -> ast
                // | (t :: []) -> match t with
                //                   | 

                | (t :: ts) -> match t with
                                  | Op op -> 
                                        match ast with
                                          | Node n -> Tree.Node { Value = t; 
                                                                  Left = Tree.Empty; 
                                                                  Right =  Tree.Node n }
                                          | Empty -> failwith "illegal (1)"
                                  | Number num ->
                                         match ast with
                                          | Node n -> Tree.Node { n with Left = Tree.Node { Value = t; Left = Tree.Empty; Right = Tree.Empty} }
                                          | Empty -> Tree.Node { Value = t; 
                                                                 Left = Tree.Empty;
                                                                 Right = Tree.Empty }
                                //   | OpenParen ->
                                //   | CloseParen ->
                                  | _ -> ast
        parser tokens.Tail newAST

let myAST = parser (lexer input []) Tree.Empty


let rec leftmost tree = 
    match tree with
        |  Node n when n.Left = Tree.Empty -> tree
        |  Node n -> leftmost n.Left 

let rec rightmost tree = 
    match tree with
        |  Node n when n.Right = Tree.Empty -> tree
        |  Node n -> rightmost n.Right 

// leftmost myAST   
rightmost myAST 

// let rec solve (ast: Tree) (result: int) =
//     let left = leftmost ast
