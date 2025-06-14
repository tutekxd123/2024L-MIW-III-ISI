using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorytm_Genetyczny_164408
{
class Program
    {
        public static void Main()
        {
            var task1 = new Task1();
            task1.TaskRun();

            Console.WriteLine("Zadanie 1(Zmutowany Dywanik) Zakonczone Czekam 15s");
            Thread.Sleep(15000);
            //Czekanie15s
            var task2 = new Task2.Task2();
            task2.TaskRun();
            Console.WriteLine("Zadanie 2 Zakonczone Czekam 15s");
            var task3 = new Task3.Task3();
            task3.TaskRun();
            Console.WriteLine("Zadanie 3 (XOR) Zakończone");
        }
    }
}
