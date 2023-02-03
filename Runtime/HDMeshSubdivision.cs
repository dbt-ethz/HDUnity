using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HD
{
    public class HDMeshSubdivision : MonoBehaviour
    {
        private static List<Vector3> _vertices_betweem(Vector3 v1, Vector3 v2, int n)
        {
            List<Vector3> rowList = new List<Vector3>();
            Vector3 deltaV = (v2 - v1) / n;
            for (int i = 0; i < n; i++)
            {
                Vector3 addV = deltaV * i + v1;
                rowList.Add(addV);
            }
            rowList.Add(v2);

            return rowList;
        }
        public static List<Vector3[]> subdivide_face_extrude(Vector3[] face_vertices, float height, bool capTop=true)
        {
            Vector3 normal = HDUtilsFace.face_normal(face_vertices);
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
        public static HDMesh subdivide_mesh_extrude(HDMesh hdMesh, float extrudeHeight)
        {
            HDMesh newMesh = new HDMesh();
            foreach (var face in hdMesh.Faces) 
            {
                List<Vector3[]> new_faces_vertices = HDMeshSubdivision.subdivide_face_extrude(hdMesh, face, extrudeHeight);
                foreach (var face_vertices in new_faces_vertices)
                {
                    newMesh.AddFace(face_vertices);
                }
            }
            return newMesh;
        }
        public static List<Vector3[]> subdivide_face_grid(Vector3[] face_vertices, int nU, int nV)
        {
            //        """
            //splits a triangle, quad or a rectangle into a regular grid
            //"""

            List<Vector3[]> new_faces_vertices = new List<Vector3[]>();
            if (face_vertices.Length == 4)
            {
                List<Vector3> vsU1 = _vertices_betweem(face_vertices[0], face_vertices[1], nU);
                List<Vector3> vsU2 = _vertices_betweem(face_vertices[3], face_vertices[2], nU);

                List<List<Vector3>> gridVertices = new List<List<Vector3>>();
                for (int i = 0; i < vsU1.Count; i++)
                {
                    gridVertices.Add(_vertices_betweem(vsU1[i], vsU2[i], nV));
                }

                for (int u = 0; u < vsU1.Count - 1; u++)
                {
                    List<Vector3> vs1 = gridVertices[u];
                    List<Vector3> vs2 = gridVertices[u + 1];
                    for (int v = 0; v < vs1.Count - 1; v++)
                    {
                        Vector3[] face = new Vector3[] { vs1[v], vs2[v], vs2[v + 1], vs1[v + 1] };
                        new_faces_vertices.Add(face);
                    }
                }
            }

            else if (face_vertices.Length == 3)
            {
                List<Vector3> vsU1 = _vertices_betweem(face_vertices[0], face_vertices[1], nU);
                List<Vector3> vsU2 = _vertices_betweem(face_vertices[0], face_vertices[2], nU);

                List<List<Vector3>> gridVertices = new List<List<Vector3>>();
                for (int u = 0; u < vsU1.Count; u++)
                {
                    gridVertices.Add(_vertices_betweem(vsU1[u], vsU2[u], nV));
                }

                Vector3 v0 = face_vertices[0];
                List<Vector3> vs1 = gridVertices[0];

                for (int i = 0; i < vs1.Count - 1; i++)
                {
                    Vector3[] face = new Vector3[] { v0, vs1[i], vs1[i + 1] };
                    new_faces_vertices.Add(face);
                    for (int u = 0; u < gridVertices.Count - 1; u++)
                    {
                        vs1 = gridVertices[u];
                        List<Vector3> vs2 = gridVertices[u + 1];
                        for (int v = 0; v < vs1.Count - 1; v++)
                        {
                            face = new Vector3[] { vs1[v], vs1[v + 1], vs2[v + 1], vs2[v] };
                            new_faces_vertices.Add(face);
                        }
                    }
                }
            }
            return new_faces_vertices;

        }
        public static List<Vector3[]> subdivide_face_grid(HDMesh hdMesh, int[] face, int nU, int nV)
        {
            Vector3[] face_vertices = HDUtilsVertex.face_vertices(hdMesh, face);
            List<Vector3[]> new_faces_vertices = subdivide_face_grid(face_vertices, nU, nV);

            return new_faces_vertices;
        }
        public static HDMesh subdivide_mesh_grid(HDMesh hdMesh, int nU, int nV)
        {
            HDMesh newMesh = new HDMesh();
            foreach (var face in hdMesh.Faces) //list of index
            {
                List<Vector3[]> new_faces_vertices = subdivide_face_grid(hdMesh, face, nU, nV);
                foreach (var face_vertices in new_faces_vertices)
                {
                    newMesh.AddFace(face_vertices);
                }
            }
            return newMesh;
        }
        public static List<Vector3[]> subdivide_face_extrude_tapered(Vector3[] face_vertices, float height = 0f, float fraction = 0.5f, bool capTop = true)
        {
            //        """
            //Extrudes the face tapered like a window by creating an
            //offset face and quads between every original edge and the
            //corresponding new edge.

            //Arguments:
            //----------
            //face : mola.core.Face
            //    The face to be extruded
            //height: float
            //   The distance of the new face to the original face, default 0
            //fraction: float
            //   The relative offset distance, 0: original vertex, 1: center point
            //    default 0.5(halfway)
            //"""

            Vector3 center_vertex = HDUtilsFace.face_center(face_vertices);
            Vector3 normal = HDUtilsFace.face_normal(face_vertices);
            Vector3 scaled_normal = normal * height;

            //# calculate new vertex positions
            List<Vector3> new_vertices = new List<Vector3>();
            for(int i = 0; i < face_vertices.Length; i++)
            {
                Vector3 n1 = face_vertices[i];
                Vector3 betw = center_vertex - n1;
                betw *= fraction;
                Vector3 nn = n1 + betw;
                nn += scaled_normal;
                new_vertices.Add(nn);
            }

            //# create the quads along the edges
            List<Vector3[]> new_faces_vertices = new List<Vector3[]>();
            int num = face_vertices.Length;
            for (int i = 0; i < num; i++)
            {
                Vector3 n1 = face_vertices[i];
                Vector3 n2 = face_vertices[(i + 1) % num];
                Vector3 n3 = new_vertices[(i + 1) % num];
                Vector3 n4 = new_vertices[i];
                Vector3[] new_face_vertices = new Vector3[] { n1, n2, n3, n4 };
                new_faces_vertices.Add(new_face_vertices);
            }

            //# create the closing cap face
            if (capTop)
            {
                Vector3[] cap_face_vertices = new_vertices.ToArray();
                new_faces_vertices.Add(cap_face_vertices);
            }

            return new_faces_vertices;
        }
        public static List<Vector3[]> subdivide_face_extrude_tapered(HDMesh hdMesh, int[] face, float height = 0f, float fraction = 0.5f, bool capTop = true)
        {
            Vector3[] face_vertices = HDUtilsVertex.face_vertices(hdMesh, face);
            List<Vector3[]> new_faces_vertices = subdivide_face_extrude_tapered(face_vertices, height, fraction, capTop);

            return new_faces_vertices;
        }
        public static HDMesh subdivide_mesh_extrude_tapered(HDMesh hdMesh, float height = 0f, float fraction = 0.5f, bool capTop = true)
        {
            HDMesh newMesh = new HDMesh();
            foreach (var face in hdMesh.Faces)
            {
                List<Vector3[]> new_faces_vertices = HDMeshSubdivision.subdivide_face_extrude_tapered(hdMesh, face, height, fraction, capTop);
                foreach (var face_vertices in new_faces_vertices)
                {
                    newMesh.AddFace(face_vertices);
                }
            }
            return newMesh;
        }
    }

}



