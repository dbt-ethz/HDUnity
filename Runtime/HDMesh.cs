using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;
namespace HD
{
	public class HDMesh
	{
		List<Vector3> vertices;
		List<int[]> faces;
		private List<Color> vertexColors;
		private List<Vector2> uvs;

		private List<int[]> topoEdges;
		private List<int[]> topoVertexEdges;


		static int VERTEX1 = 0;
		static int VERTEX2 = 1;
		static int FACE1 = 2;
		static int FACE2 = 3;
		//static int NONE = -1;

		public List<Vector2> UVs {get=> uvs; set => uvs= value;}
		public List<int[]> Faces {get=> faces; set => faces= value;}
		public List<Vector3> Vertices {get=> vertices; set => vertices= value;}
		public List<Color> Colors { get => vertexColors; set => vertexColors = value; }

		public HDMesh()
		{
			this.vertices = new List<Vector3>();
			this.faces = new List<int[]>();
			this.Colors = new List<Color>();
		}

		public HDMesh Copy()
        {
			HDMesh copyMesh = new HDMesh();
			foreach (Vector3 vertex in vertices)
			{
				copyMesh.AddVertex(vertex.x,vertex.y,vertex.z);
			}

			foreach (int[] face in faces)
            {
				copyMesh.AddFace((int[])face.Clone());
			}

			foreach (Color color in vertexColors)
			{
				copyMesh.AddColor(color);
			}
			return copyMesh;

		}

		public void SetVertexColors(Color color)
		{
			for (int i = 0; i < vertexColors.Count; i++)
			{
				vertexColors[i] = color;
			}
		}

		public void Translate(float x,float y,float z)
        {
			for (int i = 0; i < vertices.Count; i++)
            {
				Vector3 v = vertices[i];
				vertices[i] = new Vector3(v.x + x, v.y + y, v.z + z);

			}
        }

		public void Scale(float x, float y, float z)
		{
			for (int i = 0; i < vertices.Count; i++)
			{
				Vector3 v = vertices[i];
				vertices[i] = new Vector3(v.x * x, v.y * y, v.z * z);

			}
		}

		public void AddMesh(HDMesh mesh)
        {
			int nV = this.VertexCount();
			for (int i = 0; i < mesh.vertices.Count; i++)
            {
				vertices.Add(mesh.vertices[i]);
				Colors.Add(mesh.Colors[i]);
			}
			if (uvs!=null&&mesh.UVs!=null){
				for (int i = 0; i < mesh.vertices.Count; i++)
            	{
					uvs.Add(mesh.UVs[i]);
				
				}
			}

			for (int i = 0; i < mesh.faces.Count; i++)
			{
				int[] face = mesh.faces[i];
				int[] newFace = new int[face.Length];
				for (int j = 0; j < face.Length; j++)
                {
					newFace[j] = face[j] + nV;

				}
				faces.Add(newFace);
			}
		}

		public int AddVertex(float x, float y, float z)
		{
			vertices.Add(new Vector3(x, y, z));
			return vertices.Count - 1;
		}
		public int AddVertex(Vector3 v,Color color)
		{
			vertices.Add(v);
			Colors.Add(color);
			return vertices.Count - 1;
		}
		public int AddVertex(float x, float y, float z, Color color)
		{
			vertices.Add(new Vector3(x, y, z));
			Colors.Add(color);
			return vertices.Count - 1;
		}
		public void AddFace(int[] face)
		{
			faces.Add(face);
		}
		

