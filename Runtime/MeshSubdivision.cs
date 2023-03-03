using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace Mola
{
    public class MeshSubdivision : MonoBehaviour
    {
        private static List<Vector3> VerticesBetween(Vector3 v1, Vector3 v2, int n)
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
        private static List<Vector3> VerticesFrame(Vector3 v1, Vector3 v2, float w1, float w2)
        {
            Vector3 p1 = UtilsVertex.vertex_between_abs(v1, v2, w1);
            Vector3 p2 = UtilsVertex.vertex_between_abs(v2, v1, w2);
            return new List<Vector3>() { v1, p1, p2, v2 };
        }
        /// <summary>
        /// Apply Catmull–Clark algorithm to a MolaMesh
        /// </summary>
        /// <param name="mesh"></param>
        /// <returns></returns>
        public static MolaMesh SubdivideMeshCatmullClark(MolaMesh mesh)
        {
            mesh.WeldVertices();
            mesh.UpdateTopology();
            
            List<bool> vertex_fix = Enumerable.Repeat(false, mesh.VertexCount()).ToList();

            // get face vertex
            List<Vector3> face_vertices = new List<Vector3>();
            foreach (int[] face in mesh.Faces)
            {
                Vector3 v = UtilsFace.FaceCenter(mesh, face);
                face_vertices.Add(v);
            }

            // get edge vertex
            ReadOnlyCollection<int[]> topoEdges = mesh.GetTopoEdges();
            List<Vector3> edge_vertices = new List<Vector3>();
            foreach (int[] edge in topoEdges)
            {
                if (edge[2] < 0 || edge[3] < 0)
                {
                    vertex_fix[edge[0]] = true;
                    vertex_fix[edge[1]] = true;
                    Vector3 edge_center = UtilsVertex.vertex_center(mesh.Vertices[edge[0]], mesh.Vertices[edge[1]]);
                    edge_vertices.Add(edge_center);
                    vertex_fix.Add(true);
                }
                else
                {
                    Vector3 vSum = new Vector3(0, 0, 0);
                    int nElements = 2;
                    vSum += mesh.Vertices[edge[0]];
                    vSum += mesh.Vertices[edge[1]];
                    if(edge[2] >= 0)
                    {
                        vSum += face_vertices[edge[2]];
                        nElements += 1;
                    }
                    if(edge[3] >= 0)
                    {
                        vSum += face_vertices[edge[3]];
                        nElements += 1;
                    }
                    vSum /= nElements;
                    edge_vertices.Add(vSum);
                    if (vertex_fix[0] && vertex_fix[1])
                    {
                        vertex_fix.Add(true);
                    }
                    else vertex_fix.Add(false);
                }
            }

            // get vertex vertex
            ReadOnlyCollection<int[]> topoVertexEdges = mesh.GetTopoVertexEdges();
            List<Vector3> vertex_vertices = new List<Vector3>();
            for (int i = 0; i < mesh.VertexCount(); i++)
            {
                if (vertex_fix[i])
                {
                    vertex_vertices.Add(mesh.Vertices[i]);
                }
                else
                {
                    Vector3 averageFaces = new Vector3(0, 0, 0);
                    Vector3 averageEdges = new Vector3(0, 0, 0);
                    int nEdges = topoVertexEdges[i].Length;

                    foreach (int edge in topoVertexEdges[i])
                    {
                        int face = topoEdges[edge][2];
                        if (topoEdges[edge][1] == i) face = topoEdges[edge][3];
                        if (face >= 0)
                        {
                            averageFaces += face_vertices[face];
                        }
                        Vector3 edge_center = UtilsVertex.vertex_center(mesh.Vertices[topoEdges[edge][0]], mesh.Vertices[topoEdges[edge][1]]);
                        averageEdges += edge_center;
                    }
                    averageEdges *= (2.0f / nEdges);
                    averageFaces *= (1.0f / nEdges);

                    Vector3 v = new Vector3(mesh.Vertices[i].x, mesh.Vertices[i].y, mesh.Vertices[i].z);
                    v *= nEdges - 3;
                    v += averageFaces;
                    v += averageEdges;
                    v *= (1.0f / nEdges);
                    vertex_vertices.Add(v);
                }
            }

            // collect new faces
            MolaMesh newMesh = new MolaMesh();
            for (int i = 0; i < mesh.FacesCount(); i++)
            {
                int[] face = mesh.Faces[i];
                int v1 = face[^2];
                int v2 = face[^1];
                foreach (int v3 in face)
                {
                    int edge1 = mesh.AdjacentEdgeToVertices(v1, v2);
                    int edge2 = mesh.AdjacentEdgeToVertices(v2, v3);
                    if (edge1 >= 0 && edge2 >= 0)
                    {
                        Vector3[] newFace = new Vector3[] { edge_vertices[edge1], vertex_vertices[v2], edge_vertices[edge2], face_vertices[i] };
                        newMesh.AddFace(newFace);
                    }
                    v1 = v2;
                    v2 = v3;
                }
            }
            newMesh.WeldVertices();
            newMesh.UpdateTopology();
            return newMesh;
        }
        /// <summary>
        /// Extrudes the face straight by distance height.
        /// </summary>
        /// <param name="face_vertices">An array of Vector3 representing a MolaMesh face</param>
        /// <param name="height">The extrusion distance, default 0</param>
        /// <param name="capTop">Toggle if top face (extrusion face) should be created, default True</param>
        /// <returns></returns>
        public static List<Vector3[]> SubdivideFaceExtrude(Vector3[] face_vertices, float height, bool capTop = true)
        {
            Vector3 normal = UtilsFace.FaceNormal(face_vertices);
            normal *= height;

            List<Vector3> new_vertices = new List<Vector3>();
            foreach (Vector3 v in face_vertices) {
                new_vertices.Add(v + normal);
            }

            List<Vector3[]> new_faces_vertices = new List<Vector3[]>();

            for (int i = 0; i < face_vertices.Length; i++)
            {
                Vector3 v0 = face_vertices[i];
                Vector3 v1 = face_vertices[(i + 1) % face_vertices.Length];
                Vector3 v2 = new_vertices[(i + 1) % face_vertices.Length];
                Vector3 v3 = new_vertices[i];

                new_faces_vertices.Add(new Vector3[] { v0, v1, v2, v3 });
            }

            if (capTop)
            {
                new_faces_vertices.Add(new_vertices.ToArray());
            }

            return new_faces_vertices;
        }
        /// <summary>
        /// Extrudes the face straight by distance height.
        /// </summary>
        /// <param name="molaMesh"></param>
        /// <param name="face"></param>
        /// <param name="height"></param>
        /// <param name="capTop"></param>
        /// <returns></returns>
        public static List<Vector3[]> SubdivideFaceExtrude(MolaMesh molaMesh, int[] face, float height, bool capTop = true)
        {

            Vector3[] face_vertices = UtilsVertex.face_vertices(molaMesh, face);
            List<Vector3[]> new_faces_vertices = SubdivideFaceExtrude(face_vertices, height, capTop);

            return new_faces_vertices;
        }
        /// <summary>
        /// Extrudes the all faces in a MolaMesh straight by distance height.
        /// </summary>
        /// <param name="molaMesh"></param>
        /// <param name="extrudeHeight"></param>
        /// <returns></returns>
        public static MolaMesh SubdivideMeshExtrude(MolaMesh molaMesh, float extrudeHeight)
        {
            MolaMesh newMesh = new MolaMesh();
            foreach (var face in molaMesh.Faces)
            {
                List<Vector3[]> new_faces_vertices = MeshSubdivision.SubdivideFaceExtrude(molaMesh, face, extrudeHeight);
                foreach (var face_vertices in new_faces_vertices)
                {
                    newMesh.AddFace(face_vertices);
                }
            }
            return newMesh;
        }
        /// <summary>
        /// splits a triangle, quad or a rectangle into a regular grid
        /// </summary>
        /// <param name="face_vertices"></param>
        /// <param name="nU"></param>
        /// <param name="nV"></param>
        /// <returns></returns>
        public static List<Vector3[]> SubdivideFaceGrid(Vector3[] face_vertices, int nU, int nV)
        {
            List<Vector3[]> new_faces_vertices = new List<Vector3[]>();
            if (face_vertices.Length == 4)
            {
                List<Vector3> vsU1 = VerticesBetween(face_vertices[0], face_vertices[1], nU);
                List<Vector3> vsU2 = VerticesBetween(face_vertices[3], face_vertices[2], nU);

                List<List<Vector3>> gridVertices = new List<List<Vector3>>();
                for (int i = 0; i < vsU1.Count; i++)
                {
                    gridVertices.Add(VerticesBetween(vsU1[i], vsU2[i], nV));
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
                List<Vector3> vsU1 = VerticesBetween(face_vertices[0], face_vertices[1], nU);
                List<Vector3> vsU2 = VerticesBetween(face_vertices[0], face_vertices[2], nU);

                List<List<Vector3>> gridVertices = new List<List<Vector3>>();
                for (int u = 0; u < vsU1.Count; u++)
                {
                    gridVertices.Add(VerticesBetween(vsU1[u], vsU2[u], nV));
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
        /// <summary>
        /// splits a triangle, quad or a rectangle into a regular grid
        /// </summary>
        /// <param name="molaMesh"></param>
        /// <param name="face"></param>
        /// <param name="nU"></param>
        /// <param name="nV"></param>
        /// <returns></returns>
        public static List<Vector3[]> SubdivideFaceGrid(MolaMesh molaMesh, int[] face, int nU, int nV)
        {
            Vector3[] face_vertices = UtilsVertex.face_vertices(molaMesh, face);
            List<Vector3[]> new_faces_vertices = SubdivideFaceGrid(face_vertices, nU, nV);

            return new_faces_vertices;
        }
        /// <summary>
        /// splits all triangle or quad faces in a MolaMesh into a regular grid
        /// </summary>
        /// <param name="molaMesh"></param>
        /// <param name="nU"></param>
        /// <param name="nV"></param>
        /// <returns></returns>
        public static MolaMesh SubdivideMeshGrid(MolaMesh molaMesh, int nU, int nV)
        {
            MolaMesh newMesh = new MolaMesh();
            foreach (var face in molaMesh.Faces) //list of index
            {
                List<Vector3[]> new_faces_vertices = SubdivideFaceGrid(molaMesh, face, nU, nV);
                foreach (var face_vertices in new_faces_vertices)
                {
                    newMesh.AddFace(face_vertices);
                }
            }
            return newMesh;
        }
        /// <summary>
        /// Extrudes the face tapered like a window by creating an
        /// offset face and quads between every original edge and the
        /// corresponding new edge.
        /// </summary>
        /// <param name="face_vertices"></param>
        /// <param name="height"></param>
        /// <param name="fraction"></param>
        /// <param name="capTop"></param>
        /// <returns></returns>
        public static List<Vector3[]> SubdivideFaceExtrudeTapered(Vector3[] face_vertices, float height = 0f, float fraction = 0.5f, bool capTop = true)
        {
            Vector3 center_vertex = UtilsFace.FaceCenter(face_vertices);
            Vector3 normal = UtilsFace.FaceNormal(face_vertices);
            Vector3 scaled_normal = normal * height;

            //# calculate new vertex positions
            List<Vector3> new_vertices = new List<Vector3>();
            for (int i = 0; i < face_vertices.Length; i++)
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
        /// <summary>
        /// Extrudes the face tapered like a window by creating an
        /// offset face and quads between every original edge and the
        /// corresponding new edge.
        /// </summary>
        /// <param name="molaMesh"></param>
        /// <param name="face"></param>
        /// <param name="height"></param>
        /// <param name="fraction"></param>
        /// <param name="capTop"></param>
        /// <returns></returns>
        public static List<Vector3[]> SubdivideFaceExtrudeTapered(MolaMesh molaMesh, int[] face, float height = 0f, float fraction = 0.5f, bool capTop = true)
        {
            Vector3[] face_vertices = UtilsVertex.face_vertices(molaMesh, face);
            List<Vector3[]> new_faces_vertices = SubdivideFaceExtrudeTapered(face_vertices, height, fraction, capTop);

            return new_faces_vertices;
        }
        /// <summary>
        /// Extrudes all face in a MolaMesh tapered like a window by 
        /// creating an offset face and quads between every original 
        /// edge and the corresponding new edge.
        /// </summary>
        /// <param name="molaMesh"></param>
        /// <param name="height"></param>
        /// <param name="fraction"></param>
        /// <param name="capTop"></param>
        /// <returns></returns>
        public static MolaMesh SubdivideMeshExtrudeTapered(MolaMesh molaMesh, float height = 0f, float fraction = 0.5f, bool capTop = true)
        {
            MolaMesh newMesh = new MolaMesh();
            foreach (var face in molaMesh.Faces)
            {
                List<Vector3[]> new_faces_vertices = SubdivideFaceExtrudeTapered(molaMesh, face, height, fraction, capTop);
                foreach (var face_vertices in new_faces_vertices)
                {
                    newMesh.AddFace(face_vertices);
                }
            }
            return newMesh;
        }
        /// <summary>
        /// Extrudes all face in a MolaMesh tapered like a window by
        /// creating an offset face and quads between every original 
        /// edge and the corresponding new edge.
        /// </summary>
        /// <param name="molaMesh"></param>
        /// <param name="heights"></param>
        /// <param name="fractions"></param>
        /// <param name="capTops"></param>
        /// <returns></returns>
        public static MolaMesh SubdivideMeshExtrudeTapered(MolaMesh molaMesh, List<float> heights, List<float> fractions, List<bool> capTops)
        {
            MolaMesh newMesh = new MolaMesh();
            for (int i = 0; i < molaMesh.Faces.Count; i++)
            {
                List<Vector3[]> new_faces_vertices = SubdivideFaceExtrudeTapered(molaMesh, molaMesh.Faces[i], heights[i], fractions[i], capTops[i]);
                foreach (var face_vertices in new_faces_vertices)
                {
                    newMesh.AddFace(face_vertices);
                }
            }
            return newMesh;
        }
        /// <summary>
        /// Extrudes a pitched roof
        /// </summary>
        /// <param name="face_vertices"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static List<Vector3[]> SubdivideFaceSplitRoof(Vector3[] face_vertices, float height = 0f)
        {
            List<Vector3[]> new_faces_vertices = new List<Vector3[]>();

            Vector3 normal = UtilsFace.FaceNormal(face_vertices);
            normal *= height;

            if (face_vertices.Length == 4)
            {
                Vector3 ev1 = UtilsVertex.vertex_center(face_vertices[0], face_vertices[1]);
                ev1 += normal;
                Vector3 ev2 = UtilsVertex.vertex_center(face_vertices[2], face_vertices[3]);
                ev2 += normal;

                new_faces_vertices.Add(new Vector3[] { face_vertices[0], face_vertices[1], ev1 });
                new_faces_vertices.Add(new Vector3[] { face_vertices[1], face_vertices[2], ev2, ev1 });
                new_faces_vertices.Add(new Vector3[] { face_vertices[2], face_vertices[3], ev2 });
                new_faces_vertices.Add(new Vector3[] { face_vertices[3], face_vertices[0], ev1, ev2 });

                return new_faces_vertices;

            }
            else if (face_vertices.Length == 3)
            {
                Vector3 ev1 = UtilsVertex.vertex_center(face_vertices[0], face_vertices[1]);
                ev1 += normal;
                Vector3 ev2 = UtilsVertex.vertex_center(face_vertices[1], face_vertices[2]);
                ev2 += normal;

                new_faces_vertices.Add(new Vector3[] { face_vertices[0], face_vertices[1], ev1 });
                new_faces_vertices.Add(new Vector3[] { face_vertices[1], ev2, ev1 });
                new_faces_vertices.Add(new Vector3[] { face_vertices[1], face_vertices[2], ev2 });
                new_faces_vertices.Add(new Vector3[] { face_vertices[2], face_vertices[0], ev1, ev2 });

                return new_faces_vertices;
            }
            else
            {
                throw new ArgumentException("face has to be quad or triangle");
            }
        }
        /// <summary>
        /// Extrudes a pitched roof
        /// </summary>
        /// <param name="molaMesh"></param>
        /// <param name="face"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static List<Vector3[]> SubdivideFaceSplitRoof(MolaMesh molaMesh, int[] face, float height = 0f)
        {
            Vector3[] face_vertices = UtilsVertex.face_vertices(molaMesh, face);
            List<Vector3[]> new_faces_vertices = SubdivideFaceSplitRoof(face_vertices, height);

            return new_faces_vertices;
        }
        /// <summary>
        /// Extrudes all faces in a MolaMesh into pitched rooves
        /// </summary>
        /// <param name="molaMesh"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static MolaMesh SubdivideMeshSplitRoof(MolaMesh molaMesh, float height = 0f)
        {
            MolaMesh newMesh = new MolaMesh();
            foreach (var face in molaMesh.Faces)
            {
                List<Vector3[]> new_faces_vertices = SubdivideFaceSplitRoof(molaMesh, face, height);
                foreach (var face_vertices in new_faces_vertices)
                {
                    newMesh.AddFace(face_vertices);
                }
            }
            return newMesh;
        }
        /// <summary>
        /// Extrudes the face to a point by creating a
        /// triangular face from each edge to the point.
        /// </summary>
        /// <param name="face_vertices">The face to be extruded</param>
        /// <param name="point">The point to extrude to</param>
        /// <returns></returns>
        public static List<Vector3[]> SubdivideFaceExtrudeToPoint(Vector3[] face_vertices, Vector3 point)
        {
            List<Vector3[]> new_faces_vertices = new List<Vector3[]>();

            int numV = face_vertices.Length;
            for (int i = 0; i < numV; i++)
            {
                Vector3 v1 = face_vertices[i];
                Vector3 v2 = face_vertices[(i + 1) % numV];
                new_faces_vertices.Add(new Vector3[] { v1, v2, point});
            }

            return new_faces_vertices;
        }
        /// <summary>
        /// Extrudes the face to a point by creating a
        /// triangular face from each edge to the point.
        /// </summary>
        /// <param name="molaMesh"></param>
        /// <param name="face"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static List<Vector3[]> SubdivideFaceExtrudeToPoint(MolaMesh molaMesh, int[] face, Vector3 point)
        {
            Vector3[] face_vertices = UtilsVertex.face_vertices(molaMesh, face);
            List<Vector3[]> new_faces_vertices = SubdivideFaceExtrudeToPoint(face_vertices, point);

            return new_faces_vertices;
        }
        /// <summary>
        /// Extrudes the face to the center point moved by height
        /// normal to the face and creating a triangular face from
        /// each edge to the point.
        /// </summary>
        /// <param name="face_vertices"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static List<Vector3[]> SubdivideFaceExtrudeToPointCenter(Vector3[] face_vertices, float height= 0f)
        {
            List<Vector3[]> new_faces_vertices = new List<Vector3[]>();

            Vector3 normal = UtilsFace.FaceNormal(face_vertices);
            normal *= height;

            Vector3 center = UtilsFace.FaceCenter(face_vertices);
            center += normal;

            return SubdivideFaceExtrudeToPoint(face_vertices, center);
        }
        /// <summary>
        /// Extrudes the face to the center point moved by height
        /// normal to the face and creating a triangular face from
        /// each edge to the point.
        /// </summary>
        /// <param name="molaMesh"></param>
        /// <param name="face"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static List<Vector3[]> SubdivideFaceExtrudeToPointCenter(MolaMesh molaMesh, int[] face, float height = 0f)
        {
            Vector3[] face_vertices = UtilsVertex.face_vertices(molaMesh, face);
            List<Vector3[]> new_faces_vertices = SubdivideFaceExtrudeToPointCenter(face_vertices, height);

            return new_faces_vertices;
        }
        /// <summary>
        /// Extrudes each face in a MolaMesh to the center point moved 
        /// by height normal to the face and creating a triangular face
        /// from each edge to the point.
        /// </summary>
        /// <param name="molaMesh"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static MolaMesh SubdivideMeshExtrudeToPointCenter(MolaMesh molaMesh, float height = 0f)
        {
            MolaMesh newMesh = new MolaMesh();
            foreach (var face in molaMesh.Faces)
            {
                List<Vector3[]> new_faces_vertices = SubdivideFaceExtrudeToPointCenter(molaMesh, face, height);
                foreach (var face_vertices in new_faces_vertices)
                {
                    newMesh.AddFace(face_vertices);
                }
            }
            return newMesh;
        }
        /// <summary>
        /// Creates an offset frame with quad corners. Works only with convex shapes.
        /// </summary>
        /// <param name="face_vertices"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        public static List<Vector3[]> SubdivideFaceSplitFrame(Vector3[] face_vertices, float w)
        {
            List<Vector3[]> new_faces_vertices = new List<Vector3[]>();
            List<Vector3> innerVertices = new List<Vector3>();
            for(int i = 0; i < face_vertices.Length; i++)
            {
                Vector3 vp;
                if (i == 0)
                {
                    vp = face_vertices[face_vertices.Length - 1];
                }
                else
                {
                    vp = face_vertices[i - 1];
                }
                Vector3 v = face_vertices[i];
                Vector3 vn = face_vertices[(i + 1) % face_vertices.Length];
                Vector3 vnn = face_vertices[(i + 2) % face_vertices.Length];

                float th1 = UtilsVertex.vertex_angle_triangle(vp, v, vn);
                float th2 = UtilsVertex.vertex_angle_triangle(v, vn, vnn);

                float w1 = w / (float)Math.Sin(th1);
                float w2 = w / (float)Math.Sin(th2);

                List<Vector3> vs1 = VerticesFrame(v, vn, w1, w2);
                List<Vector3> vs2 = VerticesFrame(VerticesFrame(vp, v, w1, w1)[2], VerticesFrame(vn, vnn, w2, w2)[1], w1, w2);

                innerVertices.Add(vs2[1]);

                Vector3[] f1 = new Vector3[] { vs1[0], vs2[0], vs2[1], vs1[1]};
                Vector3[] f2 = new Vector3[] { vs1[1], vs2[1], vs2[2], vs1[2]};
                new_faces_vertices.Add(f1);
                new_faces_vertices.Add(f2);
            }

            Vector3[] fInner = innerVertices.ToArray();
            new_faces_vertices.Add(fInner);

            return new_faces_vertices;
        }
        /// <summary>
        /// Creates an offset frame with quad corners. Works only with convex shapes.
        /// </summary>
        /// <param name="molaMesh"></param>
        /// <param name="face"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        public static List<Vector3[]> SubdivideFaceSplitFrame(MolaMesh molaMesh, int[] face, float w)
        {
            Vector3[] face_vertices = UtilsVertex.face_vertices(molaMesh, face);
            List<Vector3[]> new_faces_vertices = SubdivideFaceSplitFrame(face_vertices, w);

            return new_faces_vertices;
        }
        /// <summary>
        /// For each face in a MolaMesh, creates an offset frame with quad corners. 
        /// Works only with convex shapes.
        /// </summary>
        /// <param name="molaMesh"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        public static MolaMesh SubdivideMeshSplitFrame(MolaMesh molaMesh, float w)
        {
            MolaMesh newMesh = new MolaMesh();
            foreach (var face in molaMesh.Faces)
            {
                List<Vector3[]> new_faces_vertices = SubdivideFaceSplitFrame(molaMesh, face, w);
                foreach (var face_vertices in new_faces_vertices)
                {
                    newMesh.AddFace(face_vertices);
                }
            }
            return newMesh;
        }
        /// <summary>
        /// Subidivide face into cells with absolute size
        /// </summary>
        /// <param name="face_vertices"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static List<Vector3[]> SubdivideFaceSplitGridAbs(Vector3[] face_vertices, float x, float y)
        {
            int u = (int)(Vector3.Distance(face_vertices[0], face_vertices[1]) / x);
            int v = (int)(Vector3.Distance(face_vertices[1], face_vertices[2]) / y);
            if (u == 0) u = 1;
            if (v == 0) v = 1;

            return SubdivideFaceGrid(face_vertices, u, v);
        }
        /// <summary>
        /// Subidivide face into cells with absolute size
        /// </summary>
        /// <param name="molaMesh"></param>
        /// <param name="face"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static List<Vector3[]> SubdivideFaceSplitGridAbs(MolaMesh molaMesh, int[] face, float x, float y)
        {
            Vector3[] face_vertices = UtilsVertex.face_vertices(molaMesh, face);
            List<Vector3[]> new_faces_vertices = SubdivideFaceSplitGridAbs(face_vertices, x, y);

            return new_faces_vertices;
        }
        /// <summary>
        /// Subidivide all faces in a MolaMesh into cells with absolute size
        /// </summary>
        /// <param name="molaMesh"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static MolaMesh SubdivideMeshSplitGridAbs(MolaMesh molaMesh, float x, float y)
        {
            MolaMesh newMesh = new MolaMesh();
            foreach (var face in molaMesh.Faces)
            {
                List<Vector3[]> new_faces_vertices = SubdivideFaceSplitGridAbs(molaMesh, face, x, y);
                foreach (var face_vertices in new_faces_vertices)
                {
                    newMesh.AddFace(face_vertices);
                }
            }
            return newMesh;
        }
    } 
}