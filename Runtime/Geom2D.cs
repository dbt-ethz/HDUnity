using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HDDirectedGraph;

public class Geom2D
	
{
    public static Vector2 RelFromAtoB(Vector2 a, Vector2 b, float parameter)
    {
        return (b - a) * parameter + a;
    }

    public static Vector2[] OffsetSegment(Vector2 v1, Vector2 v2, float offset)
    {
        Vector2 v12 = v2 - v1;
        Vector2 v = v12.Rotate(90).normalized * offset;
        return new Vector2[] { v1 + v, v2 + v };
    }

    static float AreaParallel(Vector2 a, Vector2 b, Vector2 c)
    {
        return (a.x - b.x) * (a.y - c.y) - (a.x - c.x) * (a.y - b.y);
    }

    public static bool IsLeft(Vector2 A, Vector2 B, Vector2 C)
    {

        return AreaParallel(A, B, C) > 0;
    }
    
}
public static class Vector2Extension
{
    public Vector2 FromPolar(float radius,float degrees){
        float x = radius* Math.cos(degrees);
        float y = radius* Math.sin(degrees);
       return  new Vector2(x, x);
    }
    public static Vector2 Rotate(this Vector2 v, float degrees)
    {

        float radians = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }
    public static Vector2 SetMag(this Vector2 v,float targetLength)
    {
        return v * targetLength/ v.magnitude;
    }
}