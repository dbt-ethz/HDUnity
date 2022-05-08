using System.Collections.Generic;
using System;
using UnityEngine;

namespace HD
{
    public class MarchingSquare
    {
        public static HDMesh MarchingSquares(float[][] field, float iso,float z=0)
        {
            HDMesh mesh = new HDMesh();
            //Marching Squares isolines
            int cols = field.Length;
            int rows = field[0].Length;
            for (int i = 0; i < cols - 1; i++)
            {
                for (int j = 0; j < rows - 1; j++)
                {
                    float x = i;
                    float y = j;

                    int c1 = field[i][j] < iso ? 0 : 1;
                    int c2 = field[i + 1][j] < iso ? 0 : 1;
                    int c3 = field[i + 1][j + 1] < iso ? 0 : 1;
                    int c4 = field[i][j + 1] < iso ? 0 : 1;
                    int state = GetState(c1, c2, c3, c4);

                    //interpolation
                    /*The dot . is the field values
                     .___a___.
                     |       |
                     d       b
                     |       |
                     .___c___.
                     */
                    float a_val = field[i][j];
                    float b_val = field[i + 1][j];
                    float c_val = field[i + 1][j + 1];
                    float d_val = field[i][j + 1];

                    Vector2 a = new Vector2();
                    float amt = (iso - a_val) / (b_val - a_val);
                    a.x = Mathf.Lerp(x, x + 1, amt);
                    a.y = y;

                    Vector2 b = new Vector2();
                    amt = (iso - b_val) / (c_val - b_val);
                    b.x = x + 1;
                    b.y = Mathf.Lerp(y, y + 1, amt);

                    Vector2 c = new Vector2();
                    amt = (iso - d_val) / (c_val - d_val);
                    c.x = Mathf.Lerp(x, x + 1, amt);
                    c.y = y + 1;

                    Vector2 d = new Vector2();
                    amt = (iso - a_val) / (d_val - a_val);
                    d.x = x;
                    d.y = Mathf.Lerp(y, y + 1, amt);

                    //Marching squares rule table https://en.wikipedia.org/wiki/File:Marching_squares_algorithm.svg
                    if (state > 15){
                        state = -1;
                    }
                    switch (state)
                    {
                        case 1:
                            //bottom left triangle
                            mesh.AddTri2D(d.x, d.y, c.x, c.y, i, j + 1, z);
                            break;
                        case 2:
                            //bottom right triangle
                            mesh.AddTri2D(b.x, b.y, i + 1, j + 1, c.x, c.y, z);
                            break;
                        case 3:
                            //retangular bottom half
                            mesh.AddTri2D(d.x, d.y, b.x, b.y, i + 1, j + 1, z);
                            mesh.AddTri2D(d.x, d.y, i + 1, j + 1, i, j + 1, z);
                            break;
                        case 4:
                            //top right triangle
                            mesh.AddTri2D(a.x, a.y, i + 1, j, b.x, b.y, z);
                            break;
                        case 5:
                            //bottom-left to top-right middle hexagon slice
                            mesh.AddTri2D(a.x, a.y, i + 1, j, b.x, b.y, z);
                            mesh.AddTri2D(a.x, a.y, b.x, b.y, c.x, c.y, z);
                            mesh.AddTri2D(a.x, a.y, c.x, c.y, i, j + 1, z);
                            mesh.AddTri2D(a.x, a.y, i, j + 1, d.x, d.y, z);
                            break;
                        case 6:
                            //retangular left side half
                            mesh.AddTri2D(a.x, a.y, i + 1, j, i + 1, j + 1, z);
                            mesh.AddTri2D(a.x, a.y, i + 1, j + 1, c.x, c.y, z);
                            break;
                        case 7:
                            //Pentagonal shape from subtracted top-left triangle
                            mesh.AddTri2D(a.x, a.y, i + 1, j, i + 1, j + 1, z);
                            mesh.AddTri2D(a.x, a.y, i + 1, j + 1, i, j + 1, z);
                            mesh.AddTri2D(a.x, a.y, i, j + 1, d.x, d.y, z);
                            break;
                        case 8:
                            //top-left triangle
                            mesh.AddTri2D(i, j, a.x, a.y, d.x, d.y, z);
                            break;
                        case 9:
                            //left side rectangle
                            mesh.AddTri2D(i, j, a.x, a.y, c.x, c.y, z);
                            mesh.AddTri2D(i, j, c.x, c.y, i,j + 1, z);
                            break;
                        case 10:
                            //top-left to bottom-right middle hexagon slice
                            mesh.AddTri2D(i, j, a.x, a.y, b.x, b.y, z);
                            mesh.AddTri2D(i, j, b.x, b.y, i + 1, j + 1, z);
                            mesh.AddTri2D(i, j, i + 1, j + 1, c.x, c.y, z);
                            mesh.AddTri2D(i, j, c.x, c.y, d.x, d.y, z);
                            break;
                        case 11:
                            //Pentagonal shape from subtracted top-right triangle
                            mesh.AddTri2D(i, j, a.x, a.y, b.x, b.y, z);
                            mesh.AddTri2D(i, j, b.x, b.y, i + 1, j + 1, z);
                            mesh.AddTri2D(i, j, i + 1, j + 1, i, j + 1, z);
                            break;
                        case 12:
                            //Top half rectangle
                            mesh.AddTri2D(i, j, i + 1, j, b.x, b.y, z);
                            mesh.AddTri2D(i, j, b.x, b.y, d.x, d.y, z);
                            break;
                        case 13:
                            //Pentagonal shape from subtracted bottom-right triangle
                            mesh.AddTri2D(i, j, i + 1, j, b.x, b.y, z);
                            mesh.AddTri2D(i, j, b.x, b.y, c.x, c.y, z);
                            mesh.AddTri2D(i, j, c.x, c.y, i, j + 1, z);
                            break;
                        case 14:
                            //Pentagonal shape from subtracted bottom-left triangle
                            mesh.AddTri2D(i, j, i + 1, j, i + 1, j + 1, z);
                            mesh.AddTri2D(i, j, i + 1, j + 1, c.x, c.y, z);
                            mesh.AddTri2D(i, j, c.x, c.y, d.x, d.y, z);
                            break;
                        case 15:
                            //Pentagonal shape from subtracted bottom-left triangle
                            mesh.AddTri2D(i, j, i + 1, j, i + 1, j + 1, z);
                            mesh.AddTri2D(i, j, i + 1, j + 1, i, j + 1, z);
                            break;
                    }

                }
            }
            return mesh;
        }


