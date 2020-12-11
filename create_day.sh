#!/bin/bash

NUM_ARGS=2
if [ "$#" -ne $NUM_ARGS ]; then
    echo "[-] Usage: ./create_day.sh day-number language-ext"
    exit 1
fi

mkdir ./$1
touch ./$1/example
touch ./$1/input
touch ./$1/solution.$2
touch ./$1/README.md