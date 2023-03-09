using System.Collections.Generic;
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

        public static void AddQuad(HDMesh mesh, float x1, float y1, float z1, float x2, float y2, float z2, float x3, float y3, float z3, float x4, float y4, float z4,  bool flip = false)
        {
            int[] quad = new int[4];
            if (!flip)
            {
                quad[0] = mesh.AddVertex(x1, y1, z1);
                quad[1] = mesh.AddVertex(x2, y2, z2);
                quad[2] = mesh.AddVertex(x3, y3, z3);
                quad[3] = mesh.AddVertex(x4, y4, z4);
            }
            else
            {
                quad[3] = mesh.AddVertex(x1, y1, z1);
                quad[2] = mesh.AddVertex(x2, y2, z2);
                quad[1] = mesh.AddVertex(x3, y3, z3);
                quad[0] = mesh.AddVertex(x4, y4, z4);
            }
            mesh.AddFace(quad);
        }
        public static void AddQuadByLine(HDMesh mesh, Vector2 a,Vector2 b, float z1,float z2 , Color color, bool flip)
		{
			AddQuad(mesh, a.x, z1, a.y, b.x, z1, b.y, b.x, z2, b.y, a.x, z2, a.y, color, flip);
		}
		public static void AddQuadXY(HDMesh mesh, float x1, float y1,  float x2, float y2, float z,  Color color, bool flip)
		{

			AddQuad(mesh,x1,y1,z,x1,y2,z,x2,y2,z, x2, y1, z,color,flip);
		}

        public static void AddQuadXY(HDMesh mesh, Vector2[] vs, float z, Color color, bool flip)
		{

			AddQuad(mesh, vs[0].x, z, vs[0].y, vs[1].x, z, vs[1].y, vs[2].x, z, vs[2].y, vs[3].x, z, vs[3].y, color, flip);
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
			AddBoxQuads(mesh, v);
		}

		public static HDMesh createBox(float x1, float y1, float z1, float x2, float y2, float z2, Color color)
        {

			HDMesh mesh = new HDMesh();
			AddBox(mesh, x1, y1, z1, x2, y2, z2, color);
			return mesh;

		}

		public static HDMesh createGridMesh(IList<Vector3>vertices, int nU,int nV,bool uClosed)
        {

			HDMesh mesh = new HDMesh();
			mesh.Vertices=new List<Vector3>(vertices);
			if (!uClosed){
			for (int u=0;u<nU-1;u++){
				int u2=u+1;
				for (int v=0;v<nV-1;v++){
					int v2=v+1;
					int i1=u*nV+v;
					int i2=u*nV+v2;
					int i3=u2*nV+v;
					int i4=u2*nV+v2;
					mesh.AddQuad(i1,i2,i3,i4);
				}
			}
			}
			else{
				for (int u=0;u<nU;u++){
				int u2=(u+1)%nU;
				for (int v=0;v<nV-1;v++){
					int v2=v+1;
					int i1=u*nV+v;
					int i2=u*nV+v2;
					int i3=u2*nV+v;
					int i4=u2*nV+v2;
					mesh.AddQuad(i1,i2,i3,i4);
				}
			}
			}
			return mesh;

		}
		private static void AddBoxQuads(HDMesh mesh, int[] v)
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
		public static void ExtrudeQuadYtoZ(HDMesh mesh,IList<Vector2> bounds, float y1,float y2,Color color)
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

		//public const static Color StandardColor=Color.white;

		
		public static void AddQuadX(HDMesh mesh, int x, int y, int z, Color color, bool flip = false)
		{
			int x0 = x;
			int x1 = x;
			int y0 = y;
			int y1 = y + 1;
			int z0 = z;
			int z1 = z + 1;
			AddQuad(mesh, x0, y0, z0, x1, y1, z0, x1, y1, z1, x1, y0, z1, color,flip);
			
		}
        public static void AddQuadX(HDMesh mesh, int x, int y, int z, bool flip = false)
        {
            int x0 = x;
            int x1 = x;
            int y0 = y;
            int y1 = y + 1;
            int z0 = z;
            int z1 = z + 1;
            AddQuad(mesh, x0, y0, z0, x1, y1, z0, x1, y1, z1, x1, y0, z1, flip);

        }

        public static void AddQuadY(HDMesh mesh, int x, int y, int z, Color color,bool flip=false)
		{
			int x0 = x;
			int x1 = x + 1;
			int y0 = y;
            int y1 = y;
            int z0 = z;
			int z1 = z + 1;

            AddQuad(mesh, x0, y0, z0, x1, y0, z0, x1, y0, z1, x0, y0, z1, color, flip);

		}
        public static void AddQuadY(HDMesh mesh, int x, int y, int z, bool flip = false)
        {
            int x0 = x;
            int x1 = x + 1;
            int y0 = y;
            int y1 = y;
            int z0 = z;
            int z1 = z + 1;

            AddQuad(mesh, x0, y0, z0, x1, y0, z0, x1, y0, z1, x0, y0, z1, flip);

        }


        public static void AddQuadZ(HDMesh mesh, int x, int y, int z, Color color, bool flip = false)
		{
			int x0 = x;
			int x1 = x + 1;
			int y0 = y;
			int y1 = y + 1;
			int z0 = z;
            int z1 = z;

            AddQuad(mesh, x0, y0, z0, x1, y0, z0, x1, y1, z0, x0, y1, z0, color, flip);

		}
        public static void AddQuadZ(HDMesh mesh, int x, int y, int z,  bool flip = false)
        {
            int x0 = x;
            int x1 = x + 1;
            int y0 = y;
            int y1 = y + 1;
            int z0 = z;
            int z1 = z;

            AddQuad(mesh, x0, y0, z0, x1, y0, z0, x1, y1, z0, x0, y1, z0, flip);

        }




    }
}