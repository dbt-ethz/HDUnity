using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Mola
{
	public class MeshFactory
	{
        public static void AddQuad(MolaMesh mesh, float x1, float y1, float z1, float x2, float y2, float z2, float x3, float y3, float z3, float x4, float y4, float z4, Color color , bool flip = false)
        {
			int[] quad = new int[4];
			if (!flip) {
				quad[0] = mesh.AddVertex(x1, y1, z1, color);
				quad[1] = mesh.AddVertex(x2, y2, z2, color);
				quad[2] = mesh.AddVertex(x3, y3, z3, color);
				quad[3] = mesh.AddVertex(x4, y4, z4, color);
			}
            else
            {
				quad[3] = mesh.AddVertex(x1, y1, z1, color);
				quad[2] = mesh.AddVertex(x2, y2, z2, color);
				quad[1] = mesh.AddVertex(x3, y3, z3, color);
				quad[0] = mesh.AddVertex(x4, y4, z4, color);
			}
			mesh.AddFace(quad);
		}
		public static void AddTriangle(MolaMesh mesh, float x1, float y1, float z1, float x2, float y2, float z2, float x3, float y3, float z3, Color color, bool flip = false)
        {
			int[] tri = new int[3];
            if (!flip)
            {
				tri[0] = mesh.AddVertex(x1, y1, z1, color);
				tri[1] = mesh.AddVertex(x2, y2, z2, color);
				tri[2] = mesh.AddVertex(x3, y3, z3, color);
			}
            else
            {
				tri[2] = mesh.AddVertex(x1, y1, z1, color);
				tri[1] = mesh.AddVertex(x2, y2, z2, color);
				tri[0] = mesh.AddVertex(x3, y3, z3, color);
			}
			mesh.AddFace(tri);
        }
		public static void AddQuadByLine(MolaMesh mesh, Vector2 a,Vector2 b, float z1,float z2 , Color color, bool flip)
		{
			AddQuad(mesh, a.x, z1, a.y, b.x, z1, b.y, b.x, z2, b.y, a.x, z2, a.y, color, flip);
		}
		public static void AddQuadXY(MolaMesh mesh, float x1, float y1,  float x2, float y2, float z,  Color color, bool flip)
		{

			AddQuad(mesh,x1,y1,z,x1,y2,z,x2,y2,z, x2, y1, z,color,flip);
		}
        public static void AddQuadXY(MolaMesh mesh, Vector2[] vs, float z, Color color, bool flip)
		{

			AddQuad(mesh, vs[0].x, z, vs[0].y, vs[1].x, z, vs[1].y, vs[2].x, z, vs[2].y, vs[3].x, z, vs[3].y, color, flip);
		}
		public static void AddQuadXZ(MolaMesh mesh, float x1, float z1, float x2, float z2, float y, Color color, bool flip)
		{
			AddQuad(mesh, x1, y, z1, x1, y, z2, x2, y, z2, x2, y, z1, color, flip);
		}
		public static void AddQuadYZ(MolaMesh mesh, float y1, float z1, float y2, float z2, float x, Color color, bool flip)
		{
			AddQuad(mesh, x, y1, z1, x, y1, z2, x, y2, z2, x, y2, z1, color, flip);
		}
		public static void AddBox(MolaMesh mesh,float x1,float y1,float z1,float x2,float y2,float z2,Color color)
        {
			int[] v = new int[8];
			v[0] = mesh.AddVertex(x1, y1, z1, color);
			v[1] = mesh.AddVertex(x1, y2, z1, color);
			v[2] = mesh.AddVertex(x2, y2, z1, color);
			v[3] = mesh.AddVertex(x2, y1, z1, color);
			v[4] = mesh.AddVertex(x1, y1, z2, color);
			v[5] = mesh.AddVertex(x1, y2, z2, color);
			v[6] = mesh.AddVertex(x2, y2, z2, color);
			v[7] = mesh.AddVertex(x2, y1, z2, color);
			AddBoxQuads(mesh, v);
		}
		private static void AddBoxQuads(MolaMesh mesh, int[] v)
        {
			mesh.AddQuad(v[0], v[1], v[2], v[3]);
			mesh.AddQuad(v[7], v[6], v[5], v[4]);
			for (int i0 = 0; i0 < 4; i0++)
			{
				int i1 = (i0 + 1) % 4;
				int i2 = i1 + 4;
				int i3 = i0 + 4;
				mesh.AddQuad(v[i3], v[i2], v[i1], v[i0]);
			}
		}
		public static void ExtrudeQuadYtoZ(MolaMesh mesh,IList<Vector2> bounds, float y1,float y2,Color color)
        {
			int[] v = new int[8];
			for (int i = 0; i < bounds.Count; i++)
            {
				Vector2 v2d = bounds[i];
				v[i]= mesh.AddVertex(v2d.x, y1, v2d.y, color);

			}
			for (int i = 0; i < bounds.Count; i++)
			{
				Vector2 v2d = bounds[i];
				v[i+4] = mesh.AddVertex(v2d.x, y2, v2d.y, color);

			}
			AddBoxQuads(mesh,v);
		}
		public static void AddQuadX0(MolaMesh mesh, int x, int y, int z, Color color)
		{
			int x0 = x;
			int y0 = y;
			int y1 = y + 1;
			int z0 = z;
			int z1 = z + 1;
			int[] quadX0 = new int[4];
			quadX0[3] = mesh.AddVertex(x0, y0, z0, color);
			quadX0[2] = mesh.AddVertex(x0, y1, z0, color);
			quadX0[1] = mesh.AddVertex(x0, y1, z1, color);
			quadX0[0] = mesh.AddVertex(x0, y0, z1, color);
			mesh.AddFace(quadX0);
		}
		public static void AddQuadX1(MolaMesh mesh, int x, int y, int z, Color color)
		{
			int x1 = x + 1;
			int y0 = y;
			int y1 = y + 1;
			int z0 = z;
			int z1 = z + 1;
			int[] quadX1 = new int[4];
			quadX1[0] = mesh.AddVertex(x1, y0, z0, color);
			quadX1[1] = mesh.AddVertex(x1, y1, z0, color);
			quadX1[2] = mesh.AddVertex(x1, y1, z1, color);
			quadX1[3] = mesh.AddVertex(x1, y0, z1, color);
			mesh.AddFace(quadX1);
		}
		public static void AddQuadY1(MolaMesh mesh, int x, int y, int z, Color color)
		{
			int x0 = x;
			int x1 = x + 1;
			int y1 = y + 1;
			int z0 = z;
			int z1 = z + 1;
			int[] quadY1 = new int[4];
			quadY1[3] = mesh.AddVertex(x0, y1, z0, color);
			quadY1[2] = mesh.AddVertex(x1, y1, z0, color);
			quadY1[1] = mesh.AddVertex(x1, y1, z1, color);
			quadY1[0] = mesh.AddVertex(x0, y1, z1, color);
			mesh.AddFace(quadY1);
		}
		public static void AddQuadY0(MolaMesh mesh, int x, int y, int z, Color color,bool flip=false)
		{
			int x0 = x;
			int x1 = x + 1;
			int y0 = y;
			int z0 = z;
			int z1 = z + 1;
			int[] quadY0 = new int[4];
			if (!flip) { 
				quadY0[0] = mesh.AddVertex(x0, y0, z0, color);
			quadY0[1] = mesh.AddVertex(x1, y0, z0, color);
			quadY0[2] = mesh.AddVertex(x1, y0, z1, color);
			quadY0[3] = mesh.AddVertex(x0, y0, z1, color);
			}
            else
            {
				quadY0[3] = mesh.AddVertex(x0, y0, z0, color);
				quadY0[2] = mesh.AddVertex(x1, y0, z0, color);
				quadY0[1] = mesh.AddVertex(x1, y0, z1, color);
				quadY0[0] = mesh.AddVertex(x0, y0, z1, color);
			}
				
            
			mesh.AddFace(quadY0);
		}
		public static void AddQuadZ1(MolaMesh mesh, int x, int y, int z, Color color)
		{
			int x0 = x;
			int x1 = x + 1;
			int y0 = y;
			int y1 = y + 1;
			int z1 = z + 1;
			int[] quadZ1 = new int[4];
			quadZ1[0] = mesh.AddVertex(x0, y0, z1, color);
			quadZ1[1] = mesh.AddVertex(x1, y0, z1, color);
			quadZ1[2] = mesh.AddVertex(x1, y1, z1, color);
			quadZ1[3] = mesh.AddVertex(x0, y1, z1, color);
			mesh.AddFace(quadZ1);
		}
		public static void AddQuadZ0(MolaMesh mesh, int x, int y, int z, Color color)
		{
			int x0 = x;
			int x1 = x + 1;
			int y0 = y;
			int y1 = y + 1;
			int z0 = z;
			int[] quadZ0 = new int[4];
			quadZ0[3] = mesh.AddVertex(x0, y0, z0, color);
			quadZ0[2] = mesh.AddVertex(x1, y0, z0, color);
			quadZ0[1] = mesh.AddVertex(x1, y1, z0, color);
			quadZ0[0] = mesh.AddVertex(x0, y1, z0, color);
			mesh.AddFace(quadZ0);
		}
		/// <summary>
		/// Creates and returns a single face mesh from the vertices.
		/// </summary>
		/// <param name="vertices"></param>
		/// <returns></returns>
		public static MolaMesh creatSingleFace(List<Vector3> vertices)
        {
			MolaMesh mesh = new MolaMesh();
			mesh.AddFace(vertices.ToArray());
			return mesh;
		}
		/// <summary>
		/// Creates and returns a conic cylinder.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="segments"></param>
		/// <param name="radius1"></param>
		/// <param name="radius2"></param>
		/// <returns></returns>
		public static MolaMesh creatCone(Vector3 a, Vector3 b, int segments, float radius1, float radius2)
		{
			List<Vector3> profile1 = UtilsVertex.getCircle(a.x, a.y, radius1, segments, a.z);
			List<Vector3> profile2 = UtilsVertex.getCircle(b.x, b.y, radius1, segments, b.z);
			MolaMesh mesh = new MolaMesh();
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
				mesh.AddTriangle(i, i2, iCenter);
			}
			//top
			iCenter = segments * 2 + 1;
			for (int i = 0; i < segments; i++)
			{
				int i2 = (i + 1) % segments + segments;
				mesh.AddTriangle(i + segments, iCenter, i2);
			}
			return mesh;
		}
		public static MolaMesh creatTube(Vector3 a, Vector3 b, int segments, float radius)
		{
			List<Vector3> profile = UtilsVertex.getCircle(0, 0, radius, segments);

			return MeshPiping.PipeLineWithConvexProfile(a, b, profile, new Vector3(0, 1, 0), false, false);
		}
		/// <summary>
		/// Creates and returns a mesh box with six quad faces.
		/// </summary>
		/// <param name="x1"></param>
		/// <param name="y1"></param>
		/// <param name="z1"></param>
		/// <param name="x2"></param>
		/// <param name="y2"></param>
		/// <param name="z2"></param>
		/// <param name="color"></param>
		/// <returns></returns>
		public static MolaMesh createBox(float x1, float y1, float z1, float x2, float y2, float z2, Color color)
		{

			MolaMesh mesh = new MolaMesh();
			AddBox(mesh, x1, y1, z1, x2, y2, z2, color);
			return mesh;

		}
		public static MolaMesh createGridMesh(IList<Vector3> vertices, int nU, int nV, bool uClosed)
		{

			MolaMesh mesh = new MolaMesh();
			mesh.Vertices = new List<Vector3>(vertices);
			if (!uClosed)
			{
				for (int u = 0; u < nU - 1; u++)
				{
					int u2 = u + 1;
					for (int v = 0; v < nV - 1; v++)
					{
						int v2 = v + 1;
						int i1 = u * nV + v;
						int i2 = u * nV + v2;
						int i3 = u2 * nV + v;
						int i4 = u2 * nV + v2;
						mesh.AddQuad(i1, i2, i3, i4);
					}
				}
			}
			else
			{
				for (int u = 0; u < nU; u++)
				{
					int u2 = (u + 1) % nU;
					for (int v = 0; v < nV - 1; v++)
					{
						int v2 = v + 1;
						int i1 = u * nV + v;
						int i2 = u * nV + v2;
						int i3 = u2 * nV + v;
						int i4 = u2 * nV + v2;
						mesh.AddQuad(i1, i2, i3, i4);
					}
				}
			}
			return mesh;

		}
		/// <summary>
		/// Creates and returns a mesh in the form of an icosahedron.
		/// </summary>
		/// <param name="radius"></param>
		/// <param name="cx"></param>
		/// <param name="cy"></param>
		/// <param name="cz"></param>
		/// <returns></returns>
		public static MolaMesh createIcosahedron(float radius=1, float cx=0, float cy=0, float cz=0)
        {
			MolaMesh mesh = new MolaMesh();
			float phi = (float)(1 + Math.Pow(5, 0.5)) / 2;
			float coordA = (float)(1 / (2 * Math.Sin(2 * Math.PI / 5)));
			float coordB = (float)(phi / (2 * Math.Sin(2 * Math.PI / 5)));

			mesh.Vertices = new List<Vector3>() { 
				new Vector3(0, -coordA, coordB),
				new Vector3(coordB, 0, coordA),
				new Vector3(coordB, 0, -coordA),
				new Vector3(-coordB, 0, -coordA),
				new Vector3(-coordB, 0, coordA),
				new Vector3(-coordA, coordB, 0),
				new Vector3(coordA, coordB, 0),
				new Vector3(coordA, -coordB, 0),
				new Vector3(-coordA, -coordB, 0),
				new Vector3(0, -coordA, -coordB),
				new Vector3(0, coordA, -coordB),
				new Vector3(0, coordA, coordB)
			};

			for(int i = 0; i < mesh.VertexCount(); i++)
            {
				mesh.Vertices[i] *= radius;
				mesh.Vertices[i] += new Vector3(cx, cy, cz);
            }

			List<int> indices = new List<int>() { 1, 2, 6, 1, 7, 2, 3, 4, 5, 4, 3, 8, 6, 5, 11, 5, 6, 10, 9, 10, 2, 10, 9, 3, 7, 8, 9, 8, 7, 0, 11, 0, 1, 0, 11, 4, 6, 2, 10, 1, 6, 11, 3, 5, 10, 5, 4, 11, 2, 7, 9, 7, 1, 0, 3, 9, 8, 4, 8, 0 };
			List<int[]> faces = new List<int[]>();

			for (int i = 0; i < indices.Count; i += 3)
            {
				faces.Add(new int[] {indices[i], indices[i + 1], indices[i + 2]});
			}
			mesh.Faces = faces;

			return mesh;
		}
	}
}