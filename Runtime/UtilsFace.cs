using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Mola
{
    public class UtilsFace
    {
        public static Vector3 FaceNormal(Vector3[] face_vertices)
        {
            //"""
            //Returns the normal of a face, a vector of length 1 perpendicular to the plane of the triangle.

            //Arguments:
            //----------
            //face : mola.Face
            //    the face to get the normal from
            //"""
            //return utils_vertex.TriangleNormal(face.vertices[0], face.vertices[1], face.vertices[2])

            return TriangleNormal(face_vertices[0], face_vertices[1], face_vertices[2]);
        }
        public static Vector3 TriangleNormal(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            //"""
            //Returns the normal of a triangle defined by 3 vertices.
            //The normal is a vector of length 1 perpendicular to the plane of the triangle.

            //Arguments:
            //----------
            //v1, v2, v3: mola.Vertex
            //   the vertices get the normal from
            //"""

            Vector3 v = v2 - v1;
            Vector3 u = v3 - v1;
            Vector3 crossProduct = Vector3.Cross(v, u);
            crossProduct.Normalize();

            return crossProduct;
        }
        public static Vector3 FaceCenter(Vector3[] face_vertices)
        {
            List<Vector3> vertices_list = new List<Vector3>(face_vertices);
            return UtilsVertex.vertices_list_center(vertices_list);
        }
        public static float FaceCenterY(Vector3[] face_verties)
        {
            return FaceCenter(face_verties).y;
        }
        public static Vector3 FaceCenter(MolaMesh molaMesh, int[] face)
        {
            Vector3[] face_vertices = UtilsVertex.face_vertices(molaMesh, face);
            return FaceCenter(face_vertices);
        }
        /// <summary>
        /// Returns the altitude, 0 if the face is vertical, -Pi/2 if it faces downwards, +Pi/2 if it faces upwards.
        /// </summary>
        /// <param name="face_vertices"></param>
        /// <returns></returns>
        public static float FaceAngleVertical(Vector3[] face_vertices)
        {
            Vector3 n = FaceNormal(face_vertices);
            return (float)Math.Asin(n.y);
        }
        /// <summary>
        /// Returns the altitude, 0 if the face is vertical, -Pi/2 if it faces downwards, +Pi/2 if it faces upwards.
        /// </summary>
        /// <param name="molaMesh"></param>
        /// <param name="face"></param>
        /// <returns></returns>
        public static float FaceAngleVertical(MolaMesh molaMesh, int[] face)
        {
            Vector3[] face_vertices = UtilsVertex.face_vertices(molaMesh, face);
            Vector3 n = FaceNormal(face_vertices);
            return (float)Math.Asin(n.y);
        }
        /// <summary>
         /// Returns the azimuth, the orientation of the face around the y-axis in the XZ-plane
         /// </summary>
         /// <param name="face_vertices"></param>
         /// <returns></returns>
        public static float FaceAngleHorizontal(Vector3[] face_vertices)
        {
            Vector3 n = FaceNormal(face_vertices);
            return (float)Math.Atan2(n.z, n.x);
        }
        /// <summary>
        /// Returns the azimuth, the orientation of the face around the y-axis in the XZ-plane
        /// </summary>
        /// <param name="molaMesh"></param>
        /// <param name="face"></param>
        /// <returns></returns>
        public static float FaceAngleHorizontal(MolaMesh molaMesh, int[] face)
        {
            Vector3[] face_vertices = UtilsVertex.face_vertices(molaMesh, face);
            Vector3 n = FaceNormal(face_vertices);
            return (float)Math.Atan2(n.z, n.x);
        }
        /// <summary>
        /// Assigns a color to all the faces by values,
        /// from smallest(red) to biggest(purple).
        /// </summary>
        public static void ColorFaceByValue(MolaMesh mesh, List<int[]> faces, List<float> values, bool doGrayScale=false)
        {
            if (faces.Count != values.Count)
            {
                throw new ArgumentException("face count and value count doesnt match!");
            }
            float valueMin = values.Min();
            float valueMax = values.Max();

            for (int i = 0; i < faces.Count; i++)
            {
                float value = UtilsMath.Map(values[i], valueMin, valueMax, 0f, 1);
                foreach (int v in faces[i])
                {
                    mesh.Colors[v] = Color.HSVToRGB(value, 1, 1);
                }
            }
        }
        /// <summary>
        /// Assigns a color to all the faces by values,
        /// from smallest(red) to biggest(purple).
        /// </summary>
        public static void ColorFaceByValue(MolaMesh mesh, List<float> values, bool doGrayScale = false)
        {
            ColorFaceByValue(mesh, mesh.Faces, values, doGrayScale);
        }
    }
}