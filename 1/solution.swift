// Find the two entries that sum to 2020 and then multiply those two numbers together.

import Foundation


let data = try String(contentsOfFile: "./input", encoding: .utf8)
let values = data.components(separatedBy: .newlines).map { Int($0)}

for x in values {
    for y in values {
        // Part 01
        if (x! + y!) == 2020 {
            let mult = x! * y!
            print(mult)
        }
        // Part 02
        for z in values {
            if (x! + y! + z!) == 2020 {
                let mult2 = x! * y! * z!
                print(mult2)
            }
        }
    }
}

