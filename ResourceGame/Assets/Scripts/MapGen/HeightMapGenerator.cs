using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HeightMapGenerator
{
    public static HeightMap GenerateHeightMap(int width, int height, HeightMapSettings settings, MapRulesSettings mapRules, Vector2 sampleCenter, ChunkBorderInfo borderInfo)
    {
        float[,] values = Noise.GenerateNoiseMap(width, height, settings.noiseSettings, sampleCenter);

        AnimationCurve heightCurveThreadSafe = new AnimationCurve(settings.heightCurve.keys);

        float minValue = float.MaxValue;
        float maxValue = float.MinValue;

        if (mapRules.useFalloff)
        {
            float[,] falloffMap;

            if (mapRules.useFalloff)
            {
                if (mapRules.maxMapSizeInChunks.x <= 1 && mapRules.maxMapSizeInChunks.y <= 1)
                    falloffMap = FalloffGenerator.GenerateFalloffMap(width, mapRules);
                else if (borderInfo.isCorner)
                    falloffMap = FalloffGenerator.GenerateCornerFalloffMap(width, mapRules, borderInfo.corner);
                else if (borderInfo.isEdge)
                    falloffMap = FalloffGenerator.GenerateEdgeFalloffMap(width, mapRules, borderInfo.edge);
                else 
                    falloffMap = new float[width, height];
            } 
            else
                falloffMap = new float[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    values[i, j] = Mathf.Clamp01(values[i, j] - falloffMap[i, j]);

                    values[i, j] *= heightCurveThreadSafe.Evaluate(values[i, j]) * settings.heightMultiplier;

                    if (values[i, j] > maxValue)
                    {
                        maxValue = values[i, j];
                    }
                    if (values[i, j] < minValue)
                    {
                        minValue = values[i, j];
                    }
                }
            }
        } 
        else
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    values[i, j] = Mathf.Clamp01(values[i, j]);

                    values[i, j] *= heightCurveThreadSafe.Evaluate(values[i, j]) * settings.heightMultiplier;

                    if (values[i, j] > maxValue)
                    {
                        maxValue = values[i, j];
                    }
                    if (values[i, j] < minValue)
                    {
                        minValue = values[i, j];
                    }
                }
            }
        }

        return new HeightMap(values, minValue, maxValue);
    }

    public static HeightMap GenerateOcean(int width, int height)
    {
        return new HeightMap(new float[width, height], 0, 1);
    }
}

public struct HeightMap
{
    public readonly float[,] values;
    public readonly float minValue;
    public readonly float maxValue;

    public HeightMap(float[,] values, float minValue, float maxValue)
    {
        this.values = values;
        this.minValue = minValue;
        this.maxValue = maxValue;
    }
}