using System;
using System.Diagnostics;

namespace ConsoleApp2
{
    public static class InverseMatrix
    {
        public static readonly double eps = 1e-20;

        public static double[][] Inverse(this double[][] mA,
                                          Stopwatch TimeAlgorithmRun = null)
        {
            for (int i = 0; i < mA.Length; i++)
            {
                if (mA.Length != mA[i].Length)
                {
                    throw new Exception("Не выполнено условие - размерность матрицы не равна n*n");
                }
            }
            double[][] result = new double[mA.Length][];
            for (int i = 0; i < mA.Length; i++)
            {
                result[i] = (double[])mA[i].Clone();
            }
            return Inverse(mA, result, TimeAlgorithmRun);
        }

        private static double[][] Inverse(double[][] mA,
                                          double[][] result,
                                          Stopwatch TimeAlgorithmRun = null)
        {
            if (TimeAlgorithmRun != null)
            {
                TimeAlgorithmRun.Start();
            }

            double det = mA.Determinant();
            double inserse_det = 1 / det;

            if (det == 0)
            {
                throw new ArgumentException("Вырожденная матрица");
            }

            for (int i = 0; i < mA.Length; i++)
            {
                for (int j = 0; j < mA[0].Length; j++)
                {
                    try
                    {
                        //Матрица без строки i-й и j-го столбца
                        double[][] minor = mA.CreateMinor(i, j);
                        double sign = Math.Pow(-1, i + j);
                        double minor_Det = Determinant(minor);

                        result[j][i] = inserse_det * minor_Det * sign;
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
            if (TimeAlgorithmRun != null)
            {
                TimeAlgorithmRun.Stop();
            }
            return result;
        }

        //Построение минора
        public static double[][] CreateMinor(this double[][] mA, int row, int column)
        {
            if (row > mA.Length || row < 0)
            {
                throw new IndexOutOfRangeException("Индекс строки не принадлежит матрице");
            }
            if (column > mA.Length || column < 0)
            {
                throw new IndexOutOfRangeException("Индекс столбца не принадлежит матрице");
            }
            double[][] ret = new double[mA.Length - 1][];
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = new double[ret.Length];
            }
            int offsetRow = 0;
            for (int i = 0; i < mA.Length; i++)
            {
                int offsetCollumn = 0;
                if (i == row)
                {
                    offsetRow++;
                    continue;
                }
                for (int t = 0; t < mA.Length; t++)
                {
                    if (t == column)
                    {
                        offsetCollumn++;
                        continue;
                    }
                    ret[i - offsetRow][t - offsetCollumn] = mA[i][t];
                }
            }
            return ret;
        }

        // Умножение матриц
        public static double[][] MultiplicationMatri(
            this double[][] a,
            double[][] b,
            Mode matrixMultiMode,
            Stopwatch TimeCheckRun = null)
        {
            if (TimeCheckRun != null)
            {
                TimeCheckRun.Start();
            }

            if (a is null ||
                b is null)
            {
                throw new ArgumentNullException("Одна из матриц пуста");
            }

            if (matrixMultiMode == Mode.ModeOtherMatrix &&
                a.Length != b[0].Length)
            {
                throw new Exception("Не выполнено условие, необходимое для умножения матрицы на матрицу - " +
                                    "размерность a - n*m, размерность b - m*k");
            }

            double[][] result = new double[a.Length][];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new double[b[0].Length];
            }

            for (int i = 0; i < a.Length; i++)
            {
                for (int j = 0; j < b[0].Length; j++)
                {
                    for (int k = 0; k < b.Length; k++)
                    {
                        result[i][j] += a[i][k] * b[k][j];
                    }
                }
            }
            if (TimeCheckRun != null)
            {
                TimeCheckRun.Stop();
            }
            return result;
        }

        public static double Determinant(this double[][] matrix)
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                if (matrix.Length != matrix[i].Length)
                {
                    throw new ArgumentException(nameof(matrix), "Столбцов должно быть столько же, сколько строк");
                }
            }

            double[][] result = new double[matrix.Length][];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (double[])matrix[i].Clone();
            }
            return result.DeterminantCalculate();
        }

        private static double DeterminantCalculate(this double[][] matr)
        {
            int rows = matr.Length;
            double deter = 1;
            for (int row = 0; row < rows; row++)
            {
                deter *= LineSwapping(matr, row);
                double num = matr[row][row];
                if (Math.Abs(num) < eps)
                {
                    return 0;
                }
                deter *= num;
                matr[row][row] = 1.0;
                for (int col = row + 1; col < rows; col++)
                {
                    matr[row][col] /= num;
                }

                for (int rrow = row + 1; rrow < rows; rrow++)
                {
                    double nnum = matr[rrow][row];
                    matr[rrow][row] = 0.0;
                    for (int col = row + 1; col < rows; col++)
                    {
                        matr[rrow][col] = matr[rrow][col] - nnum * matr[row][col];
                    }
                }
            }
            return deter;
        }

        private static int LineSwapping(double[][] matr, int col)
        {
            int rows = matr.Length;
            double max = 0;
            int rowMax = -1;
            for (int row = col; row < rows; row++)
            {
                double num = matr[row][col];
                if (num < 0)
                {
                    num = -num;
                }
                if (num > 1)
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