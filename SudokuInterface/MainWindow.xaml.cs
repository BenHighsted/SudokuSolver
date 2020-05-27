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
using System.Windows.Threading;

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

        public int[,] originalGrid = new int[9, 9];
        public int moves = 0;
        public bool running = false, difficult = false;

        public MainWindow()
        {
            InitializeComponent();
            UpdateGrid(grid);
        }

        public void StartClick(object sender, RoutedEventArgs e)
        {
            if (start.Content.Equals("Start"))
            {
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        int val = grid[i, j];
                        originalGrid[i, j] = val;
                    }
                }

                running = true;
                Solve(grid, 0, 0);
                running = false;

                stepCount.Text = "Solved in " + moves + " moves.";// Displays the number of moves made
                moves = 0;

                start.Content = "Reset";
            }
            else if (start.Content.Equals("Reset"))
            {
                UpdateGrid(originalGrid);
                start.Content = "Start";

                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        int val = originalGrid[i, j];
                        grid[i, j] = val;
                    }
                }
            }
        }

        void UpdateLabel(String val, Label label)
        {
            Task.Run(() =>
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (!val.Equals("0"))
                    {
                        label.Content = val;
                    }
                    else
                    {
                        label.Content = "";
                    }
                }), DispatcherPriority.Render);
                Thread.Sleep(10);
            });
        }

        public void LoadClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFile = new Microsoft.Win32.OpenFileDialog();

            openFile.DefaultExt = ".txt";
            openFile.Filter = "Text documents (.txt)|*.txt";

            Nullable<bool> result = openFile.ShowDialog();

            if (result == true)
            {
                String filename = openFile.FileName;
                openedFile.Text = "Opened file: " + filename;

                string[] file = System.IO.File.ReadAllLines(filename);
                CreateGridFromFile(file);

                start.Content = "Start";
            }
        }

        void HandleCheck(object sender, RoutedEventArgs e) 
        {
            difficult = true;
        }

        void HandleUnchecked(object sender, RoutedEventArgs e) 
        {
            difficult = false;
        }

        public void CreateGridFromFile(String[] file)
        {
            int[,] newGrid = grid;

            for (int i = 0; i < 9; i++)
            {
                String[] row = file[i].Split(" ");

                for (int j = 0; j < 9; j++)
                {
                    newGrid[i, j] = int.Parse(row[j]);
                }
            }

            UpdateGrid(newGrid);
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
                        //label.Content = val.ToString();
                       if (running == true && difficult == false)
                        {
                            UpdateLabel(val.ToString(), label);
                        }
                        else {
                            label.Content = val.ToString();
                        }
                    }
                    else
                    {
                        //label.Content = "";
                        if (running == true && difficult == false)
                        {
                            UpdateLabel("", label);
                        }
                        else {
                            label.Content = "";
                        }
                    }
                }
            }
        }

        /** might move these to a libary later but for now keeping them here */
        bool Solve(int[,] grid, int row, int col)
        {
            if (!difficult) 
            {
                UpdateGrid(grid);
            }

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
                moves++;
                return Solve(grid, row + 1, col);
            }

            //try placing values
            for (int i = 1; i <= 9; i++)
            {
                if (ValidatePlacement(grid, row, col, i))
                {
                    moves++;
                    grid[row, col] = i;

                    if (!difficult)
                    {
                        UpdateGrid(grid);
                    }

                    if (Solve(grid, row + 1, col))
                    {
                        UpdateGrid(grid);
                        return true;
                    }
                }
            }

            if (!difficult) 
            {
                UpdateGrid(grid);
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
