using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PG2.Rendering;
using PG2.Mathematics;
using PG2.Shading;

namespace PG2.Modeling
{
    public class Model
    {
        #region Constants

        public const Double Eps = 1e-5;

        public const Int32 Entering = 1;
        public const Int32 Exiting = -1;
        public static List<Model> tracedTransparentModelsStack;

        #endregion


        #region Properties

        public Shader Shader;

        #endregion


        #region Utils

        public static Boolean is3DTransparentObject(Model model)
        {
            if (model.Shader.RefractionFactor > 0 && (model is Sphere || model is Block))
                return true;

            return false;
        }

        public static void initializeTracedModelsStack()
        {
            tracedTransparentModelsStack = new List<Model>();
            tracedTransparentModelsStack.Add(new Model() { Shader = new Phong() }); // Add empty "vacuum" model with default refraction index = 1.0
        }

        public static Double getRefractionIndexRatio(Model model, out int state)
        {
            Double n1 = 1.0;
            Double n2 = 1.0;

            if (tracedTransparentModelsStack.Count > 0 && Object.ReferenceEquals(model, tracedTransparentModelsStack.Last()))
            {   // exiting transparent model
                n1 = tracedTransparentModelsStack.Last().Shader.RefractionIndex;
                tracedTransparentModelsStack.RemoveAt(tracedTransparentModelsStack.Count - 1);
                n2 = tracedTransparentModelsStack.Last().Shader.RefractionIndex;
                state = Exiting;
            }
            else
            {   // entering transparent model
                n2 = model.Shader.RefractionIndex;
                tracedTransparentModelsStack.Add(model);
                state = Entering;
            }
            return n1 / n2;
        }

        #endregion


        #region Ray Tracing

        // Collide ray with object and return intersection and intersected object
        public virtual void Collide(Ray ray)
        {
        }

        #endregion
    }
}
