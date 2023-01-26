using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HD
{
    public class HDMeshSubdivision : MonoBehaviour
    {
        public static List<Vector3[]> subdivide_face_extrude(Vector3[] face_vertices, float height, bool capTop=true)
        {
            //normal = utils_face.face_normal(face)
            //normal = utils_vertex.vertex_scale(normal, height)
            //# calculate vertices
            //new_vertices = []
            //for i in range(len(face.vertices)):
            //    new_vertices.append(utils_vertex.vertex_add(face.vertices[i], normal))
            //# faces
            //new_faces = []
            //if capBottom:
            //    new_faces.append(face)
            //for i in range(len(face.vertices)):
            //    i2 = i + 1
            //    if i2 >= len(face.vertices):
            //        i2 = 0
            //    v0 = face.vertices[i]
            //    v1 = face.vertices[i2]
            //    v2 = new_vertices[i2]
            //    v3 = new_vertices[i]
            //    new_faces.append(Face([v0, v1, v2, v3]))
            //if capTop:
            //    new_faces.append(Face(new_vertices))
            //for new_face in new_faces:
            //    utils_face.face_copy_properties(face, new_face)
            //return new_faces

            Vector3 normal = HDUtilsVertex.face_normal(face_vertices);
            normal *= height;

            List<Vector3> new_vertices = new List<Vector3>();
            foreach (Vector3 v in face_vertices){
                new_vertices.Add(v + normal);
            }

            List<Vector3[]> new_faces_vertices = new List<Vector3[]>();

            for (int i = 0; i < face_vertices.Length; i++)
            {
                Vector3 v0 = face_vertices[i];
                Vector3 v1 = face_vertices[(i + 1)%face_vertices.Length];
                Vector3 v2 = new_vertices[(i + 1) % face_vertices.Length];
                Vector3 v3 = new_vertices[i];

                new_faces_vertices.Add(new Vector3[]{ v0, v1, v2, v3});
            }

            if (capTop)
            {
                new_faces_vertices.Add(new_vertices.ToArray());
            }

            return new_faces_vertices;
        }

        public static List<Vector3[]> subdivide_face_extrude(HDMesh hdMesh, int[] face, float height, bool capTop=true)
        {

            Vector3[] face_vertices = HDUtilsVertex.face_vertices(hdMesh, face);
            List<Vector3[]> new_faces_vertices = subdivide_face_extrude(face_vertices, height, capTop);

            return new_faces_vertices;
        }

    }
}


