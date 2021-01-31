using System;
using System.Diagnostics;

namespace ConsoleApp2
{
    internal class Program
    {

        /*
         * b - правая граница отрезка
         * h - шаг сетки аргумента
         * N - число точек на отрезке
         */
        private static int N;
        private static double b;
        private static double h;

        /*
         * Константная строка, необходимая для формирования таблицы
         */
        private static string const_low_sl;

        /*
         * Массив содержащий сетку разбиения:
         * points[0][i] - значения аргумента
         * points[1][i] - приближённые значения функции
         */
        private static double[][] points = new double[2][];


        private static double func(double x,
                            double y) => (3 * x * x - 10 * x + x * x * (x - 5) - y);
        private static double real_values(double x) => x * x * (x - 5);


        private static void Modif_Euler(Stopwatch TimeAlgorithmRun = null)
        {

            if (TimeAlgorithmRun != null)
            {
                TimeAlgorithmRun.Start();
            }
            for (int i = 1; i <= N; i++)
            {
                points[0][i] = points[0][i - 1] + h;
                points[1][i] = points[1][i - 1] + 
                               h * 
                               0.5 * 
                                    (func(  points[0][i - 1], 
                                            points[1][i - 1]) + 
                                     func(  points[0][i], 
                                            points[1][i - 1] +
                                            h * 
                                            func(points[0][i - 1], points[1][i - 1])));
            }

            for (int i = 1; i <= N; i++)
            {
                points[1][i] = points[1][i - 1] +
                               h *
                               0.5 *
                                    (func(points[0][i - 1],
                                            points[1][i - 1]) +
                                     func(points[0][i], points[1][i]));
            }

            if (TimeAlgorithmRun != null)
            {
                TimeAlgorithmRun.Stop();
            }
        }
        private static void Real_value()
        {
            Console.WriteLine($"\n{" ".PadRight(92, '_')}");
            Console.WriteLine($"|{'i',5}    |{'x',25}    |{'y',25}|{'e',25}|");
            Console.WriteLine(const_low_sl +
                           $"{"".PadRight(25, '_')}|");
            for (int i = 0; i <= N; i++)
            {
                Console.WriteLine($"|{i,5}    |{points[0][i],25}    |{real_values(points[0][i]),25}|{Math.Abs(points[1][i] - real_values(points[0][i])),25}|");
                Console.WriteLine(const_low_sl +
                           $"{"".PadRight(25, '_')}|");
            }
        }

        private static void init()
        {

            Console.Write("Рассматривается отрезок вида [a,b]");
            Console.Write("Введите число точек на отрезке\nN:= ");
            N = int.Parse(Console.ReadLine());

            Console.Write("Введите левую границу аргумента\na:= ");
            points[0] = new double[N + 1];
            points[0][0] = double.Parse(Console.ReadLine());

            Console.Write("Введите правую границу аргумента\nb:= ");
            b = double.Parse(Console.ReadLine());

            h = (b - points[0][0]) / N;

            points[1] = new double[N + 1];
            points[1][0] = real_values(points[0][0]);

            const string V = "|";
            const_low_sl = $"{V}{"".PadRight(9, '_')}" +
                           $"{V}{"".PadRight(29, '_')}" +
                           $"{V}{"".PadRight(25, '_')}{V}";
        }

        private static void print()
        {
            Console.WriteLine($"\n{" ".PadRight(66, '_')}");
            Console.WriteLine($"|{'i',5}    |{'x',25}    |{'y',25}|");
            Console.WriteLine(const_low_sl);
            for (int i = 0; i <= N; i++)
            {
                Console.WriteLine($"|{i,5}    |{points[0][i],25}    |{points[1][i],25}|");
                Console.WriteLine(const_low_sl);
            }
        }
        private static void Main()
        {
            init();
            Stopwatch TimeAlgorithmRun = new Stopwatch();

            Modif_Euler(TimeAlgorithmRun);


            Console.WriteLine("\nРезультат работы простого метода Эйлера для задачи Коши:");
            print();

            Console.WriteLine("\nРезультат вычисления реальных значений функции:");
            Real_value();

            Console.WriteLine("\nВремя выполнения простого метода Эйлера для задачи Коши:\n" +
                              $"1. В тактах - {TimeAlgorithmRun.Elapsed.Ticks}\n" +
                              $"2. В милисекундах - {TimeAlgorithmRun.Elapsed.Milliseconds}");
            Console.ReadLine();
        }
    }
}