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
		public static HDMesh getQuadMeshUnwelded(Grid space, float iso)
		{
			HDMesh mesh = new HDMesh();
			int x0, y0, z0, x1, y1, z1;

			for (int x = 0; x < space.nX; x++)
			{
				for (int y = 0; y < space.nY; y++)
				{
					for (int z = 0; z < space.nZ; z++)
					{
						float v = space.GetValue(x, y, z);

						if (v <= iso)
						{
							//if (x<space.nX-1) {
							if (x == space.nX - 1 || space.GetValue(x + 1, y, z) > iso)
							{
								x1 = x + 1;
								y0 = y;
								y1 = y + 1;
								z0 = z;
								z1 = z + 1;
								int[] quadX1 = new int[4];
								quadX1[0] = mesh.AddVertex(x1, y0, z0);
								quadX1[1] = mesh.AddVertex(x1, y1, z0);
								quadX1[2] = mesh.AddVertex(x1, y1, z1);
								quadX1[3] = mesh.AddVertex(x1, y0, z1);
								mesh.AddFace(quadX1);
							}

							if (x == 0 || space.GetValue(x - 1, y, z) > iso)
							{
								x0 = x;
								y0 = y;
								y1 = y + 1;
								z0 = z;
								z1 = z + 1;
								int[] quadX0 = new int[4];
								quadX0[3] = mesh.AddVertex(x0, y0, z0);
								quadX0[2] = mesh.AddVertex(x0, y1, z0);
								quadX0[1] = mesh.AddVertex(x0, y1, z1);
								quadX0[0] = mesh.AddVertex(x0, y0, z1);
								mesh.AddFace(quadX0);

								//}
							}

							if (y == space.nY - 1 || space.GetValue(x, y + 1, z) > iso)
							{
								x0 = x;
								x1 = x + 1;
								y1 = y + 1;
								z0 = z;
								z1 = z + 1;
								int[] quadY1 = new int[4];
								quadY1[3] = mesh.AddVertex(x0, y1, z0);
								quadY1[2] = mesh.AddVertex(x1, y1, z0);
								quadY1[1] = mesh.AddVertex(x1, y1, z1);
								quadY1[0] = mesh.AddVertex(x0, y1, z1);
								mesh.AddFace(quadY1);

							}
							if (y == 0 || space.GetValue(x, y - 1, z) > iso)
							{
								x0 = x;
								x1 = x + 1;
								y0 = y;
								z0 = z;
								z1 = z + 1;
								int[] quadY0 = new int[4];
								quadY0[0] = mesh.AddVertex(x0, y0, z0);
								quadY0[1] = mesh.AddVertex(x1, y0, z0);
								quadY0[2] = mesh.AddVertex(x1, y0, z1);
								quadY0[3] = mesh.AddVertex(x0, y0, z1);
								mesh.AddFace(quadY0);
							}

							if (z == space.nZ - 1 || space.GetValue(x, y, z + 1) > iso)
							{
								x0 = x;
								x1 = x + 1;
								y0 = y;
								y1 = y + 1;
								z1 = z + 1;
								int[] quadZ1 = new int[4];
								quadZ1[0] = mesh.AddVertex(x0, y0, z1);
								quadZ1[1] = mesh.AddVertex(x1, y0, z1);
								quadZ1[2] = mesh.AddVertex(x1, y1, z1);
								quadZ1[3] = mesh.AddVertex(x0, y1, z1);
								mesh.AddFace(quadZ1);
							}

							if (z == 0 || space.GetValue(x, y, z - 1) > iso)
							{
								x0 = x;
								x1 = x + 1;
								y0 = y;
								y1 = y + 1;
								z0 = z;
								int[] quadZ0 = new int[4];
								quadZ0[3] = mesh.AddVertex(x0, y0, z0);
								quadZ0[2] = mesh.AddVertex(x1, y0, z0);
								quadZ0[1] = mesh.AddVertex(x1, y1, z0);
								quadZ0[0] = mesh.AddVertex(x0, y1, z0);
								mesh.AddFace(quadZ0);
							}
							//}
						}
					}
				}
			}
			return mesh;
		}


	}
}