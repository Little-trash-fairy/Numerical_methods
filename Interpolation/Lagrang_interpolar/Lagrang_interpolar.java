package com.company;

public class Lagrang_interpolar {

    public static double Lagrang_create_elem(int n, double x,Double[][] array)
    {
        double sum=0;
        for (int i=0; i<n; i+=2)
        {
            double accum=1;
            for (int j=0; j<n; j+=2)
            {
                if (i!=j)
                {
                    accum*=(x-array[j][0])/(array[i][0]-array[j][0]);
                }
            }
            sum+=accum*array[i][1];
        }
        return sum;
    }

    public static void Lagrang_create_mass(Double[][] table)
    {
        for(int i =1;i<table.length-1;i+=2)
        {
            table[i][1]=Lagrang_create_elem(table.length,table[i][0],table);
        }
    }
}
