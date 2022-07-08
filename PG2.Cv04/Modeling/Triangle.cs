using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PG2.Mathematics;
using PG2.Rendering;
using PG2.Shading;

namespace PG2.Modeling
{
    public class Triangle : Model
    {
        #region Properties

        // TODO: Define object properties Vertex1, Vertex2, Vertex3
        public Vector3 Vertex1;
        public Vector3 Vertex2;
        public Vector3 Vertex3;

        #endregion


        #region Init

        public Triangle()
        {
        }

        public Triangle(Shader shader, Vector3 v1, Vector3 v2, Vector3 v3)
        {
            // TODO: Initialize class members Shader (inherited from base Model object), Vertex1, Vertex2, Vertex3
            Vertex1 = v1;
            Vertex2 = v2;
            Vertex3 = v3;
            Shader = shader;
        }

        #endregion


        #region Raytracing

        public override void Collide(Ray ray)
        {
            Collide(ray, this);
        }

        // Collide ray with object and return:
        //   intersection ray.HitParameter, surface normal at intersection point ray.HitNormal and intersected object ray.HitModel
        public static void Collide(Ray ray, Triangle triangle)
        {
            // Möller–Trumbore intersection algorithm (http://en.wikipedia.org/wiki/Möller–Trumbore_intersection_algorithm)
            // TODO: Compute ray-triangle intersection 
            Vector3 Edge1 = triangle.Vertex2 - triangle.Vertex1;
            Vector3 Edge2 = triangle.Vertex3 - triangle.Vertex1;
            Vector3 h, s, q;
            double a, f, u, v;

            h = ray.Direction.Normalized % Edge2;
            a = Edge1 * h;

            if (!(a > 0.0000001 && a < 0.00000001))
            {
                f = 1.0 / a;
                s = ray.Origin - triangle.Vertex1;
                u = f * s * h;
                if (!(u < 0.0 || u > 1.0))
                {
                    q = s % Edge1;
                    v = f * ray.Direction.Normalized * q;
                    if (!(v < 0.0 || u + v > 1.0))
                    {
                        double t = f * Edge2 * q;

                        if (t > 0.0 + Eps && t < ray.HitParameter)
                        {
                            ray.HitParameter = t;
                            ray.HitModel = triangle;
                            ray.HitNormal = (Edge1 % Edge2).Normalized;
                        }
                    }
                }
            }
        }

        #endregion
    }
}
