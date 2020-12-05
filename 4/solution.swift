import Foundation

func boundChecker(val: String, lower: Int, upper: Int) -> Bool {
    print("bound: \(val) between \(lower) - \(upper) ?")
    let num = Int(val)
    if (num != nil) {
        if (num! >= lower && num! <= upper) {
            return true
        }
    } 
    return false  
} 

let data = try String(contentsOfFile: "./input", encoding: .utf8)
let passports = data.components(separatedBy: "\n\n")

var req = [
    "byr",
    "iyr",
    "eyr",
    "hgt",
    "hcl",
    "ecl",
    "pid",
]

var opt = [
    "cid"
]

var validKeys = 0
var validPart02 = 0 

for passport in passports {
    let lines = passport.components(separatedBy: "\n")
    var fields = [String]()
    for line in lines {
        let tmp = line.components(separatedBy: " ")
        for t in tmp {
            fields.append(t)
        }
    }
    var requiredFields = 0
    var requiredPart02 = 0
    for field in fields {
        let split = field.components(separatedBy: ":")
        let key = split[0]
        let val = split[1]
        if (req.contains(key)) {
            requiredFields += 1
        }
        // Part 02
        switch(key) {

            case "byr":
                if (boundChecker(val: val, lower: 1920, upper: 2002)) {
                    requiredPart02 += 1
                }
                break

            case "iyr":
                if (boundChecker(val: val, lower: 2010, upper: 2020)) {
                    requiredPart02 += 1
                }
                break

            case "eyr":
                if (boundChecker(val: val, lower: 2020, upper: 2030)) {
                    requiredPart02 += 1
                }                    
                break

            case "hgt":
                let unit = val.suffix(2)
                let heightIdx = val.index(val.endIndex, offsetBy: -2)
                let height = String(val[val.startIndex..<heightIdx])
                print("\(val) -> \(unit) \(height)")
                if (unit == "cm") {
                    if (boundChecker(val: height, lower: 150, upper: 193)) {
                        requiredPart02 += 1
                    }
                }
                if (unit == "in") {
                    if (boundChecker(val: height, lower: 59, upper: 76)) {
                        requiredPart02 += 1
                    }
                }
                break

            case "hcl":
                let regex = try! NSRegularExpression(pattern: "#[a-f0-9]")
                let range = NSRange(location: 0, length: val.utf16.count)
                let maybeMatch = regex.firstMatch(in: val, options: [], range: range) != nil
                print("\(val) -> \(maybeMatch)")
                if (maybeMatch) {
                    requiredPart02 += 1
                }
                break

            case "ecl":
                let eyeColors: Set = ["amb", "blu", "brn", "gry", "grn", "hzl", "oth"]
                print("eyecolor -> \(val)")
                if (eyeColors.contains(val)) {
                    requiredPart02 += 1
                }
                break

            case "pid":
                let isNum = Int(val)
                let is9Digits = val.count == 9
                print("passportID -> \(val) isNum=(\(isNum ?? -1)) and is9Digits=(\(is9Digits))")
                if (isNum != nil && is9Digits) {
                    requiredPart02 += 1
                }
                break

            case "cid":
                // Ignored              
                break

            default:
                continue

        }


    }
    if (requiredFields == req.count) {
        validKeys += 1

        if (requiredPart02 == req.count) {
            validPart02 += 1
        }
    }
    
}

print(validKeys)
print(validPart02)