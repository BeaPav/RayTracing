using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PG2.Mathematics;
using PG2.Lighting;

namespace PG2.Shading
{
    
    public class Shader
    {
        #region Properties

        public Double ReflectionFactor = 0.0;
        public Double RefractionFactor = 0.0;
        public Double RefractionIndex = 1.0;

        #endregion


        #region Shading

        // Each shader should return the color of the hitpoint with the hitpoint normal, in current lighting conditions
        public virtual Vector3 GetColor(Vector3 point, Vector3 normal, Vector3 viewDir, Vector3 lightDir, Double attenuation, Light light)
        {
            return Vector3.Zero;
        }

        public virtual Vector3 GetAmbientColor(Vector3 point)
        {
            return Vector3.Zero;
        }

        #endregion
    }
}
