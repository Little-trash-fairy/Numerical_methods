using System;
using System.Text;

namespace ConsoleApp1
{
    /// <summary>
    /// Класс для работы в методом прогонки
    /// </summary>
    class RunMethod
    {
        /// <summary>
        /// Коэффициент ниже главной диагонали
        /// </summary>
        private double[] A;
        /// <summary>
        /// Коэффициент главной диагонали
        /// </summary>
        private double[] B;
        /// <summary>
        /// Коэффициент выше главной диагонали
        /// </summary>
        private double[] C;
        /// <summary>
        /// Коэффициент свободного столбца
        /// </summary>
        private double[] F;

        /// <summary>
        /// Коэффициент прямого хода
        /// </summary>
        private double[] alpha;
        /// <summary>
        /// Коэффициент прямого хода
        /// </summary>
        private double[] betta;
        /// <summary>
        /// Коэффициент обратного хода
        /// </summary>
        private double[] res;

        private int n;

        public RunMethod(int n)
        {
            this.n = n;

            alpha = new double[n];
            betta = new double[n];
            res = new double[n];
            F = new double[n];
            B = new double[n];

            A = new double[n - 1];
            C = new double[n - 1];

        }
        public void Createsolution(double main_elem,double mul_elem)
        {
            double other_value = main_elem * mul_elem;
            B[0] = main_elem;
            B[n - 1] = main_elem + n - 1;
            C[0] = main_elem * mul_elem;
            A[n - 2] = B[n - 1] * mul_elem;  
            F[0] = (B[0] * B[0] + B[1] * A[0]);
            ++main_elem;

            for (int i = 1; i < n - 1; i++)
            {
                B[i] = main_elem;
                C[i] = main_elem * mul_elem;
                A[i - 1] = main_elem * mul_elem;
                F[i] = (B[0] * C[i - 1] + B[1] * B[i] + B[2] * A[i]);
                ++main_elem;
            }
            F[n - 1] = (B[0] * C[n - 2] + B[1] * B[n - 1]);

            alpha[0] = (-C[0] * B[0]);
            betta[0] = (F[0] * B[0]);


            for (int i = 1; i < n - 1; i++)
            {
                alpha[i] = (-C[i] / (A[i] * alpha[i - 1] + B[i]));
                betta[i] = ((F[i] - A[i] * betta[i - 1]) / (A[i] * alpha[i - 1] + B[i]));
            }

            res[n - 1] = ((F[n - 1] - betta[n - 2] * A[n - 2]) / (B[n - 1] + alpha[n - 2] * A[n - 2]));

            for (int i = n - 2; i >= 0; i--)
            {
                res[i] = (alpha[i] * res[i + 1] + betta[i]);
            }
        }


        private string PrintZero(int count)
        {
            StringBuilder result = new StringBuilder();
            for (int j = 0; j < count; j++)
            {
                result.Append($"{"0",20}");
            }
            return result.ToString();
        }

        public void printingSLAE()
        {


            int right_zero = n - 2;
            int left_zero = 0;

            Console.WriteLine("\nСЛАУ:");
            Console.Write(
                $"{B[0],20}{C[0],20}"+
                PrintZero(right_zero) +
                $"{F[0],20}\n"
                );
            right_zero--;

            for (int i = 1; i < n - 1; i++)
            {
                Console.Write(
                    PrintZero(left_zero) + 
                    $"{A[i - 1],20}{B[i],20}{C[i],20}"+
                    PrintZero(right_zero) +
                    $"{F[i],20}\n"
                    );

                right_zero--;
                left_zero++;
            }

            Console.Write(
                PrintZero(left_zero) +
                $"{A[n - 2],20}{B[n - 1],20}{F[n - 1],20}\n"
                );
        }

        public void PrintMethodResult()
        {
            Console.WriteLine("\nРешения:");
            foreach (var elem in res)
            {
                Console.WriteLine($"{elem,20}");
            }
        }
    }
}
