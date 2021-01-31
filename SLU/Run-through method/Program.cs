using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main()
        {
            //вводим размерность с клавиатуры
            Console.Write("Введите размерность\nn:= ");
            int n = int.Parse(Console.ReadLine());

            var res = new RunMethod(n);
            res.Createsolution(5, 0.000001);

            res.printingSLAE();

            res.PrintMethodResult();

            Console.ReadLine();
        }
    }
}
