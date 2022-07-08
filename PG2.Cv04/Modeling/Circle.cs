using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PG2.Mathematics;
using PG2.Rendering;
using PG2.Shading;

namespace PG2.Modeling
{
    public class Circle : Model
    {
        #region Properties

        // TODO: Define object properties: Origin, Normal, Radius
        public Vector3 Origin;
        public Vector3 Normal;
        public Double Radius;

        #endregion


        #region Init

        public Circle()
        {
        }

        public Circle(Shader shader, Vector3 origin, Vector3 normal, Double radius)
        {
            // TODO: Initialize class members Shader (inherited from base Model object), Origin, Normal, Radius;
            Shader = shader;
            Origin = origin;
            Normal = normal;
            Radius = radius;
        }

        #endregion


        #region Raytracing

        public override void Collide(Ray ray)
        {
            Collide(ray, this);
        }

        // Collide ray with object and return:
        //   intersection ray.HitParameter, surface normal at intersection point ray.HitNormal and intersected object ray.HitModel
        public static void Collide(Ray ray, Circle circle)
        {
            // TODO: Compute ray-circle intersection   
            double t;
            t = ((circle.Origin - ray.Origin) * circle.Normal) / (ray.Direction.Normalized * circle.Normal);

            // prienik s kruhom
            Vector3 hitpoint = ray.Origin + t * ray.Direction;
            if (t > 0.0 + Eps && (hitpoint - circle.Origin).Length <= circle.Radius && t < ray.HitParameter)
            {
                ray.HitParameter = t;
                ray.HitModel = circle;
                ray.HitNormal = circle.Normal;
            }
        }

        #endregion
    }
}
