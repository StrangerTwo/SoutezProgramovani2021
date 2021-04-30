using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Hash
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Vyber složku se soubory. [Enter]");
            Console.ReadKey();

            // nechat uživatele vybrat složku
            List<string> soubory = getFiles();
            if (soubory.Count == 0)
            {
                Console.WriteLine("Nebyli vybrány žádné soubory.");
                Console.ReadKey();
                return;
            }

            // upozornění na nalezené soubory
            Console.WriteLine("Nalezeno " + soubory.Count + " souborů :");
            soubory.Sort();
            for (int i = 0; i < soubory.Count; i++)
            {
                Console.WriteLine(i + ": " + new FileInfo(soubory[i]).Name);
            }

            Console.WriteLine("Tvořím otisk souborů..");

            for (int i = 0; i < soubory.Count; i++)
            {
                //ČTENÍ SOUBORŮ A TVOŘENÍ OTISKU
                long otisk = 0;
                using (StreamReader reader = new StreamReader(soubory[i]))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        otisk = sectiHex(line, otisk);
                    }
                }

                while (otisk.ToString("X").Length > 1)
                {
                    otisk = sectiHex(otisk.ToString("X"));
                }

                Console.WriteLine(i + " " + otisk.ToString("X") + " " + new FileInfo(soubory[i]).Name);
            }


            Console.WriteLine("Otisky vytvořeny !");
            Console.ReadKey();
        }

        static long sectiHex(string hex, long zaklad = 0)
        {
            foreach (char znak in hex)
            {
                switch (znak)
                {
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                    case 'a':
                    case 'A':
                    case 'b':
                    case 'B':
                    case 'c':
                    case 'C':
                    case 'd':
                    case 'D':
                    case 'e':
                    case 'E':
                    case 'f':
                    case 'F':
                        zaklad += int.Parse(znak.ToString(), System.Globalization.NumberStyles.HexNumber);
                        break;
                }
            }
            return zaklad;
        }

        static List<string> getFiles()
        {
            FolderBrowserDialog fBD = new FolderBrowserDialog();
            fBD.SelectedPath = Directory.GetCurrentDirectory();

            if (fBD.ShowDialog() != DialogResult.OK)
            {
                return new List<string>();
            }

            return new List<string>(Directory.GetFiles(fBD.SelectedPath, "*", SearchOption.TopDirectoryOnly));
        }
    }
}
