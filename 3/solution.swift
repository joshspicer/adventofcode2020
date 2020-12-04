import Foundation

let data = try String(contentsOfFile: "./input", encoding: .utf8)
let values = data.components(separatedBy: .newlines)

// Array for each row storing indices of trees (0-index)
var rows = [[Int]]()

func parse() {
    for inputRow in values {
        var row = [Int]()
        for (idx, char) in inputRow.enumerated() {
            if (char == "#") {
                row.append(idx)
            }
        }
        rows.append(row)
    }
}

parse()

// Count all the trees you would encounter for the slope right 3, down 1:
var x = 0
var y = 0
var count = 0
while (y < rows.count - 1) {
    x += 3
    y += 1

    // get row
    let row = rows[rows.index(rows.startIndex, offsetBy: y)]
    // If tree exists in the 
    for tree in row {
        if (tree == (x % 31)) {
            count += 1
        }
    }
}

print(count)

