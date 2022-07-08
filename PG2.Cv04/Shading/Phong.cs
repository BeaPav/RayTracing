using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PG2.Mathematics;
using PG2.Lighting;

namespace PG2.Shading
{
    public class Phong : Shader
    {
        #region Properties

        // TODO: Declare Diffuse, Specular and Ambient Color. Set all values to (0, 0, 0)
        // TODO: Declare Shininees, set default values to zero

        public Vector3 DiffuseColor = new Vector3(0, 0, 0);
        public Vector3 SpecularColor = new Vector3(0, 0, 0);
        public Vector3 AmbientColor = new Vector3(0, 0, 0);
        public Double Shininess = 0;

        #endregion


        #region Init

        public Phong()
        {
        }

        public Phong(Vector3 diffuseColor)
        {
            // TODO: Initialize class member DiffuseColor
            DiffuseColor = diffuseColor;
        }

        public Phong(Vector3 diffuseColor, Vector3 specularColor)
        {
            // TODO: Initialize class members DiffuseColor, SpecularColor
            DiffuseColor = diffuseColor;
            SpecularColor = specularColor;
        }

        public Phong(Vector3 diffuseColor, Vector3 specularColor, Vector3 ambientColor, Double shininess)
        {
            // TODO: Initialize class members DiffuseColor, SpecularColor, AmbientColor, Shininess
            DiffuseColor = diffuseColor;
            SpecularColor = specularColor;
            AmbientColor = ambientColor;
            Shininess = shininess;
        }

        #endregion


        #region Shading

        public override Vector3 GetColor(Vector3 point, Vector3 normal, Vector3 viewDir, Vector3 lightDir, Double attenuation, Light light)
        {
            // TODO: Calculate diffuseFactor being dot product of normal and light direction
            // scaled by given light attenuation. Clamp negative values to zero
            double diffuseFactor = Math.Max(0, attenuation * normal * lightDir * light.Intensity);

            // TODO: Calculate reflection vector between light direction and object normal
            Vector3 reflectionVector = ((2 * (lightDir * normal)) * normal - lightDir).Normalized;

            // TODO: Calculate specularFactor being dot product of view direction and
            // reflection vector powered by Shininess and scaled by given light attenuation
            Double specularFactor = Math.Pow(viewDir * reflectionVector, Shininess) * attenuation * light.Intensity;

            Vector3 color = Vector3.Zero;
            // TODO: Accumulate diffuse color of shader modulated (use operator '^')
            // with diffuse color of light scaled by diffuseFactor
            color += DiffuseColor ^ light.DiffuseColor * diffuseFactor;

            // TODO: Accumulate specular color of shader modulated (use operator '^')
            // with diffuse color of light scaled by specularFactor
            color += SpecularColor ^ light.DiffuseColor * specularFactor;

            return color;
        }

        public override Vector3 GetAmbientColor(Vector3 point)
        {
            return AmbientColor;
            //return Vector3.Zero; // Please remove me after code completion !
        }

        #endregion
    }
}
