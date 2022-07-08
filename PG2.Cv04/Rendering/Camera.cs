using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PG2.Mathematics;
using System.Drawing;
using PG2.Modeling;
using PG2.Shading;
using PG2.Lighting;

namespace PG2.Rendering
{
    public class Camera
    {
        public struct HitPoint
        {
            // TODO: Define HitPoint structure variables: Position, Color, Normal
            public Vector3 Position;
            public Vector3 Color;
            public Vector3 Normal;
        }

        #region Properties

        // TODO: Define camera properties Position, Target
        // TODO: Declare Up vector to (0, 0, 1), FovY value to 45
        public Vector3 Position;
        public Vector3 Target;
        public Vector3 Up = new Vector3(0, 0, 1);
        public double FovY = 45;

        // TODO: Define U, V, W vectors camera to world space
        public Vector3 U;
        public Vector3 V;
        public Vector3 W;

        // TODO: Define frame(picture) properties Bitmap, Width, Height, Pixels buffer
        // TODO: Declare BgColor to (0, 0, 0)

        public Bitmap Bitmap;
        public int Width;
        public int Height;
        public Vector3[] Pixels;

        public Vector3 BgColor = new Vector3(0, 0, 0);

        // Define models in World
        public World World;

        // TODO: Define clipping planes zNear, zFar

        public Double zNear;
        public Double zFar;

        // TODO: Declare UseShadows to control shadows rendering to true
        // TODO: Declare UseAttenuation to decrease light intensity by attenuation to true

        public bool UseShadows = true;
        public bool UseLightAttenuation = true;

        // TODO: Define MaxReflectionLevel, MaxRefractionLevel limit = the maximal number of reflection and refraction iterations

        public int MaxReflectionLevel;
        public int MaxRefractionLevel;

        // TODO: Declare MinIntensity being the minimal intensity to 0.01. Rays with intensity less than min intensity are not traced
        Double MinIntensity = 0.01;

        #endregion

        #region Init

        public Camera(Int32 width, Int32 height)
        {
            // TODO: Initialize class members Width, Height, Bitmap, Pixels buffer

            Width = width;
            Height = height;
            Bitmap = new Bitmap(width, height);
            Pixels = new Vector3[width * height];
        }

        #endregion

        #region Buffer Acess

        public Vector3 GetPixel(Int32 i, Int32 j)
        {
            return Pixels[i + j * Width];
        }

        public void SetPixel(Int32 i, Int32 j, Vector3 color)
        {
            Pixels[i + j * Width] = color;
        }

        #endregion

        #region Rendering

        public void Render()
        {
            RayTrace();
            PresentFrame();
        }

        /// <summary>Derived from Computer Graphics - David Mount.
        /// Implementations can differ - make your own from scratch. 
        /// See http://goo.gl/q6Sz0 (page 84) and http://goo.gl/rB8J6 (page 9-10)
        /// </summary>
        public void RayTrace()
        {
            // TODO: Initialize camera (U, V, W)
            W = (Position - Target).Normalized;
            V = (Up % W).Normalized;
            U = (V % W).Normalized;

            // TODO: Compute perspective projection with FovY as a field of view
            double asp = (double)Width / Height;
            double perspHeight = 2.0 * Math.Tan(MathEx.DegToRad(FovY / 2.0));
            double perspWidth = asp * perspHeight;

            // Ray trace the scene. One ray is enough for one pixel
            for (int r = 0; r < Height; r++)
            {
                for (int c = 0; c < Width; c++)
                {
                    Model.initializeTracedModelsStack();

                    // TODO: Create ray and calculate color with RayTrace()
                    //       Store color to Pixels buffer with SetPixel()

                    double row = perspHeight * (double)r / Height - perspHeight / 2;
                    double col = perspWidth * (double)c / Width - perspWidth / 2;
                    Vector3 direction = (col * U + row * V - W).Normalized;
                    Ray ray = new Ray(Position, direction);
                    SetPixel(r, c, RayTrace(ray, 1.0));
                }
            }
        }

