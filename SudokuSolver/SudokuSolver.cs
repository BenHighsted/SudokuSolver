﻿using System;

/** 
 Plan is to make it text based first, then develop a nice GUI simulation of the algorithm working
*/
namespace SudokuSolver
{
    class Solver
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Program running...");

            //when I create unit tests, this will be the basic format
            //theres a minimum number of clues required for it to have a single solution
            //will need to make sure I follow that in the unit tests
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

            //bool deez = validatePlacement(grid, 0, 0);
            //Console.WriteLine(deez);

            Solve(grid, 0, 0);
        }

        //thinking the best solution to this problem will be with a recursive backtracking algorithm
        static void Solve(int[,] grid, int row, int col) 
        {
            //Check column
            if (col == 9) {
                row++;
                col = 0;
            }

            //Check row
            if (row == 9) {
                //If it gets in here, the algorithm is complete! 
                Console.WriteLine("Complete!");
                return;
            }

            //Check if value already exists
            if (grid[row, col] != 0) {
                //One of the game clues, no need to place anything here.
                Solve(grid, row, col + 1);
            }

            //Trys placing values, if a placement validates, continue
            for (int i = 0; i < 9; i++) {
                grid[row, col] = i;
                if (validatePlacement(grid, row, col))
                {
                    //grid[row, col] = i;
                    Solve(grid, row, col + 1);
                }
            }

            Console.WriteLine("Error: Failed to place value");
            return;

        }

        static bool validatePlacement(int[,] grid, int row, int col) {
            //Check column
            for (int i = 0; i < 9; i++) {
                if (grid[col, row] == grid[i, row] && i != col) {
                    //Console.WriteLine("Column check failed");
                    return false;
                }
            }

            //Check row
            for (int i = 0; i < 9; i++) {
                if (grid[col, row] == grid[col, i] && i != row) {
                    //Console.WriteLine("Row check failed");
                    return false;
                }
            }

            //Check 3x3 (This is so garbage atm, these checks should all go in seperate methods soon
            int[] topLeft = new int[2]; // thinking i'll get the top left position and work out the values from there

            //This solution is temporary; will find a more effecient way to do this once the code is working
            int rowNum;
            if (row > -1 && row < 3) {
                rowNum = 0;
            } else if (row > 2 && row < 5) {
                rowNum = 1;
            }
            else {
                rowNum = 2;
            }

            int colNum;
            if (col > -1 && col < 3) {
                colNum = 0;
            }
            else if (col > 2 && col < 5) {
                colNum = 1;
            }
            else {
                colNum = 2;
            }

            int[,] positions = { { 0, 3, 6 }, { 1, 4, 7 }, { 2, 5, 8 } };
            //int[,] positions = { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 } };
            int pos = positions[rowNum, colNum];
            //Console.WriteLine(pos);

            if (pos == 0)
            {
                topLeft[0] = 0;
                topLeft[1] = 0;
            }
            else if (pos == 1)
            {
                topLeft[0] = 0;//column
                topLeft[1] = 3;//row
            }
            else if (pos == 2) {
                topLeft[0] = 0;
                topLeft[0] = 6;
            }
            else if (pos == 3)
            {
                topLeft[0] = 3;//column
                topLeft[1] = 0;//row
            }
            else if (pos == 4)
            {
                topLeft[0] = 3;
                topLeft[0] = 3;
            }
            else if (pos == 5)
            {
                topLeft[0] = 3;//column
                topLeft[1] = 6;//row
            }
            else if (pos == 6)
            {
                topLeft[0] = 6;
                topLeft[0] = 0;
            }
            else if (pos == 7)
            {
                topLeft[0] = 6;//column
                topLeft[1] = 3;//row
            }
            else if (pos == 8)
            {
                topLeft[0] = 6;
                topLeft[0] = 6;
            }

            if (!checkThreeByThree(grid, topLeft)) {
                //Console.WriteLine("Error: 3x3 check failed.");
                return false;
            }

            return true;
        }

        static bool checkThreeByThree(int[,] grid, int[] topLeft) {
            int col = topLeft[0];
            int row = topLeft[1];

            // if (top left value) == all other values - false. otherwise true.

            // incorrect atm: checks topLeft with all other values, need to check placed value with all other values instead.

            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    if (grid[col, row] == grid[col + i, row + j] && !(i == 0 && j == 0))
                    {
                       // Console.WriteLine("Failed: " + grid[col, row] + " " + grid[col + i, row + j]);
                        return false;
                    }
                   // Console.WriteLine(grid[col, row] + " " + grid[col + i, row + j]);
                }
            }
 
            return true;
        }

    }
}