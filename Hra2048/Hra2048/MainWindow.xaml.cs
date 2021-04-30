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
using System.IO;
using System.Windows.Threading;

namespace Hra2048
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            winCasovac = new DispatcherTimer();
            winCasovac.Interval = TimeSpan.FromMilliseconds(10);
            winCasovac.Tick += WinCasovac_Tick;
        }

        byte alfa = 0;

        private void WinCasovac_Tick(object sender, EventArgs e)
        {
            if (alfa < 255)
            {
                alfa++;
            }
            else
            {
                winCasovac.Stop();
            }

            lblWin.Background = new SolidColorBrush(Color.FromArgb(alfa, 255, 255, 255));
            if (win)
            {
                lblWin.Content = "Vyhrál jsi, dosáhl jsi 2048\nSkóre : " + skore;
            }
            else
            {
                lblWin.Content = "Prohrál jsi, již nemáš žádné tahy\nSkóre : " + skore;
            }
        }

        DispatcherTimer winCasovac;

        Rectangle[,] rectangles = new Rectangle[4, 4];
        TextBlock[,] textBlocks = new TextBlock[4, 4];

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // generování hracího pole 4x4
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Rectangle rectangle = new Rectangle();
                    rectangle.Width = gridik.ActualWidth / 4;
                    rectangle.Height = gridik.ActualHeight / 4;
                    rectangle.Fill = new SolidColorBrush(Color.FromRgb(205, 193, 180));
                    rectangle.HorizontalAlignment = HorizontalAlignment.Left;
                    rectangle.VerticalAlignment = VerticalAlignment.Top;
                    rectangle.Margin = new Thickness(rectangle.Width * j, rectangle.Height * i, 0, 0);

                    rectangle.Stroke = Brushes.Black;
                    rectangle.StrokeThickness = 2;

                    gridik.Children.Add(rectangle);

                    TextBlock textBlock = new TextBlock();
                    textBlock.Width = gridik.ActualWidth / 4;
                    textBlock.Height = gridik.ActualHeight / 4;
                    textBlock.HorizontalAlignment = HorizontalAlignment.Left;
                    textBlock.VerticalAlignment = VerticalAlignment.Top;
                    textBlock.FontSize = 40;
                    textBlock.Margin = new Thickness(textBlock.Width * j, textBlock.Height * i + rectangle.Height / 2 - 30, 0, 0);
                    textBlock.TextAlignment = TextAlignment.Center;
                    
                    textBlock.Text = "";

                    gridik.Children.Add(textBlock);

                    rectangles[i, j] = rectangle;
                    textBlocks[i, j] = textBlock;
                }
            }

            // vygenerování prvních dvou kostek
            generateBlock(2);
            generateBlock(2);
        }
        int skore = 0;

        private void generateBlock(int hodnota = 0)
        {
            Random rnd = new Random();
            bool freeSpotExist = false;

            for (int i = 0; i < textBlocks.GetLength(0); i++)
            {
                for (int j = 0; j < textBlocks.GetLength(1); j++)
                {
                    if (textBlocks[i, j].Text == "")
                    {
                        freeSpotExist = true;
                    }
                }
            }
            if (!freeSpotExist)
            {
                gameOver();
                return;
            }

            int radek;
            int sloupec;
            do
            {
                radek = rnd.Next(4);
                sloupec = rnd.Next(4);
            } while (textBlocks[radek, sloupec].Text != "");
            if (hodnota == 0)
            {
                hodnota = (rnd.Next(2) + 1) * 2;
            }
            skore += hodnota;
            lblSkore.Content = "Skóre : " + skore;
            textBlocks[radek, sloupec].Text = Convert.ToString(hodnota);
        }

        private void gameWin()
        {
            //výhra
            win = true;
            winCasovac.Start();
            lblWin.Visibility = Visibility.Visible;
        }
        private void gameOver()
        {
            // prohra
            winCasovac.Start();
            lblWin.Visibility = Visibility.Visible;
        }

        bool win = false;

        private void MoveLeft()
        {
            //posun do leva musí jít po rádcích a pak sloupcích
            for (int i = 0; i < 4; i++)
            {
                for (int j = 1; j < 4; j++)
                {
                    int stopIndex = j;
                    for (int k = j-1; k >= 0; k--)
                    {
                        if (textBlocks[i, k].Text == "")
                        {
                            stopIndex = k;
                        }
                        else if (textBlocks[i, k].Text == textBlocks[i, j].Text)
                        {
                            stopIndex = k;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (stopIndex != j)
                    {
                        MoveBlock(i, j, i, stopIndex);
                    }
                }
            }
            generateBlock();
        }
        private void MoveRight()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 2; j >= 0; j--)
                {
                    int stopIndex = j;
                    for (int k = j + 1; k < 4; k++)
                    {
                        if (textBlocks[i, k].Text == "")
                        {
                            stopIndex = k;
                        }
                        else if (textBlocks[i, k].Text == textBlocks[i, j].Text)
                        {
                            stopIndex = k;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (stopIndex != j)
                    {
                        MoveBlock(i, j, i, stopIndex);
                    }
                }
            }
            generateBlock();
        }
        private void MoveUp()
        {
            for (int j = 0; j < 4; j++)
            {
                for (int i = 1; i < 4; i++)
                {
                    int stopIndex = i;
                    for (int k = i - 1; k >= 0; k--)
                    {
                        if (textBlocks[k, j].Text == "")
                        {
                            stopIndex = k;
                        }
                        else if (textBlocks[k, j].Text == textBlocks[i, j].Text)
                        {
                            stopIndex = k;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (stopIndex != i)
                    {
                        MoveBlock(i, j, stopIndex, j);
                    }
                }
            }
            generateBlock();
        }
        private void MoveDown()
        {
            for (int j = 0; j < 4; j++)
            {
                for (int i = 2; i >= 0; i--)
                {
                    int stopIndex = i;
                    for (int k = i + 1; k < 4; k++)
                    {
                        if (textBlocks[k, j].Text == "")
                        {
                            stopIndex = k;
                        }
                        else if (textBlocks[k, j].Text == textBlocks[i, j].Text)
                        {
                            stopIndex = k;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (stopIndex != i)
                    {
                        MoveBlock(i, j, stopIndex, j);
                    }
                }
            }
            generateBlock();
        }

        private void MoveBlock(int i, int j, int newI, int newJ)
        {
            //presunutí kostky
            if (textBlocks[newI, newJ].Text == "")
            {
                textBlocks[newI, newJ].Text = textBlocks[i, j].Text;
            }
            else
            {
                textBlocks[newI, newJ].Text = Convert.ToString(Convert.ToInt32(textBlocks[newI, newJ].Text) + Convert.ToInt32(textBlocks[i, j].Text));
                if (textBlocks[newI, newJ].Text == "2048")
                {
                    gameWin();
                }
            }
            textBlocks[i, j].Text = "";
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                MoveUp();
            }
            if (e.Key == Key.Down)
            {
                MoveDown();
            }
            if (e.Key == Key.Left)
            {
                MoveLeft();
            }
            if (e.Key == Key.Right)
            {
                MoveRight();
            }
        }

        private void btnLeft_Click(object sender, RoutedEventArgs e)
        {
            MoveLeft();
        }

        private void btnRight_Click(object sender, RoutedEventArgs e)
        {
            MoveRight();
        }

        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            MoveUp();
        }

        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            MoveDown();
        }

        Point mousePos;
        bool mysPohyb = false;

        private void gridik_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mousePos = e.GetPosition(gridik);
            mysPohyb = true;
        }

        private void gridik_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (mysPohyb)
            {
                Point newMousePos = e.GetPosition(gridik);
                double vzdalenostVodorovne = Math.Abs(newMousePos.X - mousePos.X);
                double vzdalenostSvisle = Math.Abs(newMousePos.Y - mousePos.Y);

                if (vzdalenostVodorovne < vzdalenostSvisle)
                {
                    if (newMousePos.Y - mousePos.Y > 0)
                    {
                        MoveDown();
                    }
                    else
                    {
                        MoveUp();
                    }
                }
                else
                {
                    if (newMousePos.X - mousePos.X > 0)
                    {
                        MoveRight();
                    }
                    else
                    {
                        MoveLeft();
                    }
                }

                mysPohyb = false;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.SaveFileDialog sFD = new System.Windows.Forms.SaveFileDialog();
            sFD.InitialDirectory = Directory.GetCurrentDirectory();
            sFD.Filter = "txt soubory (*.txt)|*.txt|Všechny soubory (*.*)|*.*";
            sFD.FileName = "hra2048_save.txt";

            if (sFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (StreamWriter writer = new StreamWriter(sFD.FileName))
                {
                    for (int i = 0; i < 4; i++)
                    {
                        string line = "";
                        for (int j = 0; j < 4; j++)
                        {
                            string hodnota = textBlocks[i, j].Text;
                            if (hodnota == "")
                            {
                                hodnota = "0";
                            }
                            if (j == 0)
                            {
                                line += hodnota;
                            }
                            else
                            {
                                line += "," + hodnota;
                            }
                        }
                        writer.WriteLine(line);
                    }
                }
            }
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog oFD = new System.Windows.Forms.OpenFileDialog();
            oFD.InitialDirectory = Directory.GetCurrentDirectory();
            oFD.Filter = "txt soubory (*.txt)|*.txt|Všechny soubory (*.*)|*.*";

            if (oFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        textBlocks[i, j].Text = "";
                    }
                }
                using (StreamReader reader = new StreamReader(oFD.FileName))
                {
                    for (int i = 0; i < 4; i++)
                    {
                        string line = reader.ReadLine();
                        string[] hodnoty = line.Split(',');

                        for (int j = 0; j < 4; j++)
                        {
                            if (hodnoty[j] != "0")
                            {
                                textBlocks[i, j].Text = hodnoty[j];
                            }
                        }
                    }
                }
            }
        }
    }
}
