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

        /// <summary>
        /// This method is called when the user clicks the 'Start' button. It is used to run the methods that solve the puzzle.
        /// When it is pressed, the button is changed to 'Reset'. When pressed again, it will reset the button to the original state.
        /// </summary>
        /// <param name="sender">Contains a reference to the button 'Scan' that raises this event</param>
        /// <param name="e">Contains the event data</param>
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
                stepCount.Text = "The amount of moves will be displayed here...";

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

        /// <summary>
        /// This method is used to update the labels in real time by creating a new thread to handle it.
        /// </summary>
        /// <param name="val">The number to be added to the label</param>
        /// <param name="label">The label that needs updating</param>
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

        /// <summary>
        /// This method is called when the user clicks the 'Load' button. It is used to load in a file and set up the puzzle.
        /// </summary>
        /// <param name="sender">Contains a reference to the button 'Scan' that raises this event</param>
        /// <param name="e">Contains the event data</param>
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

        /// <summary>
        /// This method is called when the user checks the 'Difficult' box. It simplies sets the boolean value of difficult to true.
        /// </summary>
        /// <param name="sender">Contains a reference to the button 'Scan' that raises this event</param>
        /// <param name="e">Contains the event data</param>
        void HandleCheck(object sender, RoutedEventArgs e) 
        {
            difficult = true;
        }

        /// <summary>
        /// This method is called when the user checks the 'Difficult' box. It simplies sets the boolean value of difficult to true.
        /// </summary>
        /// <param name="sender">Contains a reference to the button 'Scan' that raises this event</param>
        /// <param name="e">Contains the event data</param>
        void HandleUnchecked(object sender, RoutedEventArgs e) 
        {
            difficult = false;
        }

        /// <summary>
        /// Updates the grid with the users selected puzzle
        /// </summary>
        /// <param name="file">The puzzle that the user has selected using the load method</param>
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

        /// <summary>
        /// Updates the WPF labels to the new grid
        /// </summary>
        /// <param name="grid">The grid that the labels have to update from</param>
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

        /// <summary>
        /// This method uses recursive backtracking to solve the puzzle.
        /// </summary>
        /// <param name="grid">The current state of the grid</param>
        /// <param name="row">The row that we are currently trying to place the value on</param>
        /// <param name="col">The column we are currently trying to place the value on</param>
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

        /// <summary>
        /// Checks all the conditions of a place value to check if it can 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="newVal"></param>
        /// <returns>True if the placement is valid, false if the placement is invalid</returns>
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