        // Ray trace the generated ray, compute the lighting, shadows, reflections and refractions
        // "ray" parameter is traced ray
        // "intensity" parameter is the light intensity after the iterations
        public Vector3 RayTrace(Ray ray, Double intensity)
        {
            // Exit if actual light intensity is less than MinIntensity
            if (intensity < MinIntensity) return Vector3.Zero;

            // TODO: Calculate ray intersection with all models (primitives) in World, use World.Collide()
            //       Return background color if intersection does not exists else calculate hitpoint color

            World.Collide(ray);
            if (ray.HitModel == null) return BgColor;

            // TODO: Calculate hitPoint.Position, use ray.GetHitPoint()

            HitPoint hitPoint;
            hitPoint.Position = ray.GetHitPoint();

            // TODO: Set hitPoint.Color to ambient color for ray.HitModel object
            hitPoint.Color = ray.HitModel.Shader.GetAmbientColor(hitPoint.Position);
            hitPoint.Normal = ray.HitNormal;

            // TODO: Create light ray
            // TODO: Create and set view direction
            Ray lightRay = new Ray();
            //Vector3 ViewDirection = ray.Direction;

            // For each light in the world do
            foreach (Light light in World.Lights)
            {
                // TODO: Setup light ray to the current light, use light.SetLightRayAt()
                light.SetLightRayAt(hitPoint.Position, lightRay);


                // TODO: Calculate attenuation factor
                Double attenuation = 1.0;
                if (UseLightAttenuation) attenuation = light.GetAttenuationFactor(hitPoint.Position);

                double ambientIntensity = 1.0;

                if (UseShadows && hitPoint.Normal * lightRay.Direction > 0)
                {
                    // TODO: Collide light ray with scene to check for shadows, use World.Collide()
                    World.Collide(lightRay);
                    if (lightRay.HitModel != null)
                    {
                        double lengthToHitPoint = (hitPoint.Position - lightRay.GetHitPoint()).Length;
                        double lengthToLightOrigin = (hitPoint.Position - light.Origin).Length;

                        // TODO: Check if the nearest occlusion object is between light and hit point
                        if (lengthToHitPoint > 0 && lengthToHitPoint < lengthToLightOrigin)
                        {
                            ambientIntensity = ray.HitModel.Shader.GetAmbientColor(hitPoint.Position).Length;
                        }
                    }
                }
                // TODO: Evaluate local shading (e.g. phong-model) and accumulate color, use ray.HitModel.Shader.GetColor()
                //       Don't forget set color to shadow color if hit point is inside a shadow


                hitPoint.Color += ambientIntensity * ray.HitModel.Shader.GetColor(hitPoint.Position, ray.HitNormal, ray.Direction,
                                                                                  lightRay.Direction, attenuation, light);
            }


            // Set n1n2 to the default refraction index ratio n1 / n2 = 1.0 / 1.0       
            Double n1n2 = 1.0;

            bool ReflectionInRefraction = false;
            bool Refracted = false;

            if (Model.is3DTransparentObject(ray.HitModel))
            {
                Int32 state;
                n1n2 = Model.getRefractionIndexRatio(ray.HitModel, out state);
                if (state == Model.Exiting)
                    hitPoint.Normal = -hitPoint.Normal;



                // If RefractionLevel of ray is less than MaxRefractionLevel trace recursively refracted ray
                //    You need to construct the refraction ray, use Vector3.Refract() function
                //    After the ray is created - we can raytrace this ray and add the result to hitPoint.Color
                // TODO: Calculate refraction color
                
                
                if (ray.HitModel.Shader.RefractionFactor > 0.0 && ray.RefractionLevel < MaxRefractionLevel)
                {
                    ReflectionInRefraction = Vector3.Refract(hitPoint.Normal.Normalized, -ray.Direction, n1n2, out Vector3 RefractionRayDir);
                    
                    if (!ReflectionInRefraction)
                    {
                        Ray RefractionRay = new Ray(hitPoint.Position, RefractionRayDir.Normalized, ray.ReflectionLevel, ray.RefractionLevel + 1);
                        hitPoint.Color += RayTrace(RefractionRay, intensity * 0.9);
                        Refracted = true;
                        intensity *= 0.7;
                    }
                    else if (ray.HitModel.Shader.ReflectionFactor > 0.0 && ray.ReflectionLevel < MaxReflectionLevel)
                    {
                        //total reflection happened
                        Ray RefractionRay = new Ray(hitPoint.Position, RefractionRayDir.Normalized, ray.ReflectionLevel + 1, ray.RefractionLevel);
                        hitPoint.Color += RayTrace(RefractionRay, intensity * 0.9);
                    }
                }
                
                
            }


                // If ReflectionLevel of ray is less than MaxReflectionLevel trace recursively reflected ray
                //    If total internal reflection happened during refraction you need call reflection too
                //    You need to construct the reflection ray, use Vector3.Reflect() function
                //    After the ray is created - we can raytrace this ray and add the result to hitPoint.Color
                // TODO: Calculate reflection color

                if (!ReflectionInRefraction && ray.HitModel.Shader.ReflectionFactor > 0.0 && ray.ReflectionLevel < MaxReflectionLevel)
                {
                    Vector3.Reflect(hitPoint.Normal, -ray.Direction, out Vector3 ReflectionRayDir);
                    Ray ReflectionRay = new Ray(hitPoint.Position, ReflectionRayDir.Normalized, ray.ReflectionLevel + 1, ray.RefractionLevel);
                    if (!Refracted && !Model.is3DTransparentObject(ray.HitModel))
                        hitPoint.Color += RayTrace(ReflectionRay, intensity * 0.9);
                    else
                        hitPoint.Color += RayTrace(ReflectionRay, intensity * 0.3);
                }
            
                
            

            return intensity * hitPoint.Color;
            //return Vector3.Zero; // Please remove me after code completion !
        }

        // Create picture. Copy all the pixels from pixel buffer to the Bitmap
        // Color is clamped in post process
        public void PresentFrame()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    // TODO: Retrieve color from Pixels buffer, use GetPixel()
                    //       Don't forget clamp color to max 1.0
                    //       Store pixel color to the Bitmap, use appropriate procedure
                    Vector3 ColorVector = Vector3.Clamp(GetPixel(x, y), 0.0, 1.0);
                    Color color = Color.FromArgb((int)Math.Floor(ColorVector.X * 255),
                                                 (int)Math.Floor(ColorVector.Y * 255),
                                                 (int)Math.Floor(ColorVector.Z * 255));
                    Bitmap.SetPixel(x, y, color);
                }
            }
        }

        #endregion
    }
}