		public int AddColor(Color color)
		{
			this.Colors.Add(color);
			return Colors.Count - 1;
		}
		public int[] AddVertices(IList<Vector3> vertices, IList<Color> colors)
		{
			int[] vs = new int[vertices.Count];
			for (int i = 0; i < vertices.Count; i++)
			{
				Vector3 vector = vertices[i];
				vs[i] = this.AddVertex(vector.x, vector.y, vector.z);
				this.AddColor(colors[i]);
			}
			return vs;
		}
		public int[] AddVertices(IList<Vector3> vertices, Color color)
		{
			int[] vs = new int[vertices.Count];
			for (int i = 0; i < vertices.Count; i++)
			{
				Vector3 vector = vertices[i];
				vs[i] = this.AddVertex(vector.x, vector.y, vector.z);
				this.AddColor(color);
			}
			return vs;
		}
		public int[] AddVertices(float x1, float y1, float z1, float x2, float y2, float z2, float x3, float y3, float z3)
		{
			int[] vs = new int[3];
			vs[0] = this.AddVertex(x1, y1, z1);
			vs[1] = this.AddVertex(x2, y2, z2);
			vs[2] = this.AddVertex(x3, y3, z3);
			return vs;
		}
		public int[] AddVertices(float x1, float y1, float z1, float x2, float y2, float z2, float x3, float y3, float z3, float x4, float y4, float z4)
		{
			int[] vs = new int[4];
			vs[0] = this.AddVertex(x1, y1, z1);
			vs[1] = this.AddVertex(x2, y2, z2);
			vs[2] = this.AddVertex(x3, y3, z3);
			vs[3] = this.AddVertex(x4, y4, z4);
			return vs;
		}
		public int VertexCount()
		{
			return vertices.Count;
		}
		public int FacesCount()
		{
			return faces.Count;
		}

		public void AddTriangle(int index1, int index2, int index3)
		{
			faces.Add(new int[] { index1, index2, index3 });
		}
		public void AddQuad(int index1, int index2, int index3, int index4)
		{
			faces.Add(new int[] { index1, index2, index3, index4 });
		}

		public void FlipFaces()
		{
			for (int i = 0; i < faces.Count; i++)

			{
				int[] face = faces[i];
				int[] face2 = new int[face.Length];

				for (int j = 0; j < face.Length; j++)
                {
					face2[j] = face[face.Length - 1 - j];

				}
				faces[i] = face2;
			}

		}
		public void FlipYZ()
        {
			for (int i=0;i<vertices.Count;i++)
				
			{
					Vector3 v = vertices[i];
					vertices[i] = new Vector3(v.x,v.z,v.y);
			}
			
        }
		public void AddQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, Color color)
		{
			int[] vs = new int[4];
			vs[0] = this.AddVertex(v1,color);
			vs[1] = this.AddVertex(v2, color);
			vs[2] = this.AddVertex(v3, color);
			vs[3] = this.AddVertex(v4, color);
			faces.Add(vs);
		}
		public void AddTri2D(float x1,float y1,float x2,float y2,float x3,float y3)
		{
			AddTri2D(x1,y1,x2,y2,x3,y3,0,Color.white);
		}
		public void AddTri2D(float x1, float y1, float x2, float y2, float x3, float y3,float z)
        {
			AddTri2D(x1, y1, x2, y2, x3, y3, z, Color.white);

		}
		public void AddTri2D(float x1, float y1, float x2, float y2, float x3, float y3, float z,Color color)
		{
			int[] vs = new int[3];
			vs[0] = this.AddVertex(x1, y1, z, color);
			vs[1] = this.AddVertex(x2, y2, z, color);
			vs[2] = this.AddVertex(x3, y3, z, color);
			faces.Add(vs);
		}

		public void AddTriangle(float x1, float y1,float z1, float x2, float y2,float z2, float x3, float y3, float z3, Color color1, Color color2, Color color3)
		{
			int[] vs = new int[3];
			vs[0] = this.AddVertex(x1, y1, z1, color1);
			vs[1] = this.AddVertex(x2, y2, z2, color2);
			vs[2] = this.AddVertex(x3, y3, z3, color3);
			faces.Add(vs);
		}
		public void AddTri2D(float x1, float y1, float x2, float y2, float x3, float y3, float z, Color color1,Color color2,Color color3)
		{
			int[] vs = new int[3];
			vs[0] = this.AddVertex(x1, y1, z, color1);
			vs[1] = this.AddVertex(x2, y2, z, color2);
			vs[2] = this.AddVertex(x3, y3, z, color3);
			faces.Add(vs);
		}


		public void AddQuad2D(float x1, float y1, float x2, float y2, float x3, float y3,float x4,float y4, float z , Color color)
		{
			int[] vs = new int[4];
			vs[0] = this.AddVertex(x1, y1, z, color);
			vs[1] = this.AddVertex(x2, y2, z, color);
			vs[2] = this.AddVertex(x3, y3, z, color);
			vs[3] = this.AddVertex(x4, y4, z, color);
			faces.Add(vs);
		}


