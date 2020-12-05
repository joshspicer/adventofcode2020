import Foundation


let data = try String(contentsOfFile: "./input", encoding: .utf8)
let seats = data.components(separatedBy: .newlines)

var highestSeat = 0
var allSeats = [Int]()
for seat in seats {

    let mid = seat.index(seat.startIndex, offsetBy: 7)

    let first = String(seat[seat.startIndex..<mid])
    // print(first)
    let second = String(seat[mid..<seat.endIndex])
    // print(second)

    var fMask = Int(127)
    var sMask = Int(7)

    //FBFBBFF
    //0101100
    for (idx, char) in first.enumerated() {
        if (char == "F") {
            let exp = (2 << (6 - 1 - (idx)))
            fMask = fMask & (127 - exp)
        }
    }

    // RLR
    // 101
    for (idx, char) in second.enumerated() {
        if (char == "L") {
            let exp = (2 << (2 - 1 - (idx)))
            sMask = sMask & (7 - exp)
        }
    }

    let uniqueID = fMask * 8 + sMask
    allSeats.append(uniqueID)
    highestSeat = max(uniqueID, highestSeat)
}

print(highestSeat)

// Part 2
allSeats.sort()
var prev = 69 
for s in allSeats {
    // print(s)
    if (prev != s - 1) {
        print(s - 1)
        break
    }
    prev = s
}
