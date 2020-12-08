#!/usr/bin/env python3

import re
from typing import *
import pprint
import copy

filename = "input"
file = open(filename, "r")   

class Entry:
    def __init__(self, idx: int, instruction: str, value: str):
        self.idx = idx
        self.instruction = instruction
        self.value = int(value)
        self.beenSeen = False
    
    def __str__(self) -> str:
        return f"[{self.idx}]{self.instruction} {self.value} {self.beenSeen}"

parsedEntries: List[Entry] = []
for idx, line in enumerate(file):
    split = line.strip().split(" ")
    e = Entry(idx, split[0], split[1])
    parsedEntries.append(e)

def execute(p2: bool, entries: List[Entry]):
    pc = 0
    acc = 0
    while True:
        # Set current to program counter
        current = entries[pc]

        # Been seen
        if current.beenSeen:
            if not p2:
                print(f"Loop Detected: pc={pc} acc={acc}")
            break
        else:
            current.beenSeen = True

        # Execute instruction
        I = current.instruction
        V = current.value

        if I == 'nop':
            pc += 1
        elif I == 'acc':
            acc += V
            pc += 1
        elif I == 'jmp':
            pc += V
    
        if p2 and pc == len(entries):
            print(f"Program Complete: idx={current.idx} acc={acc}")
            return

# Part 1
entries = copy.deepcopy(parsedEntries)
execute(False, entries)

# Part 2
for i in range(0, len(parsedEntries)):
    entries = copy.deepcopy(parsedEntries)  

    if entries[i].instruction == "jmp":
        entries[i].instruction = "nop"
    elif entries[i].instruction == "nop":
        entries[i].instruction = "jmp"

    execute(True, entries)