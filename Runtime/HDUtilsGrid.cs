using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HD
{
    public class HDUtilsGrid : MonoBehaviour
    {
        public static HDMesh VoxelMesh(HDGrid<bool> grid, Color? c = null)
        {
            Color color = c ?? Color.white;
            HDMesh hdMesh = new HDMesh();

            for (int x = 0; x < grid.NX; x++)
            {
                for (int y = 0; y < grid.NY; y++)
                {
                    for (int z = 0; z < grid.NZ; z++)
                    {
                        if (grid[x, y, z])
                        {
                            if (x == grid.NX - 1 || !grid[x + 1, y, z])
                            {
                                Vector3 v1 = new Vector3(x + 1, y, z);
                                Vector3 v2 = new Vector3(x + 1, y + 1, z);
                                Vector3 v3 = new Vector3(x + 1, y + 1, z + 1);
                                Vector3 v4 = new Vector3(x + 1, y, z + 1);
                                hdMesh.AddFace(new Vector3[4] { v1, v2, v3, v4 });
                                //HDMeshFactory.AddQuadX1(hdMesh, x, y, z);
                            }

                            if (x == 0 || !grid[x - 1, y, z])
                            {
                                Vector3 v1 = new Vector3(x, y + 1, z);
                                Vector3 v2 = new Vector3(x, y, z);
                                Vector3 v3 = new Vector3(x, y, z + 1);
                                Vector3 v4 = new Vector3(x, y + 1, z + 1);
                                hdMesh.AddFace(new Vector3[4] { v1, v2, v3, v4 });
                                //HDMeshFactory.AddQuadX0(myMesh, x, y, z);
                            }

                            if (y == grid.NY - 1 || !grid[x, y + 1, z])
                            {
                                Vector3 v1 = new Vector3(x + 1, y + 1, z);
                                Vector3 v2 = new Vector3(x, y + 1, z);
                                Vector3 v3 = new Vector3(x, y + 1, z + 1);
                                Vector3 v4 = new Vector3(x + 1, y + 1, z + 1);
                                hdMesh.AddFace(new Vector3[4] { v1, v2, v3, v4 });
                                //HDMeshFactory.AddQuadY1(myMesh, x, y, z);
                            }

                            if (y == 0 || !grid[x, y - 1, z])
                            {
                                Vector3 v1 = new Vector3(x, y, z);
                                Vector3 v2 = new Vector3(x + 1, y, z);
                                Vector3 v3 = new Vector3(x + 1, y, z + 1);
                                Vector3 v4 = new Vector3(x, y, z + 1);
                                hdMesh.AddFace(new Vector3[4] { v1, v2, v3, v4 });
                            }

                            if(z == grid.NZ - 1 || !grid[x, y, z + 1])
                            {
                                Vector3 v1 = new Vector3(x, y, z + 1);
                                Vector3 v2 = new Vector3(x + 1, y, z + 1);
                                Vector3 v3 = new Vector3(x + 1, y + 1, z + 1);
                                Vector3 v4 = new Vector3(x, y + 1, z + 1);
                                hdMesh.AddFace(new Vector3[4] { v1, v2, v3, v4 });
                                //HDMeshFactory.AddQuadZ1(myMesh, x, y, z);
                            }

                            if (z == 0 || !grid[x, y, z- 1])
                            {
                                Vector3 v1 = new Vector3(x, y + 1, z);
                                Vector3 v2 = new Vector3(x + 1, y + 1, z);
                                Vector3 v3 = new Vector3(x + 1, y, z);
                                Vector3 v4 = new Vector3(x, y, z);
                                hdMesh.AddFace(new Vector3[4] { v1, v2, v3, v4 });
                                //HDMeshFactory.AddQuadZ0(myMesh, x, y, z);
                            }

                        }
                    }
                }
            }
            return hdMesh;
        }
    }
}