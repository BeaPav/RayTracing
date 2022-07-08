using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PG2.Mathematics;
using PG2.Modeling;

namespace PG2.Rendering
{
    public class Ray
    {
        #region Properties

        // TODO: Define ray properties Origin, Direction, HitParameter, HitNormal
        // TODO: Declare HitModel to null

        public Vector3 Origin;
        public Vector3 Direction;
        public Double HitParameter;
        public Vector3 HitNormal;
        public Model HitModel = null;

        // TODO: Declare ReflectionLevel and RefractionLevel being the interaction level of reflection and refraction to zero

        public int ReflectionLevel = 0;
        public int RefractionLevel = 0;

        #endregion


        #region Init

        public Ray()
        {           
        }

        // Init ray
        public Ray(Vector3 origin, Vector3 direction)
        {
            Set(origin, direction);
        }

        public Ray(Vector3 origin, Vector3 direction, int reflectionLevel, int refractionLevel)
        {
            Set(origin, direction, reflectionLevel, refractionLevel);
        }

        public Ray(Vector3 origin, Vector3 direction, int reflectionLevel, int refractionLevel, Double zFar)
        {
            Set(origin, direction, reflectionLevel, refractionLevel, zFar);
        }

        public void Set(Vector3 origin, Vector3 direction, int reflectionLevel = 0, int refractionLevel = 0, Double zFar = Double.MaxValue)
        {
            // TODO: Init ray class members Origin, Direction, ReflectionLevel, RefractioLevel. Set HitParameter to the zFar

            Origin = origin;
            Direction = direction;
            HitParameter = zFar;
            ReflectionLevel = reflectionLevel;
            RefractionLevel = refractionLevel;
        }

        // Return hit point of current ray
        public Vector3 GetHitPoint()
        {
            // TODO: calculate hit point position on the ray, use HitParameter value;
            return Origin + HitParameter * Direction;
        }

        #endregion
    }
}
