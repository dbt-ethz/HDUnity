using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HD
{
    public class HDGridToMesh : MonoBehaviour
    {
        public static HDMesh get(HDGrid<bool> grid)
        {
            HDMesh myMesh = new HDMesh();
            int nX = grid.NX;
            int nY = grid.NY;
            int nZ = grid.NZ;

            for (int x = 0; x < grid.NX; x++)
            {
                for (int y = 0; y < grid.NY; y++)
                {
                    for (int z = 0; z < grid.NZ; z++)
                    {
                        if (grid[x, y, z])
                        {

                            int index = grid.GetIndex(x, y, z);

                            if (x == 0 || !grid[x - 1, y, z])
                            {
                                HDMeshFactory.AddQuadX(myMesh, x, y, z);
                            }

                            if (x == nX - 1 || !grid[x + 1, y, z])
                            {
                                HDMeshFactory.AddQuadX(myMesh, x+1, y, z);

                            }
                            if (z == 0 || !grid[x, y, z - 1])
                            {
                                HDMeshFactory.AddQuadZ(myMesh, x, y, z);

                            }
                            if (z == nZ - 1 || !grid[x, y, z + 1])
                            {
                                HDMeshFactory.AddQuadZ(myMesh, x, y, z+1);

                            }

                            if (y == nY - 1 || !grid[x, y + 1, z])
                            {
                                HDMeshFactory.AddQuadY(myMesh, x, y+1, z);
                            }


                        }
                    }
                }
            }
            myMesh.TriangulateQuads();
            return myMesh;
        }

    }

    }

