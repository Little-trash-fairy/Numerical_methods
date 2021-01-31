using System;
using System.Diagnostics;

namespace ConsoleApp2
{
    internal class Program
    {
        public static double[][] GenereMatrix(int n)
        {
            double start = 5;
            double else_value;
            double[][] matrix = new double[n][];
            for (int i = 0; i < n; i++)
            {
                matrix[i] = new double[n];
            }

            for (int i = 0; i < n; i++)
            {
                else_value = start / 1000;
                for (int j = 0; j < n; j++)
                {
                    if (i == j)
                    {
                        matrix[i][j] = start;
                        start++;
                    }
                    else
                    {
                        matrix[i][j] = else_value;
                    }
                }
            }

            return matrix;
        }

        public static void Print(double[][] matrix)
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                foreach (var elem in matrix[i])
                {
                    Console.Write($"{elem,25:f15} ");
                }
                if (i != matrix.GetLength(0) - 1)
                {
                    Console.WriteLine("");
                }
            }
        }

        private static void Main()
        {
            Console.Write("Введите размерность:\nn:= ");
            int n = int.Parse(Console.ReadLine());
            double[][] matrix = GenereMatrix(n);

            Console.WriteLine("\nИсходная матрица:");
            Print(matrix);

            try
            {
                Stopwatch TimeAlgorithmRun = new Stopwatch();
                TimeAlgorithmRun.Start();
                var result = matrix.Inverse();
                TimeAlgorithmRun.Stop();
                Stopwatch TimeCheckRun = new Stopwatch();

                Console.WriteLine("\n\nОбратная матрица:");
                Print(result);

                Console.WriteLine("\n\nРезультат проверки:");
                Print(matrix.MultiplicationMatri(result, Mode.ModeTwoSquareMatrix,
                                                                    TimeCheckRun));

                Console.WriteLine("\n\nВремя выполнения алгоритма:\n" +
                  $"1. В тактах - {TimeAlgorithmRun.Elapsed.Ticks}\n" +
                  $"2. В милисекундах - {TimeAlgorithmRun.Elapsed.Milliseconds}");

                Console.WriteLine("\n\nВремя выполнения проверки:\n" +
                  $"1. В тактах - {TimeCheckRun.Elapsed.Ticks}\n" +
                  $"2. В милисекундах - {TimeCheckRun.Elapsed.Milliseconds}");
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