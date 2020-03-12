using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FalloffGenerator
{
    public enum Corner {
        TOPLEFT,
        TOPRIGHT,
        BOTTOMLEFT,
        BOTTOMRIGHT
    }

    public enum Edge
    {
        TOP,
        LEFT,
        RIGHT,
        BOTTOM
    }

    public static float[,] GenerateFalloffMap(int size, MapRulesSettings mapRulesSettings)
    {
        float[,] map = new float[size, size];

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float x = i / (float)size * 2 - 1;
                float y = j / (float)size * 2 - 1;

                float value = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
                map[i, j] = Evaluate(value, mapRulesSettings);
            }
        }

        return map;
    }
    
    public static float[,] GenerateCornerFalloffMap(int size, MapRulesSettings mapRulesSettings, Corner corner)
    {
        float[,] map = new float[size, size];

        for (int i = 0; i < size / 2; i++)
        {
            for (int j = 0; j < size / 2; j++)
            {
                float x = i / (float)size * 2 - 1;
                float y = j / (float)size * 2 - 1;

                float value = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
                map[i, j] = Evaluate(value, mapRulesSettings);
            }
        }

        for (int i = 0; i < size / 2; i++)
        {
            for (int j = size / 2; j < size; j++)
            {
                float x = i / (float)size * 2 - 1;

                float value = Mathf.Max(Mathf.Abs(x), 0);
                map[i, j] = Evaluate(value, mapRulesSettings);
            }
        }

        for (int j = 0; j < size / 2; j++)
        {
            for (int i = size / 2; i < size; i++)
            {
                float y = j / (float)size * 2 - 1;

                float value = Mathf.Max(0, Mathf.Abs(y));
                map[i, j] = Evaluate(value, mapRulesSettings);
            }
        }

        if (corner == Corner.BOTTOMLEFT)
        {
            Utils<float>.rotateMatrix(map);
        }
        else if (corner == Corner.BOTTOMRIGHT)
        {
            for (int i = 0; i < 2; i++)
                Utils<float>.rotateMatrix(map);
        }
        else if (corner == Corner.TOPRIGHT)
        {
            for (int i = 0; i < 3; i++)
                Utils<float>.rotateMatrix(map);
        }

        return map;
    }

    static float Evaluate(float value, MapRulesSettings mapRulesSettings)
    {
        float a = mapRulesSettings.slope;
        float b = mapRulesSettings.shift;

        return Mathf.Pow(value, a) / (Mathf.Pow(value, a) + Mathf.Pow(b - b * value, a));
    }
}
