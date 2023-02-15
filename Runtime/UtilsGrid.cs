using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mola
{
    public class UtilsGrid : MonoBehaviour
    {
        public static MolaMesh VoxelMesh(MolaGrid<bool> grid, Color? c = null)
        {
            Color color = c ?? Color.white;
            MolaMesh molaMesh = new MolaMesh();

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
                                molaMesh.AddFace(new Vector3[4] { v1, v2, v3, v4 });
                                //MolaMeshFactory.AddQuadX1(molaMesh, x, y, z);
                            }

                            if (x == 0 || !grid[x - 1, y, z])
                            {
                                Vector3 v1 = new Vector3(x, y + 1, z);
                                Vector3 v2 = new Vector3(x, y, z);
                                Vector3 v3 = new Vector3(x, y, z + 1);
                                Vector3 v4 = new Vector3(x, y + 1, z + 1);
                                molaMesh.AddFace(new Vector3[4] { v1, v2, v3, v4 });
                                //MolaMeshFactory.AddQuadX0(myMesh, x, y, z);
                            }

                            if (y == grid.NY - 1 || !grid[x, y + 1, z])
                            {
                                Vector3 v1 = new Vector3(x + 1, y + 1, z);
                                Vector3 v2 = new Vector3(x, y + 1, z);
                                Vector3 v3 = new Vector3(x, y + 1, z + 1);
                                Vector3 v4 = new Vector3(x + 1, y + 1, z + 1);
                                molaMesh.AddFace(new Vector3[4] { v1, v2, v3, v4 });
                                //MolaMeshFactory.AddQuadY1(myMesh, x, y, z);
                            }

                            if (y == 0 || !grid[x, y - 1, z])
                            {
                                Vector3 v1 = new Vector3(x, y, z);
                                Vector3 v2 = new Vector3(x + 1, y, z);
                                Vector3 v3 = new Vector3(x + 1, y, z + 1);
                                Vector3 v4 = new Vector3(x, y, z + 1);
                                molaMesh.AddFace(new Vector3[4] { v1, v2, v3, v4 });
                            }

                            if(z == grid.NZ - 1 || !grid[x, y, z + 1])
                            {
                                Vector3 v1 = new Vector3(x, y, z + 1);
                                Vector3 v2 = new Vector3(x + 1, y, z + 1);
                                Vector3 v3 = new Vector3(x + 1, y + 1, z + 1);
                                Vector3 v4 = new Vector3(x, y + 1, z + 1);
                                molaMesh.AddFace(new Vector3[4] { v1, v2, v3, v4 });
                                //MolaMeshFactory.AddQuadZ1(myMesh, x, y, z);
                            }

                            if (z == 0 || !grid[x, y, z- 1])
                            {
                                Vector3 v1 = new Vector3(x, y + 1, z);
                                Vector3 v2 = new Vector3(x + 1, y + 1, z);
                                Vector3 v3 = new Vector3(x + 1, y, z);
                                Vector3 v4 = new Vector3(x, y, z);
                                molaMesh.AddFace(new Vector3[4] { v1, v2, v3, v4 });
                                //MolaMeshFactory.AddQuadZ0(myMesh, x, y, z);
                            }

                        }
                    }
                }
            }
            return molaMesh;
        }
    }
}