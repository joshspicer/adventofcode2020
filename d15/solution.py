from typing import *
import pprint

filename = "input"
inputs = map(lambda x: int(x), open(filename, "r").read().split(","))

"""
* Say all the starting number in order!
* If that was the first time the number has been spoken, the current player says 0.
* Otherwise, the number had been spoken before; the current player announces how many turns apart the number is from when it was previously spoken.
"""

lastSpokenOnTurn = {}
turn = 1
spoken = None
was_new = True

# Init
for num in inputs:
    spoken = num
    lastSpokenOnTurn[spoken] = (turn, None) #(lastTurn, prevLastTurn)
    turn += 1

part1 = 2020
part2 = 30000000

while turn < part2 + 1:
    # print("\n\nSPOKEN", spoken, "on", turn, "?", ".......",lastSpokenOnTurn)

    (lastTurn, prevLastTurn) = lastSpokenOnTurn[spoken]

    if prevLastTurn == None:
        spoken = 0
        # print ("IF","new [", spoken, "]=", turn, lastTurn)

    else:
        # Had been spoken before
        difference = lastTurn - prevLastTurn
        # print("ELSE", "diff=", lastTurn, "-", prevLastTurn, "=", difference)
        spoken = difference
    
    if spoken not in lastSpokenOnTurn.keys():
       lastSpokenOnTurn[spoken] = (None, None)

    (recyclePrevLast, _) = lastSpokenOnTurn[spoken]
    lastSpokenOnTurn[spoken] = (turn, recyclePrevLast)
    turn += 1
    
print(spoken)