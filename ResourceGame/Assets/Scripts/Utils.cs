using System.Collections;
using System.Collections.Generic;

public static class Utils<T>
{
    // An Inplace function to rotate a N x N matrix by 90 degrees in anti-clockwise direction 
    public static void rotateMatrix(T[,] array)
    {
        int n = array.GetLength(0);

        // Consider all squares one by one 
        for (int x = 0; x < n / 2; x++)
        {
            // Consider elements in group of 4 in current square 
            for (int y = x; y < n - x - 1; y++)
            {
                // store current cell in temp variable 
                T temp = array[x, y];

                // move values from right to top 
                array[x, y] = array[y, n - 1 - x];

                // move values from bottom to right 
                array[y, n - 1 - x] = array[n - 1 - x, n - 1 - y];

                // move values from left to bottom 
                array[n - 1 - x, n - 1 - y] = array[n - 1 - y, x];

                // assign temp to left 
                array[n - 1 - y, x] = temp;
            }
        }
    }
}
