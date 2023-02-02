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

    }
}

