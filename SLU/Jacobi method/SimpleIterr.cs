using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ConsoleApp2
{
    public static class SimpleIterr
    {
        private static readonly double e = 1E-20;
        private static int n;
        public static void Normalization(this double[][] matrix,
                                      Stopwatch TimeAlgorithmRun = null)
        {
            n = matrix.Length;
            double temp;
            if (TimeAlgorithmRun != null)
            {
                TimeAlgorithmRun.Start();
            }

            for (int k = 0; k < n; k++)
            {
                for (int i = 0; i < n; i++)
                {
                    temp = -1 / matrix[i][i];
                    matrix[i][n] *= temp;

                    for (int j = 0; j < n; j++)
                    {
                        matrix[i][j] *= temp;
                    }
                }
            }
            for (int i = 0; i < n; i++)
            {
                matrix[i][n] *= -1;
                matrix[i][i] = 0;
            }

            if (TimeAlgorithmRun != null)
            {
                TimeAlgorithmRun.Stop();
            }
        }
        private static double Elem_Norma(this double[][] mas)
        {
            double sum, max = double.MinValue;

            for (int i = 0; i < n; i++)
            {
                sum = 0;
                for (int j = 0; j < n; j++)
                {
                    sum += Math.Abs(mas[i][j]);
                }

                if (sum > max)
                {
                    max = sum;
                }
            }
            return max;
        }
        private static double Inaccuracy(this double[][] matrix)
        {
            return (e / matrix.Elem_Norma() - e);
        }
        public static (double[], int) MethodJacoby(this double[][] matrix,
                                                    List<double> max_value_history,
                                                    Stopwatch TimeAlgorithmRun = null)
        {
            n = matrix.Length;

            double[] buf_free_column = new double[n];
            double[] epsilon = new double[n];

            for (int i = 0; i < n; i++)
            {
                buf_free_column[i] = matrix[i][n];
            }
            try
            {
                return SimpleIterration(matrix,
                        max_value_history,
                        buf_free_column,
                        epsilon,
                        TimeAlgorithmRun);
            }catch(Exception e)
            {
                throw e;
            }
        }
        private static (double[], int) SimpleIterration(this double[][] matrix,
                                            List<double> max_value_history,
                                            double[] buf_free_column,
                                            double[] epsilon,
                                            Stopwatch TimeAlgorithmRun = null)
        {

            double  max, 
                    matrix_normal,
                    buf_elem;

            int counter = 0;

            if (TimeAlgorithmRun != null)
            {
                TimeAlgorithmRun.Start();
            }

            matrix_normal = matrix.Inaccuracy();
            do
            {
                max = double.MinValue;
                for (int i = 0; i < n; i++)
                {
                    buf_elem = 0;
                    for (int j = 0; j < n; j++)
                    {
                        buf_elem += matrix[i][j] * buf_free_column[j];
                    }

                    buf_elem += matrix[i][n];
                    epsilon[i] = Math.Abs(buf_elem - buf_free_column[i]);
                    buf_free_column[i] = buf_elem;

                    if (max < epsilon[i])
                    {
                        max = epsilon[i];
                    }
                }

                max_value_history.Add(max);
                counter++;

                if (counter > 100)
                {
                    throw new Exception("Количество итераций превысило 100");
                }

            } while (max > matrix_normal);

            if (TimeAlgorithmRun != null)
            {
                TimeAlgorithmRun.Stop();
            }

            return (buf_free_column, counter);
        }
    }
}