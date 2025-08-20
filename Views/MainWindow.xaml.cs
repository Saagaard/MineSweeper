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

namespace MineSweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int Rows = 10;
        private const int Columns = 10;

        Random random = new Random();
        bool gameOver = false;

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
            generateMines(10);
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
                        Tag = (row, col)
                    };

                    Grid.SetRow(button, row);
                    Grid.SetColumn(button, col);

                    button.Click += (sender, args) => checkField(button);

                    fieldGrid.Children.Add(button);
                }
            }
        }

        void checkField(Button button)
        {

            var (row, column) = ((int, int))button.Tag;

            if (mineFields[row, column] == 1)
            {
                endGame();
            }

            int adjacentMines = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
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

            button.Style = (Style)FindResource("buttonPressed");
            button.Content = adjacentMines >= 0 ? adjacentMines.ToString() : "";
        }

        void generateMines(int mineAmount)
        {
            int minesGenerated = 0;

            while (minesGenerated < mineAmount)
            {
                int newMineRow = random.Next(0, mineFields.GetLength(0));
                int newMineColumn = random.Next(0, mineFields.GetLength(1));
                Console.WriteLine("Position: " + newMineRow + "," + newMineColumn);

                if (mineFields[newMineRow, newMineColumn] == 0)
                {
                    mineFields[newMineRow, newMineColumn] = 1;
                    //Console.WriteLine("Mine generated");
                    minesGenerated++;
                }
            }
        }

        void endGame()
        {
            gameOver = true;
            textGameOver.Visibility = Visibility.Visible;
            //Console.WriteLine("You stepped on a mine. You lose!!!!");
        }

    }
}
