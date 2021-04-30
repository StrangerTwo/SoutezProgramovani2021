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
using System.Windows.Threading;

namespace Semafor
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        DispatcherTimer casovac;
        public MainWindow()
        {
            InitializeComponent();
        }

        int stav = 1;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // nastavíme světla na vypnutá
            svetloCervena.Fill = Brushes.Black;
            svetloOranzova.Fill = Brushes.Black;

            // spustíme časovač
            casovac = new DispatcherTimer(DispatcherPriority.Send);
            casovac.Interval = TimeSpan.FromSeconds(5);
            casovac.Tick += Casovac_Tick;
            casovac.Start();


        }

        private void Casovac_Tick(object sender, EventArgs e)
        {
            // pokaždé se zvedne stav
            stav++;

            switch (stav)
            {
                case 1:
                    // 1 - svítí pouze zelená po 5s
                    svetloZelena.Fill = Brushes.Green;
                    svetloOranzova.Fill = Brushes.Black;
                    svetloCervena.Fill = Brushes.Black;
                    casovac.Interval = TimeSpan.FromSeconds(5);
                    break;
                case 2:
                    // 2 - svítí pouze oranžová po 3s
                    svetloZelena.Fill = Brushes.Black;
                    svetloOranzova.Fill = Brushes.Orange;
                    svetloCervena.Fill = Brushes.Black;
                    casovac.Interval = TimeSpan.FromSeconds(3);
                    break;
                case 3:
                    // 3 - svítí pouze červená po 5s
                    svetloZelena.Fill = Brushes.Black;
                    svetloOranzova.Fill = Brushes.Black;
                    svetloCervena.Fill = Brushes.Red;
                    casovac.Interval = TimeSpan.FromSeconds(5);
                    break;
                default:
                    // 4 - svítí pouze červená a oranžová po 1s
                    svetloZelena.Fill = Brushes.Black;
                    svetloOranzova.Fill = Brushes.Orange;
                    svetloCervena.Fill = Brushes.Red;
                    casovac.Interval = TimeSpan.FromSeconds(1);

                    // restartovat cyklus
                    stav = 0;
                    break;
            }
        }
    }
}
