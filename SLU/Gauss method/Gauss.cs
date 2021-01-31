using System;
using System.Diagnostics;

namespace ConsoleApp1
{
    public static class Gauss
    {

        public static double eps = 1e-30;
        private static int n;
        //Метод обёртка для функции, реализующей метод Гауса
        public static void SLE(this double[][] matrix,
                                    Stopwatch stopWatch = null)
        {
            n = matrix.Length;
            for (int i = 0; i < n; i++)
            {
                if (n != matrix[i].Length - 1)
                {
                    throw new ArgumentException(nameof(matrix), 
                        "Размерность матрицы должна быть равна n*(n+1)");
                }
            }

            try
            {
                matrix.GaussMethod(stopWatch);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static void GaussMethod(this double[][] matr,
                                                 Stopwatch stopWatch = null)
        {
            if (stopWatch != null)
            {
                stopWatch.Start();
            }

            for (int i = 0; i < n; i++)
            {
                matr.LineSwapping(i);

                double element_main_diagonal = matr[i][i];

                if (element_main_diagonal < eps)
                {
                    throw new ArgumentException("Наличие нулевого столбца");
                }

                matr[i][i] = 1.0;

                for (int j = i + 1; j <= n; j++)
                {
                    matr[i][j] /= element_main_diagonal;
                }

                for (int i_zero_step = 0; i_zero_step < n; i_zero_step++)
                {
                    if (i_zero_step != i)
                    {
                        double nnum = matr[i_zero_step][i];
                        matr[i_zero_step][i] = 0.0;
                        for (int j_zero_step = i + 1; 
                            j_zero_step <= n; 
                            j_zero_step++)
                        {
                            matr[i_zero_step][j_zero_step] = 
                                matr[i_zero_step][j_zero_step] - 
                                nnum * matr[i][j_zero_step];
                        }
                    }
                }
            }

            if (stopWatch != null)
            {
                stopWatch.Stop();
            }
        }

        private static void LineSwapping(this double[][] matr, int col)
        {
            double max = 0;
            int rowMax = -1;
            for (int row = col; row < n; row++)
            {
                double num = matr[row][col];

                if (num < 0)
                {
                    num = -num;
                }
                else if (num > 1)
                {
                    num = 1.0 / num;
                }

                if (num > max)
                {
                    max = num;
                    rowMax = row;
                }
            }

            if (rowMax > col)
            {
                var temp = matr[col];
                matr[col] = matr[rowMax];
                matr[rowMax] = temp;
            }
        }
    }
}
