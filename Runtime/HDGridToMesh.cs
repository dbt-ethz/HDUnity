using UnityEngine;
using UnityEngine.UI;
namespace HD
{
	public class HDGridToMesh
	{
		public static void AddQuadX0(HDMesh mesh, int x, int y, int z, Color color)
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
		public static void AddQuadX1(HDMesh mesh, int x, int y, int z, Color color)
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
		public static void AddQuadY1(HDMesh mesh, int x, int y, int z, Color color)
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
		public static void AddQuadY0(HDMesh mesh, int x, int y, int z, Color color)
		{
			int x0 = x;
			int x1 = x + 1;
			int y0 = y;
			int z0 = z;
			int z1 = z + 1;
			int[] quadY0 = new int[4];
			quadY0[0] = mesh.AddVertex(x0, y0, z0, color);
			quadY0[1] = mesh.AddVertex(x1, y0, z0, color);
			quadY0[2] = mesh.AddVertex(x1, y0, z1, color);
			quadY0[3] = mesh.AddVertex(x0, y0, z1, color);
			mesh.AddFace(quadY0);
		}

		public static void AddQuadZ1(HDMesh mesh, int x, int y, int z, Color color)
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
		public static void AddQuadZ0(HDMesh mesh, int x, int y, int z, Color color)
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
		


	}
}