        public static List<Vector2> MarchingContours(float[][] field, float iso)
        {
            HDMesh mesh = new HDMesh();
            //Marching Squares isolines
            int cols = field.Length;
            int rows = field[0].Length;
            List<Vector2> contours = new List<Vector2>();
            for (int i = 0; i < cols - 1; i++)
            {
                for (int j = 0; j < rows - 1; j++)
                {
                    float x = i;
                    float y = j;

                    int c1 = field[i][j] < iso ? 0 : 1;
                    int c2 = field[i + 1][j] < iso ? 0 : 1;
                    int c3 = field[i + 1][j + 1] < iso ? 0 : 1;
                    int c4 = field[i][j + 1] < iso ? 0 : 1;
                    int state = GetState(c1, c2, c3, c4);

                    //interpolation
                    /*The dot . is the field values
                     .___a___.
                     |       |
                     d       b
                     |       |
                     .___c___.
                     */
                    float a_val = field[i][j];
                    float b_val = field[i + 1][j];
                    float c_val = field[i + 1][j + 1];
                    float d_val = field[i][j + 1];

                    Vector2 a = new Vector2();
                    float amt = (iso - a_val) / (b_val - a_val);
                    a.x = Mathf.Lerp(x, x + 1, amt);
                    a.y = y;

                    Vector2 b = new Vector2();
                    amt = (iso - b_val) / (c_val - b_val);
                    b.x = x + 1;
                    b.y = Mathf.Lerp(y, y + 1, amt);

                    Vector2 c = new Vector2();
                    amt = (iso - d_val) / (c_val - d_val);
                    c.x = Mathf.Lerp(x, x + 1, amt);
                    c.y = y + 1;

                    Vector2 d = new Vector2();
                    amt = (iso - a_val) / (d_val - a_val);
                    d.x = x;
                    d.y = Mathf.Lerp(y, y + 1, amt);

                    //Marching squares rule table https://en.wikipedia.org/wiki/File:Marching_squares_algorithm.svg
                    if (state > 15)
                    {
                        state = -1;
                    }
                    switch (state)
                    {
                        case 1:
                            //bottom left triangle
                            contours.Add(c);
                            contours.Add(d);
                            break;
                        case 2:
                            //bottom right triangle
                            contours.Add(c);
                            contours.Add(b);
                            break;
                        case 3:
                            //retangular bottom half
                            contours.Add(d);
                            contours.Add(b);
                            break;
                        case 4:
                            //top right triangle
                            contours.Add(b);
                            contours.Add(a);
                            break;
                        case 5:
                            //bottom-left to top-right middle hexagon slice
                            contours.Add(d);
                            contours.Add(a);
                            contours.Add(b);
                            contours.Add(c);
                            break;
                        case 6:
                            //retangular left side half
                            contours.Add(c);
                            contours.Add(a);
                            break;
                        case 7:
                            //Pentagonal shape from subtracted top-left triangle
                            contours.Add(d);
                            contours.Add(a);
                            break;
                        case 8:
                            //top-left triangle
                            contours.Add(a);
                            contours.Add(d);
                            break;
                        case 9:
                            //left side rectangle
                            contours.Add(a);
                            contours.Add(c);
                            break;
                        case 10:
                            //top-left to bottom-right middle hexagon slice
                            contours.Add(a);
                            contours.Add(b);
                            contours.Add(c);
                            contours.Add(d);
                            break;
                        case 11:
                            //Pentagonal shape from subtracted top-right triangle
                            contours.Add(a);
                            contours.Add(b);
                            break;
                        case 12:
                            //Top half rectangle
                            contours.Add(b);
                            contours.Add(d);
                            break;
                        case 13:
                            //Pentagonal shape from subtracted bottom-right triangle
                            contours.Add(b);
                            contours.Add(c);
                            break;
                        case 14:
                            //Pentagonal shape from subtracted bottom-left triangle
                            contours.Add(c);
                            contours.Add(d);
                            break;
                        case 15:
                            break;
                    }

                }
            }
            return contours;
        }

        
        //Get the decimal value for the 4-bit binary number abcd
        public static int GetState(int a, int b, int c, int d)
        {
            return a * 8 + b * 4 + c * 2 + d * 1;
        }
    }

}