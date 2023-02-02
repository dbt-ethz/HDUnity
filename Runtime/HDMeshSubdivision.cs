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
        public static HDMesh subdivide_mesh_extrude(HDMesh hdMesh, float extrudeHeight)
        {
            HDMesh newMesh = new HDMesh();
            foreach (var face in hdMesh.Faces) //list of index
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
            //if len(face.vertices) > 4:
            //    print('too many vertices')
            //    return face

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
                        // TODO face_copy_properties(face, f) ? color and group 
                        new_faces_vertices.Add(face);
                    }
                }
            }
            //if len(face.vertices) == 4:
            //    vsU1 = _vertices_between(face.vertices[0], face.vertices[1], nU)
            //    vsU2 = _vertices_between(face.vertices[3], face.vertices[2], nU)
            //    gridVertices = []
            //    for u in range(len(vsU1)):
            //        gridVertices.append(_vertices_between(vsU1[u], vsU2[u], nV))
            //    faces = []
            //    for u in range(len(vsU1) - 1):
            //        vs1 = gridVertices[u]
            //        vs2 = gridVertices[u + 1]
            //        for v in range(len(vs1) - 1):
            //            #f = Face([vs1[v], vs1[v + 1], vs2[v + 1], vs2[v]])
            //            f = Face([vs1[v], vs2[v], vs2[v + 1], vs1[v + 1]])
            //                    utils_face.face_copy_properties(face, f)
            //            faces.append(f)
            //    return faces
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
                    // TODO face_copy_properties(face, f) ? color and group 
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
            //if len(face.vertices) == 3:
            //    vsU1 = _vertices_between(face.vertices[0], face.vertices[1], nU)
            //    vsU2 = _vertices_between(face.vertices[0], face.vertices[2], nU)
            //    gridVertices = []
            //    for u in range(1, len(vsU1)):
            //        gridVertices.append(_vertices_between(vsU1[u], vsU2[u], nV))
            //    faces = []
            //    # triangles
            //    v0 = face.vertices[0]
            //    vs1 = gridVertices[0]
            //    for v in range(len(vs1) - 1):
            //        f = Face([v0, vs1[v], vs1[v + 1]])
            //                utils_face.face_copy_properties(face, f)
            //        faces.append(f)
            //    for u in range(len(gridVertices) - 1):
            //        vs1 = gridVertices[u]
            //        vs2 = gridVertices[u + 1]
            //        for v in range(len(vs1) - 1):
            //            f = Face([vs1[v], vs1[v + 1], vs2[v + 1], vs2[v]])
            //                utils_face.face_copy_properties(face, f)
            //            faces.append(f)
            //    return faces
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
        private static List<Vector3> _vertices_betweem(Vector3 v1, Vector3 v2, int n)
        {
            //row = []
            //deltaV = utils_vertex.vertex_subtract(v2, v1)
            //deltaV = utils_vertex.vertex_divide(deltaV, n)
            //for i in range(n):
            //    addV = utils_vertex.vertex_scale(deltaV, i)
            //    row.append(utils_vertex.vertex_add(addV, v1))
            //row.append(v2)
            //return row

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
        private static List<Vector3[]> subdivie_face_extrude_tapered(Vector3[] face_vertices, float height = 0f, float fraction = 0.5f, bool capTop = true)
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
            //center_vertex = utils_face.face_center(face)
            //normal = utils_face.face_normal(face)
            //scaled_normal = utils_vertex.vertex_scale(normal, height)

            //Vector3 center_vertex = HDUtil

            //# calculate new vertex positions
            //        new_vertices = []
            //for i in range(len(face.vertices)):
            //    n1 = face.vertices[i]
            //    betw = utils_vertex.vertex_subtract(center_vertex, n1)
            //    betw = utils_vertex.vertex_scale(betw, fraction)
            //    nn = utils_vertex.vertex_add(n1, betw)
            //    nn = utils_vertex.vertex_add(nn, scaled_normal)
            //    new_vertices.append(nn)

            //new_faces = []
            //# create the quads along the edges
            //num = len(face.vertices)
            //for i in range(num):
            //    n1 = face.vertices[i]
            //    n2 = face.vertices[(i + 1) % num]
            //    n3 = new_vertices[(i + 1) % num]
            //    n4 = new_vertices[i]
            //    new_face = Face([n1, n2, n3, n4])
            //                new_faces.append(new_face)

            //# create the closing cap face
            //        if doCap:
            //    cap_face = Face(new_vertices)
            //    new_faces.append(cap_face)

            //for new_face in new_faces:
            //    utils_face.face_copy_properties(face, new_face)
            //return new_faces
            Vector3 normal = HDUtilsVertex.face_normal(face_vertices);
        }
    }

}



