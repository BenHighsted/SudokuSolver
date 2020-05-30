# Sudoku Solver

I am creating a C# WPF application that solves Sudoku puzzles.

## Installation

The files are for a Visual Studio project so simply download it and open it in Visual Studio. 

As this is a WPF application, it will only work on Windows.

## Usage

Simply run the Visual Studio project, then click the 'Load' button to load a file (see formatting below) and then click the 'Start' button. If you do not have a file and wish to see it in action, a default puzzle will be loaded in automatically. Also included in the files are two examples of an easy puzzle and the worlds hardest puzzle.

File Format needs to be a 9x9 grid of numbers, with a space between each number, and a new line every 9 numbers. For spaces that don't have a number yet, use zero instead of a space.
```bash
0 0 0 0 0 1 2 3 0
1 2 3 0 0 8 0 4 0
8 0 4 0 0 7 6 5 0
7 6 5 0 0 0 0 0 0
0 0 0 0 0 0 0 0 0
0 0 0 0 0 0 1 2 3
0 1 2 3 0 0 8 0 4
0 8 0 4 0 0 7 6 5
0 7 6 5 0 0 0 0 0
```

If the puzzle is complex, it is recommended that you leave the 'Difficult Puzzle' checkbox marked, as the program tends to hang for long amounts of time on puzzles with thousands of moves.
