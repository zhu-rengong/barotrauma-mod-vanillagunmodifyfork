using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoGenerateXML
{
    public static class MathUtils
    {
        /// <summary>
        /// Returns a scalar t from a value v between a range from min to max. Clamped between 0 and 1.
        /// </summary>
        public static float InverseLerp(float min, float max, float v)
        {
            float diff = max - min;
            // Ensure that we don't get division by zero exceptions.
            if (diff == 0) { return v >= max ? 1f : 0f; }
            return MathHelper.Clamp((v - min) / diff, 0f, 1f);
        }

        public static float Min(params float[] vals)
        {
            return vals.Min();
        }

        public static float Max(params float[] vals)
        {
            return vals.Max();
        }

        public static float Remap(float value, float inMin, float inMax, float outMin, float outMax)
        {
            return MathHelper.Lerp(outMin, outMax, InverseLerp(inMin, inMax, value));
        }
    }
}
