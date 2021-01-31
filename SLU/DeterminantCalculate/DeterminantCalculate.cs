using System;
using System.Diagnostics;

namespace ConsoleApp2
{
    public static class DeterminantCalculate
    {
        public static double eps = 1e-20;
        private static int n;
        public static double Determinant(this double[][] matr,
                                              Stopwatch stopWatch = null)
        {
            n = matr.Length;
            for (int i = 0; i < n; i++)
            {
                if (n != matr[i].Length)
                {
                    throw new ArgumentException(nameof(matr), "Столбцов должно быть столько же, сколько строк");
                }
            }

            return matr.Det(stopWatch);
        }
        private static double Det(this double[][] matr,
                                       Stopwatch stopWatch = null)
        {
            if (stopWatch != null)
            {
                stopWatch.Start();
            }

            double deter = 1;

            for (int row = 0; row < n; row++)
            {
                deter *= LineSwapping(matr, row);
                double num = matr[row][row];
                if (Math.Abs(num) < eps)
                {
                    throw new ArgumentException(nameof(matr), "Найден нулевой столбец");
                }

                deter *= num;
                matr[row][row] = 1.0;

                for (int col = row + 1; col < n; col++)
                {
                    matr[row][col] /= num;
                }

                for (int rrow = row + 1; rrow < n; rrow++)
                {
                    double nnum = matr[rrow][row];
                    matr[rrow][row] = 0.0;
                    for (int col = row + 1; col < n; col++)
                    {
                        matr[rrow][col] = matr[rrow][col] - nnum * matr[row][col];
                    }
                }
            }
            if (stopWatch != null)
            {
                stopWatch.Stop();
            }
            return deter;
        }

        private static int LineSwapping(this double[][] matr, int col)
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
                return -1;
            }
            return 1;
        }
    }
}