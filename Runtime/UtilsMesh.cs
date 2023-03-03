using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.ObjectModel;
using System;

namespace Mola
{
    public class UtilsMesh
    {
        /// <summary>
        /// Creates an offset of a mesh.
        /// If `doclose` is `True`, it will create quad faces
        /// along the naked edges of an open input mesh.
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="offset"></param>
        /// <param name="closeborders"></param>
        /// <param name="constrainZ"></param>
        /// <returns></returns>
        public static MolaMesh MeshOffset(MolaMesh mesh, float offset, bool closeborders, bool constrainZ = false)
        {
            // calculate normals per vertex
            // create new vertices and duplicate faces
            // close borders

            int nFaces = mesh.Faces.Count;
            int nVertices = mesh.Vertices.Count;
            Vector3[] normals = UtilsVertex.getVertexNormals(mesh);
            if (constrainZ)
            {
                for (int i = 0; i < normals.Length; i++)
                {
                    Vector3 normal = normals[i];
                    normal.Set(normal.x, normal.y, 0);
                    normal.Normalize();
                    normals[i] = normal;
                }
            }
            for (int i = 0; i < normals.Length; i++)
            {
                Vector3 n = normals[i];
                n *= offset;
                n += mesh.Vertices[i];
                mesh.AddVertex(n.x, n.y, n.z);
            }
            
            for (int i = 0; i < nFaces; i++)
            {
                int[] face = mesh.Faces[i];
                int[] newFace = new int[face.Length];
                for (int j = 0; j < face.Length; j++)
                {
                    newFace[j] = face[face.Length - j - 1] + nVertices;
                }
                mesh.AddFace(newFace);
            }

            if (closeborders)
            {
                mesh.UpdateTopology();
                ReadOnlyCollection<int[]> edges = mesh.GetTopoEdges();
                foreach (int[] edge in edges)
                {
                    int f1 = edge[2];
                    int f2 = edge[3];
                    if (f1 < 0 || f2 < 0)
                    {
                        if (edge[0] < nVertices && edge[1] < nVertices)
                        {
                            int[] face = new int[4];
                            face[3] = edge[0];
                            face[2] = edge[1];
                            face[1] = edge[1] + nVertices;
                            face[0] = edge[0] + nVertices;
                            mesh.AddFace(face);
                        }

                    }
                }
            }
            for (int i = 0; i < mesh.FacesCount(); i++)
            {
                Array.Reverse(mesh.Faces[i]);
            }
            return mesh;
        }
        public static void FillUnitySubMesh(Mesh mesh, List<MolaMesh> molameshes)
        {
            mesh.Clear();
            mesh.subMeshCount = molameshes.Count;
            MolaMesh meshAll = new MolaMesh();
            int iVertices = 0;
            int iMesh = 0;
            foreach (MolaMesh mymesh in molameshes)
            {
                meshAll.AddMesh(mymesh);
            }
            mesh.vertices = meshAll.VertexArray();
            foreach (MolaMesh mymesh in molameshes)
            {
                int[] triangles = mymesh.FlattenedTriangles();
                for (int i = 0; i < triangles.Length; i++)
                {
                    triangles[i] += iVertices;
                }
                mesh.SetTriangles(triangles, iMesh);
                iVertices += mymesh.Vertices.Count;
                iMesh++;
            }

            mesh.RecalculateNormals();
        }
    }
}
