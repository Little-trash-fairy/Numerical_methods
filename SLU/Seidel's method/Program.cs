﻿using System;
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
                for (int j = 0; j < n+1; j++)
                {
                    Console.Write($"{matrix[i][j],15:E} ");
                }
                Console.WriteLine("");
            }
        }
        private static void Main()
        {
            Console.Write("Введите размерность:\nn:= ");
            n = int.Parse(Console.ReadLine());

            GenereMatrix(out double[][] matrix);

            Console.WriteLine("\nИсходная матрица:");
            Print(matrix);
            Console.WriteLine();

            try
            {
                Stopwatch TimeAlgorithmRun = new Stopwatch();
                var (counter, result) = matrix.method_Seidel(TimeAlgorithmRun);

                Console.WriteLine("\nВремя выполнения алгоритма:\n" +
                      $"1. В тактах - {TimeAlgorithmRun.Elapsed.Ticks}\n" +
                      $"2. В милисекундах - {TimeAlgorithmRun.Elapsed.Milliseconds}\n");


                Console.WriteLine($"\n\nКоличество итераций: {counter}");

                Console.WriteLine("\nРезультат:");
                for (int i = 0; i < n; i++)
                {
                    Console.Write($"{result[i],15:E} ");
                }

            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadLine();
        }
    }
}