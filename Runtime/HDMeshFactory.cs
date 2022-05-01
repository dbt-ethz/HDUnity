using UnityEngine;
using UnityEngine.UI;
namespace HD
{
	public class HDMeshFactory
	{

        public static void AddQuad(HDMesh mesh, float x1, float y1, float z1, float x2, float y2, float z2, float x3, float y3, float z3, float x4, float y4, float z4, Color color , bool flip = false)
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
		public static void AddQuadXY(HDMesh mesh, float x1, float y1,  float x2, float y2, float z,  Color color, bool flip)
		{

			AddQuad(mesh,x1,y1,z,x1,y2,z,x2,y2,z, x2, y1, z,color,flip);
		}
		public static void AddQuadXZ(HDMesh mesh, float x1, float z1, float x2, float z2, float y, Color color, bool flip)
		{
			AddQuad(mesh, x1, y, z1, x1, y, z2, x2, y, z2, x2, y, z1, color, flip);
		}
		public static void AddQuadYZ(HDMesh mesh, float y1, float z1, float y2, float z2, float x, Color color, bool flip)
		{
			AddQuad(mesh, x, y1, z1, x, y1, z2, x, y2, z2, x, y2, z1, color, flip);
		}

		public static void AddBox(HDMesh mesh,float x1,float y1,float z1,float x2,float y2,float z2,Color color)
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
			mesh.AddQuad(v[0], v[1], v[2], v[3]);
			mesh.AddQuad(v[7], v[6], v[5], v[4]);
			for (int i0 = 0; i0 < 4; i0++)
            {
				int i1 = (i0 + 1) % 4;
				int i2 = i1+4;
				int i3 = i0 + 4;
				mesh.AddQuad(v[i3], v[i2], v[i1], v[i0]);
			}
		}

		public static HDMesh createBox(float x1, float y1, float z1, float x2, float y2, float z2, Color color)
        {

			HDMesh mesh = new HDMesh();
			AddBox(mesh, x1, y1, z1, x2, y2, z2, color);
			return mesh;

		}


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
		public static void AddQuadY0(HDMesh mesh, int x, int y, int z, Color color,bool flip=false)
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