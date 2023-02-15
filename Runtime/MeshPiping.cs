using System;
using System.Collections.Generic;
using Mola;
using UnityEngine;

namespace Mola
{
    public class MeshPiping
    {
        public static Vector3 GetCenterAverage(List<Vector3> profile)
        {
            Vector3 center = new Vector3();
            foreach (Vector3 p in profile)
            {
                center = center + p;
            }
            return center / profile.Count;
        }
        public static void AddTriangleFan(MolaMesh mesh, int center, int iStart, int iEnd, bool inverse = false)
        {
            for (int i = iStart; i < iEnd; i++)
            {
                int i2 = i + 1;
                if (i2 >= iEnd) i2 = iStart;
                if (inverse)
                {
                    mesh.AddTriangle(center, i2, i);
                }
                else
                {
                    mesh.AddTriangle(center, i, i2);
                }
            }
        }
        public static MolaMesh PipeLineWithConvexProfile(Vector3 a, Vector3 b, List<Vector3> profile, Vector3 up, bool closeStart, bool closeEnd)
        {
            MolaMesh pipe = new MolaMesh();
            Matrix4x4 m;

            Vector3 from = a;
            Vector3 to = b;
            Vector3 aToB = b - a;
            m = Matrix4x4.LookAt(from, to, up);
            List<Vector3> ring = new List<Vector3>();
            foreach (Vector3 p in profile)
            {
                Vector3 cP = m.MultiplyPoint(p);
                ring.Add(cP);
                pipe.AddVertex(cP.x, cP.y, cP.z);
            }
            foreach (Vector3 cP in ring)
            {
                Vector3 p = cP + aToB;
                pipe.AddVertex(p.x, p.y, p.z);
            }

            int nSegs = profile.Count;
            for (int i = 0; i < pipe.Vertices.Count - nSegs; i += nSegs)
            {
                for (int j = 0; j < profile.Count; j++)
                {
                    int j2 = (j + 1) % profile.Count;
                    pipe.AddQuad(i + j, i + j + nSegs, i + j2 + nSegs, i + j2);
                }
            }

            if (closeStart)
            {
                List<Vector3> startCap = new List<Vector3>();
                for (int i = 0; i < profile.Count; i++)
                {
                    startCap.Add(pipe.Vertices[i]);

                }
                Vector3 center = GetCenterAverage(startCap);
                int iCenter = pipe.AddVertex(center.x, center.y, center.z);
                AddTriangleFan(pipe, iCenter, 0, profile.Count);

            }
            if (closeEnd)
            {
                List<Vector3> endCap = new List<Vector3>();
                int startI = 0;
                for (int i = 0; i < profile.Count; i++)
                {
                    endCap.Add(pipe.Vertices[i + startI]);
                }
                Vector3 center = GetCenterAverage(endCap);
                int iCenter = pipe.AddVertex(center.x, center.y, center.z);
                AddTriangleFan(pipe, iCenter, startI, startI + profile.Count);
            }

            return pipe;
        }
        public static MolaMesh PipePolyLineWithConvexProfile(List<Vector3> nodes, List<Vector3> profile, Vector3 up, bool closeStart, bool closeEnd)
        {
            MolaMesh pipe = new MolaMesh();
            Matrix4x4 m;
            Matrix4x4 mb = Matrix4x4.LookAt(Vector3.zero, new Vector3(0, 0, 1), new Vector3(1, 0, 0));

            // last direction taken from previous direction
            // todo: check closed ring
            for (int i = 0; i < nodes.Count; i++)
            {
                Vector3 from = nodes[i];
                Vector3 to;
                if (i < nodes.Count - 1)
                {
                    to = nodes[i + 1];
                }
                else
                {
                    to = from + from - nodes[i - 1];
                }

                m = Matrix4x4.LookAt(from, to, up);
                // m.
                Matrix4x4 resultm = mb * m;
                foreach (Vector3 p in profile)
                {
                    Vector3 cP = resultm.MultiplyPoint(p);
                    pipe.AddVertex(cP.x, cP.y, cP.z);
                }
            }
            int nSegs = profile.Count;
            for (int i = 0; i < pipe.Vertices.Count - nSegs; i += nSegs)
            {
                for (int j = 0; j < profile.Count; j++)
                {
                    int j2 = (j + 1) % profile.Count;
                    pipe.AddQuad(i + j, i + j + nSegs, i + j2 + nSegs, i + j2);
                }
            }

            if (closeStart)
            {
                List<Vector3> startCap = new List<Vector3>();
                for (int i = 0; i < profile.Count; i++)
                {
                    startCap.Add(pipe.Vertices[i]);

                }
                Vector3 center = GetCenterAverage(startCap);
                int iCenter = pipe.AddVertex(center.x, center.y, center.z);
                AddTriangleFan(pipe, iCenter, 0, profile.Count);

            }
            if (closeEnd)
            {
                List<Vector3> endCap = new List<Vector3>();
                int startI = (nodes.Count - 2) * profile.Count;
                for (int i = 0; i < profile.Count; i++)
                {
                    endCap.Add(pipe.Vertices[i + startI]);
                }
                Vector3 center = GetCenterAverage(endCap);
                int iCenter = pipe.AddVertex(center.x, center.y, center.z);
                AddTriangleFan(pipe, iCenter, startI, startI + profile.Count);
            }

            return pipe;
        }
    }
}



