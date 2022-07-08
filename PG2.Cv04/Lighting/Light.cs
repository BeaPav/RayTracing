using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PG2.Mathematics;
using PG2.Rendering;

namespace PG2.Lighting
{
    public class Light
    {
        #region Properties

        // TODO: Define light Origin
        public Vector3 Origin;

        // TODO: Define light Intensity
        public double Intensity;

        // TODO: Declare light diffuse color to (1, 1, 1)
        public Vector3 DiffuseColor = new Vector3(1, 1, 1);

        #endregion


        #region Lighting

        public virtual Double GetAttenuationFactor(Vector3 point)
        {
            return 1.0;
        }

        public virtual void SetLightRayAt(Vector3 point, Ray ray)
        {
        }

        #endregion
    }
}
