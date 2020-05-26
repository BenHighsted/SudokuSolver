using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Diagnostics;
using SudokuSolver;
using System.Threading;
using System.ComponentModel;

/** 
 * 
 * Thinking because of the way I want it to update in real time, I might need to use windows forms.
 * Probably need to do more research before I make the switch.
 * 
 */

namespace SudokuInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int[,] grid = {{ 0, 0, 0, 2, 6, 0, 7, 0, 1 },
                              { 6, 8, 0, 0, 7, 0, 0, 9, 0 },
                              { 1, 9, 0, 0, 0, 4, 5, 0, 0 },
                              { 8, 2, 0, 1, 0, 0, 0, 4, 0 },
                              { 0, 0, 4, 6, 0, 2, 9, 0, 0 },
                              { 0, 5, 0, 0, 0, 3, 0, 2, 8 },
                              { 0, 0, 9, 3, 0, 0, 0, 7, 4 },
                              { 0, 4, 0, 0, 5, 0, 0, 3, 6 },
                              { 7, 0, 3, 0, 1, 8, 0, 0, 0 }
        };

        public MainWindow()
        {
            InitializeComponent();
            UpdateGrid(grid);
        }

        public void StartClick(object sender, RoutedEventArgs e) {
            int[,] newGrid = grid;

            UpdateGrid(newGrid);
            Solve(newGrid, 0, 0);

            UpdateGrid(newGrid);

            start.IsEnabled = false;
        }

        void UpdateGrid(int[,] grid)
        {
            int count = 0;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    int val = grid[i, j];

                    String labelName = "label" + (++count);
                    Label label = (Label)FindName(labelName);
                    if (val != 0)
                    {
                        label.Content = val.ToString();

                    }
                    else
                    {
                        label.Content = "";
                    }
                }
            }

        }

        /** might move these to a libary later but for now keeping them here */
        bool Solve(int[,] grid, int row, int col)
        {
            if (row == 9)
            {//end of row (moves down 1 row)
                row = 0;
                col++;
                if (col == 9)
                {//complete
                    return true;
                }
            }

            if (grid[row, col] != 0)
            {//clue value, so move on
                return Solve(grid, row + 1, col);
            }

            //try placing values
            for (int i = 1; i <= 9; i++)
            {
                if (ValidatePlacement(grid, row, col, i))
                {
                    grid[row, col] = i;
                    if (Solve(grid, row + 1, col))
                    {
                        UpdateGrid(grid);
                        return true;
                    }
                }
            }

            grid[row, col] = 0;
            return false;

        }

        /* This method checks if the value can be placed in the current position */
        bool ValidatePlacement(int[,] grid, int row, int col, int newVal)
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
    }
}
