using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Threading;

namespace MineSweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int Rows = 10;
        private const int Columns = 10; // Hard coded grid-size because lazy

        Random random = new Random();
        bool gameOver = false;

        int mineAmount;
        int checkedFieldsAmount = 0;

        private Stopwatch stopwatch;
        private DispatcherTimer timer;

        int[,] mineFields =
    {
                {0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0}
                };

        // To keep track of which buttons have been pressed
        int[,] checkedFields =
            {
                {0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0}
                };

        public MainWindow()
        {
            InitializeComponent();
            createGridButtons();
            mineAmount = random.Next(10, 16); // Between 10-15 mines
            generateMines(mineAmount);
            startTimer();
        }

        private void createGridButtons()
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    var button = new Button
                    {
                        Content = "",
                        Tag = (row, col) // Saving the grid position for future uses
                    };

                    Grid.SetRow(button, row);
                    Grid.SetColumn(button, col);

                    button.Click += (sender, args) => checkField(button);

                    fieldGrid.Children.Add(button);
                }
            }
        }

        void generateMines(int mineAmount)
        {
            int minesGenerated = 0;

            while (minesGenerated < mineAmount)
            {
                int newMineRow = random.Next(0, mineFields.GetLength(0));
                int newMineColumn = random.Next(0, mineFields.GetLength(1));
                //Console.WriteLine("Position: " + newMineRow + "," + newMineColumn);

                if (mineFields[newMineRow, newMineColumn] == 0)
                {
                    mineFields[newMineRow, newMineColumn] = 1;
                    //Console.WriteLine("Mine generated");
                    minesGenerated++;
                }
            }

            textMines.Text = "Number of mines: " + mineAmount.ToString();
        }

        void checkField(Button button)
        {
            if (gameOver) return;

            var (row, column) = ((int, int))button.Tag; // Takes the coordinates from the button's tag

            // Check if field has already been pressed/checked
            if (checkedFields[row, column] == 0)
            {
                //Check if game ends
                if (mineFields[row, column] == 1) // If mine is pressed
                {
                    button.Style = (Style)FindResource("minePressed");
                    button.Content = "M";
                    endGame(0);
                    return;
                }
                else
                {
                    checkedFieldsAmount = checkedFieldsAmount + 1;
                    if (checkedFieldsAmount >= (mineFields.GetLength(0) * mineFields.GetLength(1) - mineAmount))
                    {
                        endGame(1);
                    }
                }

                // Calculates the amount of adjacent mines
                int adjacentMines = 0;
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        // Check out-of-bounds
                        if (row + i < 0 || row + i >= mineFields.GetLength(0) || column + j < 0 || column + j >= mineFields.GetLength(1))
                        {
                            continue;
                        }

                        if (mineFields[row + i, column + j] == 1)
                        {
                            adjacentMines++;
                        }
                    }
                }

                checkedFields[row, column] = 1;
                button.Style = (Style)FindResource("buttonPressed");
                button.Content = adjacentMines >= 0 ? adjacentMines.ToString() : "";

            }
        }

        void endGame(int winLose)
        {
            if (winLose == 0)
            {
                gameOver = true;
                textGameOver.Visibility = Visibility.Visible;
                //Console.WriteLine("You stepped on a mine. You lose!!!!");
            } else
            {
                gameOver = true;
                textWon.Visibility = Visibility.Visible;
                //Console.WriteLine("You've won!");
            }

        }

        // Starts a timer/stopwatch
        private void startTimer()
        {
            stopwatch = Stopwatch.StartNew();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += updateTimer;
            timer.Start();
        }

        // Updates the UI label
        private void updateTimer(object sender, EventArgs e)
        {
            var timeElapsed = stopwatch.Elapsed;
            gameTimer.Content = $"Time: {timeElapsed.Minutes:D2}:{timeElapsed.Seconds:D2}";

            if (gameOver)
            {
                timer.Stop();
            }
        }

    }
}