		public void SeparateVerticesWithUVs()
		{
			List<Vector3> oldVertices = new List<Vector3>(vertices);
			List<Vector2> oldUVS = new List<Vector2>(uvs);
			List<Color>listColors = new List<Color>(Colors);
			this.vertices.Clear();
			this.Colors.Clear();
			this.uvs.Clear();
			int vertexIndex=0;
			foreach (int[] face in faces)
			{
				for (int i = 0; i < face.Length; i++)
				{
					vertexIndex=face[i];
					this.vertices.Add(oldVertices[vertexIndex]);
					this.Colors.Add(listColors[vertexIndex]);
					this.uvs.Add(oldUVS[vertexIndex]);
					face[i] = this.vertices.Count - 1;
				}
			}
		}

		public void SeparateVertices()
		{
			List<Vector3> oldVertices = new List<Vector3>(vertices);
			List<Color>listColors = new List<Color>(Colors);
			this.vertices.Clear();
			this.Colors.Clear();
			foreach (int[] face in faces)
			{
				for (int i = 0; i < face.Length; i++)
				{

					Vector3 v = oldVertices[face[i]];

					this.vertices.Add(v);
					if (listColors.Count> face[i])
                    {
                        this.Colors.Add(listColors[face[i]]);
                    }
					
					face[i] = this.vertices.Count - 1;
				}
			}
			
		}
		public void TriangulateQuads()
		{
			List<int[]> triangles = new List<int[]>();
			foreach (int[] face in faces)
			{
				if (face.Length == 3)
				{
					triangles.Add(face);
				}
				if (face.Length == 4)
				{
					triangles.Add(new int[] { face[0], face[1], face[2] });
					triangles.Add(new int[] { face[2], face[3], face[0] });
				}
			}
			this.faces = triangles;
		}
		public Vector3[] VertexArray()
		{
			return vertices.ToArray();
		}
		public int[] FlattenedTriangles()
		{
			TriangulateQuads();
			int[] indices = new int[faces.Count * 3];
			int index = 0;
			foreach (int[] face in faces)
			{
				indices[index] = face[0];
				index++;
				indices[index] = face[1];
				index++;
				indices[index] = face[2];
				index++;
			}
			return indices;
		}




		void UpdateTopology()
		{
			// TODO Auto-generated method stub
			topoEdges = new List<int[]>();
			topoVertexEdges = new List<int[]>(vertices.Count);
			for (int i = 0; i < vertices.Count; i++)
			{
				topoVertexEdges.Add(new int[] { });
			}
			for (int faceIndex = 0; faceIndex < faces.Count; faceIndex++)
			{
				int[] face = faces[faceIndex];
				int nPts = face.Length;
				for (int j = 0; j < nPts; j++)
				{
					int v1Index = face[j];
					int v2Index = face[(j + 1) % nPts];
					int edgeIndex = AddEdge(v1Index, v2Index);
					// this only works for clean meshes
					AttachFaceToEdge(edgeIndex, v1Index, faceIndex);
				}
			}
		}

		private void AttachEdgeToVertex(int vertexIndex, int edgeIndex)
		{
			int[] edges = topoVertexEdges[vertexIndex];
			int[] newEdges = new int[edges.Length + 1];
			for (int i = 0; i < edges.Length; i++)
			{
				newEdges[i] = edges[i];
			}
			newEdges[edges.Length] = edgeIndex;
			topoVertexEdges[vertexIndex] = newEdges;
		}

		private void AttachFaceToEdge(int edgeIndex, int vertexIndex, int faceIndex)
		{
			int[] edge = topoEdges[edgeIndex];
			if (edge[0] == vertexIndex)
			{
				edge[FACE1] = faceIndex;
			}
			else if (edge[1] == vertexIndex)
			{
				edge[FACE2] = faceIndex;
			}
		}

