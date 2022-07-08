using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PG2.Mathematics;
using PG2.Rendering;
using PG2.Shading;

namespace PG2.Modeling
{
    public class Block : Model
    {
        #region Properties

        // TODO: Define AABB block object properties Min, Max (top-left and bottom-right)
        public Vector3 Max;
        public Vector3 Min;

        #endregion


        #region Init

        public Block()
        {
        }

        public Block(Shader shader, Vector3 min, Vector3 max)
        {
            // TODO: Initialize class members Shader (inherited from base Model object), Min, Max
            Shader = shader;
            Min = min;
            Max = max;
        }

        #endregion


        #region Raytracing

        public override void Collide(Ray ray)
        {
            Collide(ray, this);
        }

        // Collide ray with object and return:
        //   intersection ray.HitParameter, surface normal at intersection point ray.HitNormal and intersected object ray.HitModel
        public static void Collide(Ray ray, Block box)
        {
            // TODO: Compute ray-block intersection
            Vector3 A = box.Min;
            Vector3 B = box.Max;

            List<Vector3> Points = new List<Vector3>();
            List<Vector3> Normals = new List<Vector3>();

            Points.Add(A);
            Points.Add(new Vector3(B.X, A.Y, A.Z));
            Points.Add(new Vector3(B.X, B.Y, A.Z));
            Points.Add(new Vector3(A.X, B.Y, A.Z));
            Points.Add(new Vector3(A.X, A.Y, B.Z));
            Points.Add(new Vector3(B.X, A.Y, B.Z));
            Points.Add(B);
            Points.Add(new Vector3(A.X, B.Y, B.Z));

            List<Vector3> StartVert = new List<Vector3>();
            List<Vector3[]> Edges = new List<Vector3[]>();
            Vector3[] E = new Vector3[2];


            //spodna stena
            E = new Vector3[]
            {
                Points[1] - Points[0],
                Points[3] - Points[0]
            };
            Normals.Add((E[1] % E[0]).Normalized);
            Edges.Add(E);
            StartVert.Add(Points[0]);

            //bocne steny
            for (int i = 0; i < 4; i++)
            {
                E = new Vector3[]
            {
                Points[(i + 1) % 4] - Points[i],
                Points[i + 4] - Points[i]
            };
                Normals.Add((E[0] % E[1]).Normalized);
                Edges.Add(E);
                StartVert.Add(Points[i]);
            }

            //vrchna stena
            E = new Vector3[]
            {
                Points[5] - Points[4],
                Points[7] - Points[4]
            };
            Normals.Add((E[0] % E[1]).Normalized);
            Edges.Add(E);
            StartVert.Add(Points[4]);



            for (int i = 0; i < Normals.Count(); i++)
            {


                double t = ((Points[i] - ray.Origin) * Normals[i]) / (ray.Direction.Normalized * Normals[i]);
                Vector3 hitpoint = ray.Origin + t * ray.Direction.Normalized;

                double u = (hitpoint - StartVert[i]) * (Edges[i][0].Normalized);
                double v = (hitpoint - StartVert[i]) * (Edges[i][1].Normalized);

                if (u >= 0 && u <= Edges[i][0].Length && v >= 0 && v <= Edges[i][1].Length)
                {
                    if (t > 0.0 + Eps && t < ray.HitParameter)
                    {
                        ray.HitParameter = t;
                        ray.HitModel = box;
                        ray.HitNormal = Normals[i];
                    }
                }

            }
        }

        #endregion
    }
}
