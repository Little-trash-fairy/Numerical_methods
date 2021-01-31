using System;
using System.Diagnostics;

namespace ConsoleApp1
{
    class Program
    {
        private static int n;
        public static void GenereMatrix(out double[][] matrix)
        {
            double elem = 5;

            matrix = new double[n][];

            for (int i = 0; i < n; i++)
            {
                matrix[i] = new double[n + 1];
                for (int j = 0, k = n - 1; j <= k; j++, k--)
                {
                    if (i == j)
                    {
                        k++;
                        continue;
                    }
                    matrix[i][k] = matrix[i][j] = elem * 0.000001;
                }
                matrix[i][i] = elem;
                elem++;
            }

            double res;
            for (int i = 0; i < n; i++)
            {
                res = 0;
                for (int j = 0; j < n; j++)
                {
                    res += matrix[i][j] * matrix[j][j];
                }
                matrix[i][n] = res;
            }
        }
        public static void Print(double[][] matrix)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n + 1; j++)
                {
                    Console.Write($"{matrix[i][j],17:f7}");
                }
                Console.WriteLine();
            }
        }
        static void Main()
        {
            Console.Write("Введите размерность:\nn:= ");
            n = int.Parse(Console.ReadLine());

            GenereMatrix(out double[][] matrix);

            Console.WriteLine("Исходная матрица:");
            Print(matrix);

            try
            {
                Stopwatch stopWatch = new Stopwatch();
                matrix.SLE(stopWatch);

                Console.WriteLine("\nМатрица обратного хода:");
                Print(matrix);

                Console.WriteLine("\nРезультат:");
                for (int i = 0; i < n; i++)
                {
                    Console.Write($"{matrix[i][n],17:f7}");
                }

                Console.WriteLine($"\n\nВремя выполнения алгоритма:\n" +
                    $"1. В тактах - {stopWatch.Elapsed.Ticks}\n" +
                    $"2. В милисекундах - {stopWatch.Elapsed.Milliseconds}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}
