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

var slopes = [
    (1, 1),
    (3, 1), // Just run this for part 1
    (5, 1),
    (7, 1),
    (1, 2)
]

var product = 1
for (dx, dy) in slopes {
    var x = 0
    var y = 0
    var count = 0
    while (y < rows.count - 1) {
        x += dx
        y += dy

        // get row
        let row = rows[rows.index(rows.startIndex, offsetBy: y)]
        // If tree exists in the space mod length
        for tree in row {
            if (tree == (x % 31)) {
                count += 1
            }
        }
    }
    print("slope \(dx),\(dy) == \(count)")
    product *= count
}
print(product)

