using System;
using System.Diagnostics;

namespace ConsoleApp2
{
    internal class Program
    {

        /*
         * a - левая граница отрезка
         * b - правая граница отрезка
         * h - шаг сетки аргумента
         * N - число точек на отрезке
         */
        private static int N;
        private static double left_border;
        private static double right_border;
        private static double h;

        /*
         * Константы уравнения
         */
        public static int V = 5;
        public static int T = 5;


        private static double[] a;
        private static double[] b;
        private static double[] c;

        /*
         * Коэф-ты матрицы
         */
        private static double[] alpha;
        private static double[] beta;
        private static double[] f;

        /*
         * Решение
         */
        private static double[] x;
        private static double[] y;


        private static double P(double x) => x * x;

        private static double Q(double x) => x;
        private static double Func(double x) => 4 * V * Math.Pow(x, 4) -
                                               3 * V * T * Math.Pow(x, 3) +
                                               6 * V * x - 2 * V * T;
        private static double Real(double x) => V * x * x * (x - T);


        private static void Method_Differences(Stopwatch TimeAlgorithmRun = null)
        {
            if (TimeAlgorithmRun != null)
            {
                TimeAlgorithmRun.Start();
            }

            /*
             * Коэф-ты краевых условий
             */
            double A, B, C, D, E, F;
            A = left_border;
            B = right_border;
            C = 0;
            D = left_border;
            E = right_border;
            F = 0;

            b[0] = h * B - A;
            c[0] = A;
            f[0] = C * h;

            /*
             * Буферные переменные, для уменьшения кол-ва делений
             */
            double one_div_h_mul_h = 1.0 / (h * h),
                   one_div_two_h = 1.0 / (2 * h);

            /*
             * Заполняем коэф-ты матрицы
             */
            for (int i = 1; i < N - 1; ++i)
            {
                a[i] = one_div_h_mul_h - P(x[i]) * one_div_two_h;
                c[i] = one_div_h_mul_h + P(x[i]) * one_div_two_h;
                b[i] = -2.0 * one_div_h_mul_h + Q(x[i]);
                f[i] = Func(x[i]);
            }

            /*
             * Краевые услвоия
             */
            a[N - 1] = -D;
            b[N - 1] = E * h + D;
            f[N - 1] = h * F;

            alpha[0] = -c[0] / b[0];
            beta[0] = f[0] / b[0];

            /*
             * Прямой ход
             */
            for (int i = 1; i < N - 1; i++)
            {
                alpha[i] = -c[i] / (a[i] * alpha[i - 1] + b[i]);
                beta[i] = (f[i] - a[i] * beta[i - 1]) / (a[i] * alpha[i - 1] + b[i]);
            }

            /*
             * Обратный ход
             */
            y[N - 1] = (f[N - 1] - beta[N - 2] * a[N - 1]) /
                       (b[N - 1] + alpha[N - 2] * a[N - 1]);

            for (int i = N - 2; i >= 0; i--)
            {
                y[i] = alpha[i] * y[i + 1] + beta[i];
            }

            if (TimeAlgorithmRun != null)
            {
                TimeAlgorithmRun.Stop();
            }
        }
        private static void init()
        {

            Console.Write("Рассматривается отрезок вида [a,b]");

            Console.Write("Введите число точек на отрезке\nN:= ");
            N = int.Parse(Console.ReadLine());

            Console.Write("Введите левую границу\na:= ");
            left_border = int.Parse(Console.ReadLine());

            Console.Write("Введите правую границу\nb:= ");
            right_border = int.Parse(Console.ReadLine());

            a = new double[N];
            b = new double[N];
            c = new double[N];

            alpha = new double[N];
            beta = new double[N];
            x = new double[N];
            f = new double[N];
            y = new double[N];


            //точек больше чем интервалов разбиения
            h = (right_border - left_border) / (N - 1);

            /*
             * Заполняем x
             */
            x[0] = left_border;

            for (int i = 1; i < N; i++)
            {
                x[i] = x[i - 1] + h;
            }
        }
        private static void print()
        {
            const string V = "|";
            String const_low_sl = $"{V}{"".PadRight(9, '_')}" +
                            $"{V}{"".PadRight(29, '_')}" +
                            $"{V}{"".PadRight(25, '_')}" +
                            $"{V}{"".PadRight(25, '_')}" +
                            $"{V}{"".PadRight(25, '_')}{V}";

            Console.WriteLine($"\n{" ".PadRight(118, '_')}");
            Console.WriteLine($"|{'i',5}    |" +
                $"{"x",25}    |" +
                $"{"y_calc",25}|" +
                $"{"y_real",25}|" +
                $"{"e",25}|");
            Console.WriteLine(const_low_sl);

            for (int i = 0; i < N; i++)
            {
                double buf = Real(x[i]);
                Console.WriteLine($"|{i,5}    |" +
                    $"{x[i],25}    |" +
                    $"{y[i],25}|" +
                    $"{buf,25}|" +
                    $"{Math.Abs(y[i] - buf),25}|");
                Console.WriteLine(const_low_sl);
            }
        }
        private static void Main()
        {
            init();

            Stopwatch TimeAlgorithmRun = new Stopwatch();

            Method_Differences(TimeAlgorithmRun);

            print();

            Console.WriteLine("\nВремя выполнения разностного метода для краевой задачи:\n" +
                                     $"1. В тактах - {TimeAlgorithmRun.Elapsed.Ticks}\n" +
                                     $"2. В милисекундах - {TimeAlgorithmRun.Elapsed.Milliseconds}");
            Console.ReadLine();
        }
    }
}