using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier
	
{
    public Vector3[] controlPoints;


     public List<Vector3> CalculateQuadtraticBezier(int SEGMENT_COUNT)
    {
        List<Vector3>polyLine=new List<Vector3>();
        int curveCount = (int)controlPoints.Length / 3;
        for (int j = 0; j <curveCount; j++)
        {
            for (int i = 1; i <= SEGMENT_COUNT; i++)
            {
                float t = i / (float)SEGMENT_COUNT;
                int nodeIndex = j * 3;

                Vector3 p=CalculateCubicBezierPoint(t, controlPoints [nodeIndex], controlPoints [nodeIndex + 1], controlPoints [nodeIndex + 2], controlPoints [nodeIndex + 3]);
            polyLine.Add(p);
            }
        }
        return polyLine;
    }
    public List<Vector3> CalculateCubicBezier(int SEGMENT_COUNT)
    {
        List<Vector3>polyLine=new List<Vector3>();
        int curveCount = (int)controlPoints.Length / 3;
        for (int j = 0; j <curveCount; j++)
        {
            for (int i = 1; i <= SEGMENT_COUNT; i++)
            {
                float t = i / (float)SEGMENT_COUNT;
                int nodeIndex = j * 3;

                Vector3 p=CalculateCubicBezierPoint(t, controlPoints [nodeIndex], controlPoints [nodeIndex + 1], controlPoints [nodeIndex + 2], controlPoints [nodeIndex + 3]);
            polyLine.Add(p);
            }
        }
        return polyLine;
    }

    Vector3 CalculateCubicBezierPoint(float t){
         int curveCount = (int)controlPoints.Length / 3;
         int cCurve=(int)(curveCount/t);
         float segmentDomain=1f/curveCount;
         float localT=t-cCurve*segmentDomain;
         int nodeIndex = cCurve * 3;
         return CalculateCubicBezierPoint(localT, controlPoints [nodeIndex], controlPoints [nodeIndex + 1], controlPoints [nodeIndex + 2], controlPoints [nodeIndex + 3]);
    }

    Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;
        
        Vector3 p = uuu * p0; 
        p += 3 * uu * t * p1; 
        p += 3 * u * tt * p2; 
        p += ttt * p3; 
        
        return p;
    }


    Vector3 CalculateQuadtraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u=1-t;
        return  (u*u)*p0+(2*u)*t*p1+(t*t)*p2;
    }
}