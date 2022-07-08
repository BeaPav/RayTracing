using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PG2.Mathematics;
using PG2.Rendering;

namespace PG2.Lighting
{
    public class PointLight : Light
    {
        #region Properties

        // Declare light Linear attenuation factor coefficient to 0.02
        double LinearAttenuation = 0.02;

        // Declare light Quadratic attenuation factor coefficient to 0.00
        double QuadraticAttenuation = 0.00;

        #endregion


        #region Lighting

        public override Double GetAttenuationFactor(Vector3 point)
        {
            // TODO: Calculate light attenuation factor for point, use .Length method for the length of a vector

            double r = (Origin - point).Length;
            return 1.0 / (1.0 + LinearAttenuation * r + QuadraticAttenuation * r * r);
        }


        public override void SetLightRayAt(Vector3 point, Ray ray)
        {
            // TODO: Set normalized light vector from point to Origin of the light, use ray.Set() method

            ray.Set(point, (Origin - point).Normalized);
        }

        #endregion
    }
}