		private int AddEdge(int v1, int v2)
		{
			int existingEdge = AdjacentEdgeToVertices(v1, v2);
			if (existingEdge >= 0)
			{
				return existingEdge;
			}
			int[] edgeVs = new int[] { v1, v2, -1, -1 };
			topoEdges.Add(edgeVs);
			AttachEdgeToVertex(v1, topoEdges.Count);
			AttachEdgeToVertex(v2, topoEdges.Count);
			return topoEdges.Count - 1;
		}
		private int AdjacentEdgeToVertices(int v1, int v2)
		{
			int[] vEdges = topoVertexEdges[v1];
			for (int i = 0; i < vEdges.Length; i++)
			{
				int edgeIndex = vEdges[i];
				int[] edge = topoEdges[edgeIndex];
				if (edge[VERTEX1] == v2 || edge[VERTEX2] == v2)
				{
					return edgeIndex;
				}
			}
			return -1;
		}
		private int AdjacentFacetoVertices(int v1, int v2)
		{
			int edgeIndex = AdjacentEdgeToVertices(v1, v2);
			int[] edge = topoEdges[edgeIndex];
			if (edge[0] == v1) return topoEdges[edgeIndex][FACE1];
			if (edge[1] == v1) return topoEdges[edgeIndex][FACE2];
			return -1;
		}
		private int[] AdjacentFacesToFace(int faceIndex)
		{
			int[] face = faces[faceIndex];
			int nPts = face.Length;
			int[] faceNbs = new int[nPts];
			for (int j = 0; j < nPts; j++)
			{
				int v1Index = face[j];
				int v2Index = face[(j + 1) % nPts];
				faceNbs[j] = AdjacentFacetoVertices(v1Index, v2Index);
			}
			return faceNbs;
		}
		private int[] AdjacentVerticesToVertex(int vertexIndex)
		{
			int[] vEdges = topoVertexEdges[vertexIndex];
			int[] nbVertices = new int[vEdges.Length];
			for (int i = 0; i < vEdges.Length; i++)
			{
				int[] eVertices = topoEdges[vEdges[i]];
				if (eVertices[VERTEX1] == vertexIndex)
				{
					nbVertices[i] = eVertices[1];
				}
				else if (eVertices[VERTEX2] == vertexIndex)
				{
					nbVertices[i] = eVertices[0];
				}
			}
			return nbVertices;
		}
		private int[] AdjacentFacesToVertex(int vertexIndex)
		{
			int[] vEdges = topoVertexEdges[vertexIndex];
			int[] allFaceIds = new int[vEdges.Length];
			int nFaces = 0;
			for (int i = 0; i < vEdges.Length; i++)
			{
				int edgeIndex = vEdges[i];
				int faceIndex = this.adjacentFace1ToEdge(edgeIndex);
				allFaceIds[i] = faceIndex;
				if (faceIndex >= 0)
				{
					nFaces++;
				}
			}
			int[] faceIds = new int[nFaces];
			int index = 0;
			for (int i = 0; i < allFaceIds.Length; i++)
			{
				if (allFaceIds[i] >= 0)
				{
					faceIds[index] = allFaceIds[i];
					index++;
				}
			}
			return faceIds;
		}
		private int[] AdjacentEdgesToEdge(int edgeIndex)
		{
			int v1 = AdjacentVertex1ToEdge(edgeIndex);
			int v2 = adjacentVertex2ToEdge(edgeIndex);
			int[] vEdges1 = topoVertexEdges[v1];
			int[] vEdges2 = topoVertexEdges[v2];
			int[] nbEdges = new int[vEdges1.Length + vEdges2.Length - 2];
			int index = 0;
			for (int i = 0; i < vEdges1.Length; i++)
			{
				nbEdges[index] = vEdges1[i];
				index++;
			}
			for (int i = 0; i < vEdges2.Length; i++)
			{
				nbEdges[index] = vEdges2[i];
				index++;
			}

			return nbEdges;
		}

		private int AdjacentVertex1ToEdge(int edgeIndex)
		{
			return topoEdges[edgeIndex][VERTEX1];
		}
		private int adjacentVertex2ToEdge(int edgeIndex)
		{
			return topoEdges[edgeIndex][VERTEX2];
		}
		private int adjacentFace1ToEdge(int edgeIndex)
		{
			return topoEdges[edgeIndex][FACE1];
		}
		private int adjacentFace2ToEdge(int edgeIndex)
		{
			return topoEdges[edgeIndex][FACE2];
		}

		public ReadOnlyCollection<int[]> GetTopoVertexEdges()
		{
			return new ReadOnlyCollection<int[]>(topoVertexEdges);
		}

		public void FillUnityMesh(Mesh mesh){
			mesh.Clear();
        	mesh.vertices = this.VertexArray();
        	mesh.triangles = this.FlattenedTriangles();
			if (this.UVs!=null&&this.UVs.Count==this.VertexArray().Length){
 				mesh.uv = this.UVs.ToArray();
			}
        	mesh.SetColors(this.Colors);
        	mesh.RecalculateNormals();
		}
	}
}