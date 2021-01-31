package com.company;

import static com.company.Main.array;

public class Newton_interpolar {

    private static double step;
    private static int step_dec = 0;

    public static void Newton_create_mass() {
        step = array[2][0] - array[0][0];
        for (int i = 1; i < array.length; i += 2) {
            array[i][1] = Pn(array[i][0], array.length);
        }

        step = 0;
    }

    private static double Pn(double x, int n) {
        double result = Cj(0);
        for (int j = 2; j < n; j += 2) {
            double something = Cj(j);
            step_dec++;
            for (int k = 0; k < j; k += 2)
                something *= x - array[k][0];
            result += something;
        }
        step_dec = 0;
        return result;
    }

    static double Cj(int j) {
        if (j == 0)
            return array[0][1];
        double delta = Delta(j, j);
        return delta / (Factorial(j - 1 - step_dec) * Math.pow(step, j - 1 - step_dec));
    }

    static double Delta(int p, int i) {
        if (p == 2)
            return array[i][1] - array[i - 2][1];
        return Delta(p - 2, i) - Delta(p - 2, i - 2);
    }

    static double Factorial(double value) {
        if (value == 0)
            return 1;
        return value * Factorial(value - 1);
    }
}
