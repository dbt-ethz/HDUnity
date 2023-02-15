using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mola
{
    public class UtilsVertex
    {
        public static Vector3[] face_vertices(MolaMesh mesh, int[] face)
        {
            Vector3[] face_v = new Vector3[face.Length];
            for (int i = 0; i < face.Length; i++)
            {
                face_v[i] = mesh.Vertices[face[i]];
            }

            return face_v;
        }
        public static Vector3 vertices_list_center(List<Vector3> vertices)
        {
            Vector3 vSum = new Vector3(0, 0, 0);
            foreach (var vertex in vertices)
            {
                vSum += vertex;
            }

            return vSum / vertices.Count;
        }
        public static Vector3 vertex_center(Vector3 v1, Vector3 v2)
        {
            return (v1 + v2) / 2;
        }
        public static Vector3[] getVertexNormals(MolaMesh mesh)
        {
            Vector3[] normals = new Vector3[mesh.Vertices.Count];
            int[] nFaces = new int[mesh.Vertices.Count];
            for (int i = 0; i < normals.Length; i++)
            {
                normals[i] = new Vector3();
            }
            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                int[] face = mesh.Faces[i];
                for (int j = 0; j < face.Length; j++)
                {
                    int j2 = (j + 1) % face.Length;
                    int j3 = (j2 + 1) % face.Length;
                    int v1 = face[j];
                    int v2 = face[j2];
                    int v3 = face[j3];
                    Vector3 u = mesh.Vertices[v2] - mesh.Vertices[v1];
                    Vector3 v = mesh.Vertices[v3] - mesh.Vertices[v1];
                    Vector3 normal = Vector3.Cross(u, v);
                    normal.Normalize();
                    nFaces[v1] += 1;
                    normals[v1] += normal;

                }
            }
            for (int i = 0; i < normals.Length; i++)
            {
                normals[i] = normals[i] / nFaces[i];
                normals[i].Normalize();
            }
            return normals;
        }
        public static Vector3 polarRad(float angle, float radius)
        {
            //float a=Mathf.Rad2Deg
            return new Vector3(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle), 0);
        }
        public static Vector3 polarRadWithZ(float angle, float radius, float z)
        {
            //float a=Mathf.Rad2Deg
            return new Vector3(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle), z);
        }
        public static Vector3 getBetweenRel(Vector3 p1, Vector3 p2, float mag)
        {
            Vector3 v = p2 - p1;
            return v * mag + p1;
        }
        /// <summary>
        /// finds a position vector between v1 and v2 by a factor (0.0 to 1.0 corresponds to v1 to v2)
        /// and returns the result as a new Vertex.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="factor"></param>
        /// <returns></returns>
        public static Vector3 vertex_between_rel(Vector3 v1, Vector3 v2, float factor)
        {
            return new Vector3((v2.x - v1.x) * factor + v1.x, (v2.y - v1.y) * factor + v1.y, (v2.z - v1.z) * factor + v1.z);
        }
        public static List<Vector3> getLine(Vector3 v1, Vector3 v2, int segments)
        {
            List<Vector3> profile = new List<Vector3>();
            Vector3 vec = (v2 - v1) / segments;
            for (int i = 0; i < segments; i++)
            {
                Vector3 v = vec * i + v1;
                profile.Add(v);
            }
            return profile;
        }
        public static List<Vector3> getArc(float angle1, float angle2, float radius, int segments)
        {
            List<Vector3> profile = new List<Vector3>();
            float deltaAngle = (float)((angle2 - angle1) / (segments));
            for (int i = 0; i < segments; i++)
            {
                float cAngle = deltaAngle * i + angle1;
                profile.Add(new Vector3((float)Mathf.Cos(cAngle) * radius, (float)Mathf.Sin(cAngle) * radius, 0));
            }
            return profile;
        }
        public static List<Vector3> getCircle(float cX, float cY, float radius, int segments, float z = 0)
        {
            List<Vector3> profile = new List<Vector3>();
            float deltaAngle = (float)(2 * Math.PI / segments);
            for (int i = 0; i < segments; i++)
            {
                float cAngle = deltaAngle * i;
                profile.Add(new Vector3((float)Mathf.Cos(cAngle) * radius + cX, (float)Mathf.Sin(cAngle) * radius + cY, z));
            }
            return profile;
        }
        public static void translate(List<Vector3> vectors, float tX, float tY, float tZ)
        {

            for (int i = 0; i < vectors.Count; i++)
            {
                Vector3 v = vectors[i];
                v.Set(v.x + tX, v.y + tY, v.z + tZ);
                vectors[i] = v;

            }
        }
        public static List<Vector3> Rotate(float degrees, Vector3 axis, List<Vector3> vertices)
        {
            List<Vector3> newVertices = new List<Vector3>();
            Quaternion quat = Quaternion.AngleAxis(degrees, axis);
            for (int i = 0; i < vertices.Count; i++)
            {
                newVertices.Add(Rotated(vertices[i], quat));
            }
            return newVertices;
        }
        public static Vector3 Rotated(Vector3 vector, Quaternion rotation, Vector3 pivot = default(Vector3))
        {
            return rotation * (vector - pivot) + pivot;
        }
        /// <summary>
        /// finds a position vector between v1 and v2 by an absolute distance value from v1
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="dis"></param>
        /// <returns></returns>
        public static Vector3 vertex_between_abs(Vector3 v1, Vector3 v2, float dis)
        {
            float d = Vector3.Distance(v1, v2);
            return vertex_between_rel(v1, v2, dis / d);
        }
        /// <summary>
        /// law of cosines
        /// </summary>
        /// <param name="vPrevious"></param>
        /// <param name="v"></param>
        /// <param name="vNext"></param>
        /// <returns></returns>
        public static float vertex_angle_triangle(Vector3 vPrevious, Vector3 v, Vector3 vNext)
        {
            float vvn = Vector3.Distance(v, vNext);
            float vvp = Vector3.Distance(vPrevious, v);
            float vnvp = Vector3.Distance(vNext, vPrevious);
            return (float)Math.Acos((vvn * vvn + vvp * vvp - vnvp * vnvp) / (2 * vvn * vvp));
        }

    }

}

