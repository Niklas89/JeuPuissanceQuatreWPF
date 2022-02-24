using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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


namespace MyCanvasEvents
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Create a canvas
        Canvas MyCanvas = new Canvas();

        // Coin of the actual player
        Ellipse playerEllipse = new Ellipse();

        // Create a list of red ellipses
        List<Ellipse> redEllipseList = new List<Ellipse>(24);

        // Create a list of yellow ellipses
        List<Ellipse> yellowEllipseList = new List<Ellipse>(24);

        double ellipseTop = 0;
        double ellipseLeft = 0;
        char[,] board = new char[7,7];
        char actualPlayer = 'r';

        int h = 0; // horizontal spots
        int v = 0; // vertical spots

        public MainWindow()
        {
            InitializeComponent();
            MyCanvas.MaxWidth = 700;
            MyCanvas.MaxHeight = 700;
            //MyCanvas.Background = new SolidColorBrush(Colors.Azure);
            MyCanvas.Focusable = true;
            MyCanvas.Focus();
            //MyCanvas.Visibility = Visibility.Visible;
            MyCanvas.KeyDown += Canvas_KeyDown;
            this.Content = MyCanvas;
            drawGrid();
        }

        private void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            // Press the down keyboard arrow to make a coin go down, left - right keyboard arrows to go left and right
            switch (e.Key)
            {
                case Key.Down:
                    if(board[h, v] == 'r' || board[h, v] == 'y')
                    {
                        MessageBox.Show("There is already a coin here !");
                    } else
                    {
                        bool noOtherCoin = true;
                        while (noOtherCoin)
                        {
                            v++;
                            if (board[h, v] == 'r' || board[h, v] == 'y')
                            {
                                v--;
                                noOtherCoin = false;
                            } else if(v == 6)
                            {
                                ellipseTop += 100;
                                playerEllipse.SetValue(Canvas.TopProperty, ellipseTop);
                                noOtherCoin = false;
                            }
                            else
                            {
                                ellipseTop += 100;
                                playerEllipse.SetValue(Canvas.TopProperty, ellipseTop);
                            }
                        }
                        changePlayer();
                    }
                    break;

                case Key.Left:
                    if(h == 0)
                    {
                        MessageBox.Show("You can't go left !");
                    } else
                    {
                        h--;
                        ellipseLeft -= 100;
                        playerEllipse.SetValue(Canvas.LeftProperty, ellipseLeft);
                    }
                    break;

                case Key.Right:
                    if(h == 6)
                    {
                        MessageBox.Show("You can't go right !");
                    } else
                    {
                        h++;
                        ellipseLeft += 100;
                        playerEllipse.SetValue(Canvas.LeftProperty, ellipseLeft);
                    }
                    break;

            }
        }

        public void drawGrid()
        {

            var rectBorder = new Rectangle();
            rectBorder.Width = 700;
            rectBorder.Height = 700;
            rectBorder.Stroke = new SolidColorBrush(Colors.Black);
            rectBorder.StrokeThickness = 4;
            rectBorder.Fill = new SolidColorBrush(Colors.Aquamarine);
            MyCanvas.Children.Add(rectBorder);

            // Create a Horizontal Line List
            var myHorizontalLines = new List<Line>(6);

            // Create my horizontal lines
            for (int i = 1; i < 7; i++)
            {
                var lineItem = new Line
                {
                    Stroke = new SolidColorBrush(Colors.Black),
                    StrokeThickness = 4,
                    X1 = 0,
                    Y1 = i * 100,
                    X2 = 700,
                    Y2 = i * 100,
                };
                myHorizontalLines.Add(lineItem);
            }
            myHorizontalLines.ForEach(x => MyCanvas.Children.Add(x));

            // Create a Vertical Line List
            var myVerticalLines = new List<Line>(6);

            // Create my vertical lines
            for (int i = 1; i < 7; i++)
            {
                var lineItem = new Line
                {
                    Stroke = new SolidColorBrush(Colors.Black),
                    StrokeThickness = 4,
                    X1 = i * 100,
                    Y1 = 0,
                    X2 = i * 100,
                    Y2 = 700,
                };
                myVerticalLines.Add(lineItem);
            }
            myVerticalLines.ForEach(x => MyCanvas.Children.Add(x));
            startGame();
        }


        public void startGame()
        {
            
            for(int i = 0; i < 24; i++)
            {
                var redEllipse = new Ellipse
                {
                    Height = 100,
                    Width = 100,
                    Fill = new SolidColorBrush(Colors.Red),
                };
                redEllipseList.Add(redEllipse);
            }
            //redEllipseList.ForEach(x => MyCanvas.Children.Add(x));
            

            for (int i = 0; i < 24; i++)
            {
                var yellowEllipse = new Ellipse
                {
                    Height = 100,
                    Width = 100,
                    Fill = new SolidColorBrush(Colors.Yellow),
                };
                yellowEllipseList.Add(yellowEllipse);
            }
            //yellowEllipseList.ForEach(x => MyCanvas.Children.Add(x));

            if (redEllipseList.Any())
            {
                playerEllipse = redEllipseList.First();
                MyCanvas.Children.Add(playerEllipse);
                redEllipseList.Remove(redEllipseList.First());
            }
            else
            {
                MessageBox.Show("There are no more coins for the red player !");
            }
        }


        public void changePlayer()
        {
            if (actualPlayer == 'r')
            {
                board[h, v] = 'r';
                if (yellowEllipseList.Any())
                {
                    playerEllipse = yellowEllipseList.First();
                    MyCanvas.Children.Add(playerEllipse);
                    yellowEllipseList.Remove(yellowEllipseList.First());
                }
                else
                {
                    MessageBox.Show("There are no more coins for the yellow player !");
                }
                actualPlayer = 'y';
            }
            else if (actualPlayer == 'y')
            {
                board[h, v] = 'y';
                if (redEllipseList.Any())
                {
                    playerEllipse = redEllipseList.First();
                    MyCanvas.Children.Add(playerEllipse);
                    redEllipseList.Remove(redEllipseList.First());
                }
                else
                {
                    MessageBox.Show("There are no more coins for the red player !");
                }
                actualPlayer = 'r';
            }
            h = 0;
            v = 0;
            ellipseLeft = 0;
            ellipseTop = 0;

            // check if a player won
            for(int i=0; i<7; i++)
            {
                for(int j=0; j<7; j++)
                {
                    // Check if a player won vertically
                    if(j+3 < 7)
                    {
                        if (board[i, j] == 'r' && board[i, j+1] == 'r' && board[i, j+2] == 'r' && board[i, j+3] == 'r')
                        {
                            MessageBox.Show("Player red won !");
                        } else if (board[i, j] == 'y' && board[i, j + 1] == 'y' && board[i, j + 2] == 'y' && board[i, j + 3] == 'y')
                        {
                            MessageBox.Show("Player yellow won !");
                        }
                    }

                    // Check if a player won horizontally
                    if (i + 3 < 7)
                    {
                        if (board[i, j] == 'r' && board[i + 1, j] == 'r' && board[i + 2, j] == 'r' && board[i + 3, j] == 'r')
                        {
                            MessageBox.Show("Player red won !");
                        }
                        else if (board[i, j] == 'y' && board[i + 1, j] == 'y' && board[i + 2, j] == 'y' && board[i + 3, j] == 'y')
                        {
                            MessageBox.Show("Player yellow won !");
                        }
                    }

                    // Check if a player won diagonally from top left to bottom right
                    if (i + 3 < 7 && j + 3 < 7)
                    {
                        if (board[i, j] == 'r' && board[i + 1, j + 1] == 'r' && board[i + 2, j + 2] == 'r' && board[i + 3, j + 3] == 'r')
                        {
                            MessageBox.Show("Player red won !");
                        }
                        else if (board[i, j] == 'y' && board[i + 1, j + 1] == 'y' && board[i + 2, j + 2] == 'y' && board[i + 3, j + 3] == 'y')
                        {
                            MessageBox.Show("Player yellow won !");
                        }
                    }

                    // Check if a player won diagonally from top right to bottom left
                    if (i + 3 < 7 && j - 3 >= 0)
                    {
                        if (board[i, j] == 'r' && board[i + 1, j - 1] == 'r' && board[i + 2, j - 2] == 'r' && board[i + 3, j - 3] == 'r')
                        {
                            MessageBox.Show("Player red won !");
                        }
                        else if (board[i, j] == 'y' && board[i + 1, j - 1] == 'y' && board[i + 2, j - 2] == 'y' && board[i + 3, j - 3] == 'y')
                        {
                            MessageBox.Show("Player yellow won !");
                        }
                    }

                }
            } // end check player won

        }
    }
}
