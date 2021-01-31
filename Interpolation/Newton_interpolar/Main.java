package com.company;

import java.util.Scanner;

import static com.company.Newton_interpolar.Newton_create_mass;


public class Main {
    public static Double[][] array = null;

    public static void main(String[] args) {

        Read_data_and_create_matrix();

        if (array != null) {
            Newton_create_mass();
            Printer();
        } else {
            System.out.println("Ошибка ввода");
        }
        waitForEnter();
    }

    private static void Read_data_and_create_matrix() {
        try {
            Scanner in = new Scanner(System.in);
            System.out.print("Введите кол-во узлов:\nn:=");
            int n = in.nextInt();

            array = new Double[(n * 2) - 1][2];

            int counter = 0;
            for (int i = 0; i < array.length; i += 2) {

                System.out.print("Введите узел\nX[" + counter + "]:=");
                array[i][0] = in.nextDouble();
                System.out.print("Введите значение функции в узле\nF[" + counter + "]:=");
                array[i][1] = in.nextDouble();
                counter++;
            }
            for (int i = 1; i < array.length - 1; i += 2) {
                double k = (array[i - 1][0] + array[i + 1][0]) / 2;
                array[i][0] = k;
            }

        } catch (Exception e) {
            array = null;
            e.printStackTrace();
        }
    }

    private static void Printer() {
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
