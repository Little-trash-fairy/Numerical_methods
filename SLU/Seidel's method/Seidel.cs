using System;
using System.Diagnostics;

namespace ConsoleApp2
{
    public static class Seidel
    {
        private static readonly double e = 1E-20;
        private static int n;

        public static (int, double[]) method_Seidel(this double[][] matrix,
                                            Stopwatch TimeAlgorithmRun = null)
        {
            n = matrix.Length;
            double[] previousVariableValues = new double[n];
            double[] currentVariableValues = new double[n];

            return matrix.Find_result(previousVariableValues,
                                       currentVariableValues,
                                       TimeAlgorithmRun);
        }

        private static void CreateResult(this double[][] matrix, double[] this_step, double[] last_step)
        {
            double buf;
            for (int i = 0; i < n; i++)
            {
                buf = matrix[i][n];

                for (int j = 0; j < n; j++)
                {
                    if (j < i)
                    {
                        buf -= matrix[i][j] * this_step[j];
                    }
                    if (j > i)
                    {
                        buf -= matrix[i][j] * last_step[j];
                    }
                }
                this_step[i] = buf / matrix[i][i];
            }
        }
        public static (int, double[]) Find_result(this double[][] matrix,
                                            double[] last_step,
                                            double[] this_step,
                                            Stopwatch TimeAlgorithmRun = null)
        {
            int counter = 0;
            double error;

            if (TimeAlgorithmRun != null)
            {
                TimeAlgorithmRun.Start();
            }


            do
            {

                matrix.CreateResult(this_step, last_step);

                error = 0;

                for (int i = 0; i < n; i++)
                {
                    error += Math.Abs(this_step[i] - last_step[i]);
                }

                if (error < e)
                {
                    if (TimeAlgorithmRun != null)
                    {
                        TimeAlgorithmRun.Stop();
                    }

                    return (counter, last_step);
                }
                counter++;

                if(counter>100)
                {
                    throw new Exception("Количество итераций превысило 100");
                }
                for (int i = 0; i < n; i++)
                {
                    last_step[i] = this_step[i];
                }
            } while (true);
        }
    }
}