using System;

namespace Scripts.Maths
{
    public static class MathExtensions
    {
        public static int AtLeast(this int value, int constraint)
            => Math.Max(value, constraint);
        public static long AtLeast(this long value, long constraint)
            => Math.Max(value, constraint);
        public static float AtLeast(this float value, float constraint)
            => Math.Max(value, constraint);
        public static double AtLeast(this double value, double constraint)
            => Math.Max(value, constraint);
        
        public static int AtMost(this int value, int constraint)
            => Math.Min(value, constraint);
        public static long AtMost(this long value, long constraint)
            => Math.Min(value, constraint);
        public static float AtMost(this float value, float constraint)
            => Math.Min(value, constraint);
        public static double AtMost(this double value, double constraint)
            => Math.Min(value, constraint);
    }
}