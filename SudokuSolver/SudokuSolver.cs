using System;

/** 
 Plan is to make it text based first, then develop a nice GUI simulation of the algorithm working
*/
namespace SudokuSolver
{
    class Solver
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Program running...\n");

            //when I create unit tests, this will be the basic format
            //theres a minimum number of clues required for it to have a single solution
            //will need to make sure I follow that in the unit tests (I believe 17 clues are required(

            int[,] grid = {{ 0, 0, 0, 2, 6, 0, 7, 0, 1 },
                           { 6, 8, 0, 0, 7, 0, 0, 9, 0 },
                           { 1, 9, 0, 0, 0, 4, 5, 0, 0 },
                           { 8, 2, 0, 1, 0, 0, 0, 4, 0 },
                           { 0, 0, 4, 6, 0, 2, 9, 0, 0 },
                           { 0, 5, 0, 0, 0, 3, 0, 2, 8 },
                           { 0, 0, 9, 3, 0, 0, 0, 7, 4 },
                           { 0, 4, 0, 0, 5, 0, 0, 3, 6 },
                           { 7, 0, 3, 0, 1, 8, 0, 0, 0 }
            };

            Solve(grid, 0, 0);

            PrintGrid(grid);
        }

        /*
         A recursive method to solve the Sudoku puzzle.
         Uses a backtracking algorithm to do this.
         */
        static bool Solve(int[,] grid, int row, int col)
        {
            if (row == 9) {//end of row (moves down 1 row)
                row = 0;
                col++;
                if (col == 9) {//complete
                    return true;
                }
            }

            if (grid[row, col] != 0) {//clue value, so move on
                return Solve(grid, row + 1, col);
            }

            for (int i = 1; i <= 9; i++) {
                if(ValidatePlacement(grid, row, col, i))
                {
                    grid[row, col] = i;
                    if(Solve(grid, row + 1, col))
                    {
                        return true;
                    }
                }
            }

            grid[row, col] = 0;
            return false;

        }

        /* This method checks if the value can be placed in the current position */
        static bool ValidatePlacement(int[,] grid, int row, int col, int newVal)
        {
            //Check column
            for (int i = 0; i < 9; i++)
            {
                if (grid[row, i] == newVal)
                {
                    return false;
                }
            }

            //Check row
            for (int i = 0; i < 9; i++)
            {
                if (grid[i, col] == newVal)
                {
                    return false;
                }
            }

            //Calculates where the top left of the 3x3 is
            int[] topLeft = new int[2];
            topLeft[0] = (row / 3) * 3;
            topLeft[1] = (col / 3) * 3;

            //Checks the 3x3
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (newVal == grid[topLeft[0] + i, topLeft[1] + j])
                    {
                        return false;
                    }
                }
            }

            //If it makes it here, the value is able to be placed
            return true;
        }

        static void PrintGrid(int[,] grid)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Console.Write(grid[i, j]);
                }
                Console.WriteLine();
            }
        }

    }
}
