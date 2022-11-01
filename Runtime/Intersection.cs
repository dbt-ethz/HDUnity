using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HDDirectedGraph;

public class Intersection 
{

	public static Nullable<Vector2> LineLineIntersection(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
	{
		return LineLineIntersection(a1.x, a1.y, a2.x, a2.y, b1.x, b1.y, b2.x, b2.y);
	}

	public static Nullable<Vector2> LineLineIntersection(float aX, float aY, float bX,
			float bY, float cX, float cY, float dX, float dY)
	{
		double denominator = ((bX - aX) * (dY - cY)) - ((bY - aY) * (dX - cX));
		if (denominator == 0)
			return null;// parallel
		double numerator = ((aY - cY) * (dX - cX)) - (aX - cX) * (dY - cY);
		double r = numerator / denominator;
		double x = aX + r * (bX - aX);
		double y = aY + r * (bY - aY);
		return new Vector2((float)x, (float)y);
	}

public static Nullable<Vector2> LineLineIntersectionDir(Vector2 org1,Vector2 dir1,
		Vector2 org2,Vector2 dir2)
	{
		float denominator = dir1.x * dir2.y - dir1.y * dir2.x;
		if (denominator == 0)
			return null;// parallel
		float numerator = (org1.y - org2.y) * dir2.x - (org1.x - org2.x) * dir2.y;
		float r = numerator / denominator;
		return org1 + r * dir1;
		
	}
	
	public static Vector2? RaySegment(Vector2 org,Vector2 dir,
		Vector2 c,Vector2 d)
	{
		float denominator = dir.x * (d.y - c.y) - (dir.y) * (d.x - c.x);
		if (denominator == 0) { 
			//Nullable<Vector2> result = null;
			return null;
		}
		float numerator = (org.y - c.y) * (d.x - c.x) - (org.x - c.x) * (d.y - c.y);
		float numerator2 = (org.y - c.y) * dir.x - (org.x - c.x) * dir.y;
		float r = numerator / denominator;
		float s = numerator2 / denominator;
		if (s < 0 || s > 1 || r <= 0)
        {
			//Nullable<Vector2> result = null;
			return null;
		}
		//return null;// colinear
		Vector2 intersection= org + r * dir;
		return intersection;
	}

	

	
}
