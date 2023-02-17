using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static MolaDirectedGraph;
using Random = UnityEngine.Random;

public class UtilsMath
{
    public static float Remap(float value, float input1, float input2, float output1, float output2)
    {
        return (output2 - output1) * value / (input2 - input1) + output1;
    }
    public static Color RandomColor()
    {
        return new Color(Random.value, Random.value, Random.value);
    }
    /// <summary>
    /// Maps the values of a list from a minimum value to a maximum value.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="toMin"></param>
    /// <param name="toMax"></param>
    /// <returns></returns>
    public static List<float> MapList(List<float> value, float toMin = 0, float toMax = 1)
    {
        float minValue = value.Min();
        float maxValue = value.Max();
        float delta = maxValue - minValue;
        float deltaTarget = toMax - toMin;
        List<float> toValue = value.Select(x => toMin + deltaTarget * (x - minValue) / delta ).ToList();
        return toValue;
    }
    /// <summary>
    /// Maps a value from one range to another.
    /// </summary>
    /// <returns></returns>
    public static float Map(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        float delta = fromMax - fromMin;
        if (Math.Abs(delta) < 1.23E-7) return 0;
        return toMin + ((toMax - toMin) / delta) * (value - fromMin);
    }
}