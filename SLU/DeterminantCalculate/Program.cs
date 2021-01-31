using System;
using System.Diagnostics;

namespace ConsoleApp2
{
    internal class Program
    {
        private static int n;
        public static void GenereMatrix(out double[][] matrix)
        {
            double elem = 5;

            matrix = new double[n][];

            for (int i = 0; i < n; i++)
            {
                matrix[i] = new double[n];
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
        }

        public static void Print(double[][] matrix)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write($"{matrix[i][j],17:f7} ");
                }
                Console.WriteLine();
            }
        }

        private static void Main()
        {
            Console.Write("Введите размерность:\nn:= ");
            n = int.Parse(Console.ReadLine());

            GenereMatrix(out double[][] matrix);

            Console.WriteLine("Исходная матрица:\n");
            Print(matrix);

            try
            {
                Stopwatch stopWatch = new Stopwatch();
                var result = matrix.Determinant(stopWatch);

                Console.WriteLine("Матрица, после работы алгоритма:\n");
                Print(matrix);

                Console.Write($"\nОпределитель = {result}");

                Console.WriteLine("\n\nВремя выполнения алгоритма:\n" +
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