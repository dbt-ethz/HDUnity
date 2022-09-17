using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HD
{
    public class HDUtilsVertex : MonoBehaviour
    {
        public static Vector3[] face_vertices(HDMesh mesh, int[] face)
        {
            Vector3[] face_v = new Vector3[face.Length];
            for (int i = 0; i < face.Length; i++)
            {
                face_v[i] = mesh.vertices[face[i]];
            }

            return face_v;
        }
        
        public static Vector3 face_normal(Vector3[] face_vertices)
        {
            //"""
            //Returns the normal of a face, a vector of length 1 perpendicular to the plane of the triangle.

            //Arguments:
            //----------
            //face : mola.Face
            //    the face to get the normal from
            //"""
            //return utils_vertex.triangle_normal(face.vertices[0], face.vertices[1], face.vertices[2])

            return triangle_normal(face_vertices[0], face_vertices[1], face_vertices[2]);
        }

        public static Vector3 triangle_normal(Vector3 v1, Vector3 v2, Vector3 v3)
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

    }
}

