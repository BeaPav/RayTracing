using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PG2.Mathematics;
using PG2.Lighting;

namespace PG2.Shading
{
    public class Checker : Shader
    {
        public const double Eps = 1e-5;

        #region Properties

        // Even material
        public Shader Shader0 = new Phong(new Vector3(1, 1, 1));
        // Odd material
        public Shader Shader1 = new Phong(new Vector3(0, 0, 0));
        // Cube size should be included to calculations
        public Double CubeSize = 1;

        #endregion

        #region Init

        public Checker()
        {
        }

        public Checker(Double cubesize)
        {
            CubeSize = cubesize;
        }

        public Checker(Shader shader0, Shader shader1)
        {
            Shader0 = shader0;
            Shader1 = shader1;
        }

        public Checker(Shader shader0, Shader shader1, Double cubesize)
        {
            Shader0 = shader0;
            Shader1 = shader1;
            CubeSize = cubesize;
        }

        #endregion


        #region Shading

        public override Vector3 GetColor(Vector3 point, Vector3 normal, Vector3 viewDir, Vector3 lightDir, Double attenuation, Light light)
        {
            // Floor is number rounding 
            Int32 dx = (Int32)Math.Floor(point.X / CubeSize + Eps);
            Int32 dy = (Int32)Math.Floor(point.Y / CubeSize + Eps);
            Int32 dz = (Int32)Math.Floor(point.Z / CubeSize + Eps);

            return ((((dx + dy + dz) % 2) == 0) ? Shader0 : Shader1).GetColor(point, normal, viewDir, lightDir, attenuation, light);
        }

        public override Vector3 GetAmbientColor(Vector3 point)
        {
            // Floor is number rounding 
            Int32 dx = (Int32)Math.Floor(point.X / CubeSize + Eps);
            Int32 dy = (Int32)Math.Floor(point.Y / CubeSize + Eps);
            Int32 dz = (Int32)Math.Floor(point.Z / CubeSize + Eps);

            return ((((dx + dy + dz) % 2) == 0) ? Shader0 : Shader1).GetAmbientColor(point);
        }

        #endregion
    }
}
