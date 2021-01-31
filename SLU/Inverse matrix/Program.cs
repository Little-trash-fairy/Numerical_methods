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
                    Console.Write($"{matrix[i][j],25:f15} ");
                }
                Console.WriteLine();
            }
        }

        private static void Main()
        {
            Console.Write("Введите размерность:\nn:= ");
            n = int.Parse(Console.ReadLine());

            GenereMatrix(out double[][] matrix);

            Console.WriteLine("\nИсходная матрица:");
            Print(matrix);

            Stopwatch TimeAlgorithmRun = new Stopwatch();

            try
            {
                var result = matrix.Inverse(TimeAlgorithmRun);

                Console.WriteLine("\n\nОбратная матрица:");
                Print(result);

                Console.WriteLine("\n\nВремя выполнения алгоритма:\n" +
                                  $"1. В тактах - {TimeAlgorithmRun.Elapsed.Ticks}\n" +
                                  $"2. В милисекундах - {TimeAlgorithmRun.Elapsed.Milliseconds}");

                TimeAlgorithmRun.Reset();

                Console.WriteLine("\n\nРезультат проверки:");
                Print(matrix.MatrixMul(result, Mode.ModeTwoSquareMatrix,
                                                                    TimeAlgorithmRun));

                Console.WriteLine("\n\nВремя выполнения проверки:\n" +
                  $"1. В тактах - {TimeAlgorithmRun.Elapsed.Ticks}\n" +
                  $"2. В милисекундах - {TimeAlgorithmRun.Elapsed.Milliseconds}");
            }
            catch (Exception e)
            {
                Print(matrix);
                Console.WriteLine("\n\n" + e.Message);
            }

            Console.ReadLine();
        }
    }
}