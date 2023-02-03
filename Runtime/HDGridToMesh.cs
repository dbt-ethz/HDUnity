using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HD
{
    public class HDGridToMesh : MonoBehaviour
    {
        public static HDMesh get(HDGrid grid)
        {
            HDMesh myMesh = new HDMesh();

            for (int x = 0; x < nX; x++)
            {
                for (int y = 0; y < nY; y++)
                {
                    for (int z = 0; z < nZ; z++)
                    {
                        if (grid[x, y, z])
                        {

                            int index = grid.GetIndex(x, y, z);



                            if (x == 0 || !grid[x - 1, y, z])
                            {
                                HDMeshFactory.AddQuadX0(myMesh, x, y, z);
                            }

                            if (x == nX - 1 || !grid[x + 1, y, z])
                            {
                                HDMeshFactory.AddQuadX1(myMesh, x, y, z);

                            }
                            if (z == 0 || !grid[x, y, z - 1])
                            {
                                HDMeshFactory.AddQuadZ0(myMesh, x, y, z);

                            }
                            if (z == nZ - 1 || !grid[x, y, z + 1])
                            {
                                HDMeshFactory.AddQuadZ1(myMesh, x, y, z);

                            }

                            if (y == nY - 1 || !grid[x, y + 1, z])
                            {
                                HDMeshFactory.AddQuadY1(myMesh, x, y, z);
                            }


                        }
                    }
                }
            }
            myMesh.TriangulateQuads();
            return myMesh;
        }

       

    }

