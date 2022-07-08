using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PG2.Mathematics;
using PG2.Rendering;
using PG2.Shading;

namespace PG2.Modeling
{
    public class Sphere : Model
    {
        #region Properties

        // TODO: Define object properties: Origin, Radius
        public Vector3 Origin;
        public double Radius;

        #endregion


        #region Init

        public Sphere()
        {
        }

        public Sphere(Shader shader, Vector3 origin, Double radius)
        {
            // TODO: Initialize class members Shader (inherited from base Model object), Origin, Radius
            Origin = origin;
            Radius = radius;
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
        public static void Collide(Ray ray, Sphere sphere)
        {
            // TODO: Compute ray-sphere intersection
            Vector3 V = sphere.Origin - ray.Origin;
            double t0 = ray.Direction.Normalized * V;
            double d = V * V - t0 * t0;
            double td = sphere.Radius * sphere.Radius - d;
            if (td >= 0 && t0 > 0)
            {
                double t = t0 - Math.Sqrt(td) > 0.0 + Eps ? t0 - Math.Sqrt(td) : t0 + Math.Sqrt(td);
                
                if (t > 0.0 + Eps && t < ray.HitParameter)
                {
                    ray.HitParameter = t;
                    ray.HitModel = sphere;
                    ray.HitNormal = ((ray.Origin + t * ray.Direction.Normalized) - sphere.Origin).Normalized;
                }
            }
        }

        #endregion
    }
}
