using System;
using System.Collections.Generic;
using UnityEngine;
using HD;
using System.Collections.ObjectModel;

public class VectorUtils
{
    public VectorUtils()
    {
    }
    public static Vector3[] getVertexNormals(HDMesh mesh)
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
    public static HDMesh meshOffset(HDMesh mesh, float offset, bool closeborders, bool constrainZ=false)
    {
        // calculate normals per vertex
        // create new vertices and duplicate faces
        // close borders

        int nFaces = mesh.Faces.Count;
        int nVertices = mesh.Vertices.Count;
        Vector3[] normals = getVertexNormals(mesh);
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
            //mesh.
            ReadOnlyCollection<int[]> edges = mesh.GetTopoEdges();
            foreach (int[] edge in edges)
            {
                int f1 = edge[2];
                int f2 = edge[3];
                if (f1 < 0 || f2 < 0)
                {
                    if (edge[0]<nVertices&& edge[1] < nVertices) { 
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
        return mesh;
    }

    public static void FillUnitySubMesh(Mesh mesh, List<HDMesh> hdmeshes)
    {
        mesh.Clear();
        mesh.subMeshCount = hdmeshes.Count;
        HDMesh meshAll = new HDMesh();
        int iVertices = 0;
        int iMesh = 0;
        foreach (HDMesh mymesh in hdmeshes)
        {
            meshAll.AddMesh(mymesh);
        }
        mesh.vertices = meshAll.VertexArray();
        foreach (HDMesh mymesh in hdmeshes)
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


    public static HDMesh getCone(Vector3 a, Vector3 b, int segments, float radius1,float radius2)
    {
        List<Vector3> profile1 = getCircle(a.x, a.y, radius1, segments,a.z);
        List<Vector3> profile2 = getCircle(b.x, b.y, radius1, segments, b.z);
        HDMesh mesh = new HDMesh();
        mesh.Vertices.AddRange(profile1);
        mesh.Vertices.AddRange(profile2);
        mesh.AddVertex(a.x, a.y, a.z);
        mesh.AddVertex(b.x, b.y, b.z);
        for (int i = 0; i < segments; i++)
        {
            int i2 = (i + 1) % segments;
            int i3 = i + segments;
            int i4 = i2 + segments;
            mesh.AddQuad(i2, i, i3, i4);
        }
        // base;
        int iCenter = segments * 2;
        for (int i = 0; i < segments; i++)
        {
            int i2 = (i + 1) % segments;
            mesh.AddTriangle(i, i2,iCenter);
        }
        //top
        iCenter = segments * 2+1;
        for (int i = 0; i < segments; i++)
        {
            int i2 = (i + 1) % segments+segments;
            mesh.AddTriangle(i+ segments, iCenter, i2);
        }
        return mesh;
    }
    

    
    public static HDMesh getTube(Vector3 a, Vector3 b, int segments, float radius)
    {
        List<Vector3> profile = getCircle(0, 0, radius, segments);

        return HDMeshPiping.PipeLineWithConvexProfile(a, b, profile, new Vector3(0, 1, 0), false, false);
    }
    public static List<Vector3> getLine(Vector3 v1, Vector3 v2 , int segments)
    {
        List<Vector3> profile = new List<Vector3>();
        Vector3 vec = (v2-v1)/ segments;
        for (int i = 0; i < segments; i++)
        {
            Vector3 v  = vec * i + v1;
            profile.Add(v);
        }
        return profile;
    }
    public static List<Vector3> getArc(float angle1,  float angle2, float radius, int segments)
    {
        List<Vector3> profile = new List<Vector3>();
        float deltaAngle = (float)((angle2-angle1) / (segments));
        for (int i = 0; i < segments; i++)
        {
            float cAngle = deltaAngle * i+angle1;
            profile.Add(new Vector3((float)Mathf.Cos(cAngle) * radius , (float)Mathf.Sin(cAngle) * radius , 0));
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
   

    public static void translate(List<Vector3> vectors,float tX, float tY, float tZ)
     {

         for (int i=0;i<vectors.Count;i++)
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
}


