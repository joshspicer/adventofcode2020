// Valid Passwords

import Foundation

// 4-9 m: xvrwfmkmmmc
struct PasswordItem {
    var min: Int
    var max: Int
    var letter: String
    var password: String
}

extension String {
    func subString(from: Int, to: Int) -> String {
       let startIndex = self.index(self.startIndex, offsetBy: from)
       let endIndex = self.index(self.startIndex, offsetBy: to)
       return String(self[startIndex...endIndex])
    }
}

func countLetters(str:String, char:Character) ->Int {
    let letters = Array(str); var count = 0
    for letter in letters {
        if letter == char {
            count += 1
        }
    }
    return count
}

let data = try String(contentsOfFile: "./input", encoding: .utf8)
let values = data.components(separatedBy: .newlines)
var items = [PasswordItem]()
// 4-9 m: xvrwfmkmmmc
for val in values {
    // (4-9) (m:) (xvsjlkj)
    let groups = val.components(separatedBy: " ")
    // (4) (9)
    let range = groups[0].components(separatedBy: "-")
    // m
    let letter = groups[1].subString(from: 0, to: 0)
    
    let finalItem = PasswordItem(min: Int(range[0])!, max: Int(range[1])!, letter: letter, password: groups[2])
    items.append(finalItem)
}

var valid = 0 
for passwd in items {
    let count = countLetters(str: passwd.password, char: Character(passwd.letter))
    if count >= passwd.min && count <= passwd.max {
        valid += 1
    }
}
print(valid)

// Part 2
// 1-3 a: abcde (exactly one of a_c__ must be an a)

var part2 = 0
for passwd in items {
    let firstIdx = passwd.min - 1
    let secondIdx = passwd.max - 1
    
    let fLetter = passwd.password.subString(from: firstIdx, to: firstIdx)
    let first = fLetter == passwd.letter

    let sLetter = passwd.password.subString(from: secondIdx, to: secondIdx)
    let second = sLetter == passwd.letter

    if ((first == true && second != true) || (first != true && second == true)) {
        part2 += 1
    }
}
print(part2)

