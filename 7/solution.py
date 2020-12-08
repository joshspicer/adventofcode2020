#!/usr/bin/env python3

# mypy solution.py

import re
from typing import *
import pprint

filename = "input"
file = open(filename, "r")

re_1 = "^(.*) bags contain (.*).$"
re_2 ="(.*\d) (.*) (bags?)"

bags : Dict[str, List[Tuple[int, str]]]= {}

for line in file:
    v = re.match(re_1, line)
    if not v:
        print("No valid groups. Err.")
        exit(1)
    outer = v.groups()[0].strip()
    inner = []
    for i in v.groups()[1].split(","):
        r = re.match(re_2, i)
        if not r:
            continue 
        inner.append((int(r.groups()[0].strip()), r.groups()[1].strip()))
    bags[outer] = inner

def count_can_contain_a_valid_bag(valid_bags: List[str], count: int) -> int:
    for b in valid_bags:
        for outer, inner in bags.items():
            if any(b in innerBags for innerBags in inner) and outer not in valid_bags:
                valid_bags.append(outer)
                count += 1
    return count

print(count_can_contain_a_valid_bag(['shiny gold'], 0))

def count_num_bags_inside(bag: List[Tuple[int, str]], sum: int) -> int:
    for (num, name) in bag:
        # print(f"{num} {name}")
        inner_count = count_num_bags_inside(bags[name], 1)
        sum += num * inner_count
    return sum
    
print(count_num_bags_inside(bags["shiny gold"], 0))
