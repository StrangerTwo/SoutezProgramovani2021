using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HraSCisli
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Vítej v programu");
            int cisloN = 0;
            while (true)
            {
                Console.WriteLine("Zadej číslo n:");
                cisloN = Convert.ToInt32(Console.ReadLine());
                if (cisloN == 0)
                {
                    break;
                }

                //tvoření řady až do potřebného číslo
                string rada = "";
                int i = 0;
                while (rada.Length < cisloN)
                {
                    i++;
                    rada += i;
                }

                Console.WriteLine("číslice " + rada[cisloN - 1] + " z čísla " + i);
            };
        }
    }
}
