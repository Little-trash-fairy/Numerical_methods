using System;
using System.Diagnostics;

namespace ConsoleApp2
{
    public static class InverseMatrix
    {
        public static readonly double eps = 1e-20;
        private static int n;
        private static void Gauss(this double[][] source_matrix,
                                        int index_in_buf_row,
                                        double[] result)
        {
            int index;
            double max = 0;

            double[] buf_row = new double[n];
            buf_row[index_in_buf_row] = 1;

            double[][] buf_source_matrix = new double[n][];

            for (int i = 0; i < n; i++)
            {
                buf_source_matrix[i] = new double[n];
                for (int j = 0; j < n; j++)
                {
                    buf_source_matrix[i][j] = source_matrix[i][j];
                }
            }

            for (int k = 0; k < n; k++)
            {
                if (buf_source_matrix[k][k] < 0)
                {
                    max = -buf_source_matrix[k][k];
                }

                index = k;
                double buf;
                for (int i = k + 1; i < n; i++)
                {
                    buf = buf_source_matrix[i][k];
                    if (buf < 0)
                    {
                        buf *= -1;
                    }

                    if (buf > max)
                    {
                        max = buf;
                        index = i;
                    }
                }

                for (int j = 0; j < n; j++)
                {
                    buf = buf_source_matrix[k][j];
                    buf_source_matrix[k][j] = buf_source_matrix[index][j];
                    buf_source_matrix[index][j] = buf;
                }
                buf = buf_row[k];
                buf_row[k] = buf_row[index];
                buf_row[index] = buf;

                for (int i = k + 1; i < n; i++)
                {
                    double M = buf_source_matrix[i][k] / buf_source_matrix[k][k];

                    for (int j = k; j < n; j++)
                    {
                        buf_source_matrix[i][j] -= M * buf_source_matrix[k][j];
                    }

                    buf_row[i] -= M * buf_row[k];
                }
            }

            if (buf_source_matrix[n - 1][n - 1] == 0 &&
                buf_row[n - 1] == 0)
            {
                throw new Exception("Ошибка при выполнении метода Гауса");
            }

            for (int i = n - 1; i >= 0; i--)
            {
                double sum = 0;
                for (int j = i + 1; j < n; j++)
                {
                    sum += buf_source_matrix[i][j] * result[j];
                }
                result[i] = (buf_row[i] - sum) / buf_source_matrix[i][i];
            }
        }

        public static double[][] Inverse(this double[][] source_matrix,
                                              Stopwatch TimeAlgorithmRun = null)
        {
            n = source_matrix.Length;
            foreach (var elem in source_matrix)
            {
                if (n != elem.Length)
                {
                    throw new ArgumentException("\n\nМатрица должна иметь квадратное представление");
                }
            }

            double[][] result_matrix = new double[n][];

            for (int i = 0; i < n; i++)
            {
                result_matrix[i] = new double[n];
            }

            double[] result = new double[n];
            try
            {
                return source_matrix.CreateInverseMatrix(result_matrix,
                                        result,
                                        TimeAlgorithmRun);
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        private static double[][] CreateInverseMatrix(this double[][] source_matrix,
                                                      double[][] result_matrix,
                                                      double[] result,
                                                      Stopwatch TimeAlgorithmRun = null)
        {
            if (TimeAlgorithmRun != null)
            {
                TimeAlgorithmRun.Start();
            }

            for (int i = 0; i < n; i++)
            {
                try
                {
                    source_matrix.Gauss(i, result);
                }
                catch (Exception e)
                {
                    throw e;
                }

                for (int j = 0; j < source_matrix.Length; j++)
                {
                    result_matrix[j][i] = result[j];
                }
            }

            if (TimeAlgorithmRun != null)
            {
                TimeAlgorithmRun.Stop();
            }
            return result_matrix;
        }


        public static double[][] MatrixMul(
            this double[][] a,
            double[][] b,
            Mode matrixMultiMode,
            Stopwatch TimeCheckRun = null)
        {

            if (a is null ||
                b is null)
            {
                throw new ArgumentNullException("Одна из матриц пуста");
            }
            if (matrixMultiMode == Mode.ModeOtherMatrix)
            {
                for (int i = 0; i < b.Length; i++)
                {
                    if (a.Length != b[i].Length)
                    {
                        throw new Exception("Не выполнено условие, необходимое для умножения матрицы на матрицу - " +
                                            "размерность a - n*m, размерность b - m*k");
                    }
                }
            }

            double[][] result = new double[a.Length][];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new double[b[0].Length];
            }
            try
            {
                return a.MatrixMulAlgorithm(b, result, TimeCheckRun);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // Умножение матриц
        private static double[][] MatrixMulAlgorithm(
            this double[][] a,
            double[][] b,
            double[][] result,
            Stopwatch TimeCheckRun = null)
        {
            if (TimeCheckRun != null)
            {
                TimeCheckRun.Start();
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
    }
}