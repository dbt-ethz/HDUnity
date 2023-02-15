using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
}