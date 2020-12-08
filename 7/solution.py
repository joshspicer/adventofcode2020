#!/usr/bin/env python3

import re
import typing
import pprint

filename = "input"
file = open(filename, "r")

re_1 = "^(.*) bags contain (.*).$"
re_2 ="(.*\d) (.*) (bags?)"

bags = {}
for line in file:
    v = re.match(re_1, line)
    outer = v.groups()[0].strip()
    inner = []
    for i in v.groups()[1].split(","):
        r = re.match(re_2, i)
        if not r:
            continue 
        inner.append((r.groups()[0].strip(), r.groups()[1].strip()))
    bags[outer] = inner

def count_can_contain_a_valid_bag(valid_bags, count):
    for b in valid_bags:
        for outer, inner in bags.items():
            if any(b in innerBags for innerBags in inner) and outer not in valid_bags:
                valid_bags.append(outer)
                count += 1
    return count

print(count_can_contain_a_valid_bag(['shiny gold'], 0))