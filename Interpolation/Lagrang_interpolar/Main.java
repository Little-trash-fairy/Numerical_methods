package com.company;

import org.jetbrains.annotations.NotNull;

import java.util.Scanner;

import static com.company.Lagrang_interpolar.Lagrang_create_mass;


public class Main {

    public static void main(String[] args) {

        Double[][] array = Read_data_and_create_matrix();

        Lagrang_create_mass(array);
        Printer(array);
        waitForEnter();
    }

    private static Double[][] Read_data_and_create_matrix() {
        Double[][] array = null;
        try {
            Scanner in = new Scanner(System.in);
            System.out.print("Введите кол-во узлов:\nn:=");
            int n = in.nextInt();

            array = new Double[n * 2 - 1][2];

            int counter = 0;
            for (int i = 0; i < array.length; i += 2) {

                System.out.print("Введите узел\nX[" + counter + "]:=");
                array[i][0] = in.nextDouble();
                System.out.print("Введите значение функции в узле\nF[" + counter + "]:=");
                array[i][1] = in.nextDouble();
                counter++;
            }
            for (int i = 1; i < array.length -1; i += 2) {
                double k = (array[i - 1][0] + array[i + 1][0]) / 2;
                array[i][0] = k;
                array[i][1] = 0.0;
            }

        } catch (Exception e) {
            e.printStackTrace();
        }

        return array;
    }

    private static void Printer(Double[] @NotNull [] array) {
        System.out.println("\nРезультат интерполяции:");
        for (int i = 0; i < array.length; ++i) {
            System.out.println("X[" + i + "]:= " + array[i][0] + "    f[" + i + "]:= " + array[i][1]);
        }
    }


    public static void waitForEnter() {
        Scanner scanner = new Scanner(System.in);

        System.out.print("\nPress ENTER to proceed.");
        scanner.nextLine();
    }
}
