using System;
using UnityEngine;

public static class Util
{
    //Array com cada cor de cada ID, para diferenciar chaves e fechaduras
    public static Color[] colorId = new Color[] {Color.yellow, Color.blue, Color.green, Color.red, Color.gray, Color.white, Color.cyan, Color.black};

    public static float LogNormalization(float value, float minValue, float maxValue, float minNormalized, float maxNormalized)
    {
        return (Mathf.Log(value - minValue) / Mathf.Log(maxValue - minValue)) * (maxNormalized - minNormalized);
    }

    public const int nSpawnPointsVer = 4;
    public const int nSpawnPointsHor = 4;
    public const int distFromBorder = 1;

    public const int defaultRoomSizeX = 22;
    public const int defaultRoomSizeY = 19;

    public enum PlayerProjectileEnum
    {
        STRAIGHT = 0,
        SIN = 1,
        TRIPLE = 2
    }
}
