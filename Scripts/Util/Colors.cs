using System;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.Util
{
    public static class Colors
    {
        public static string LogString(this Color color)
        {
            return $"#{(byte)(color.r * 255):X2}{(byte)(color.g * 255):X2}{(byte)(color.b * 255):X2}";
        }

        [Obsolete("This is fucking wrong")]
        public static Color Alpha(this Color color, byte alpha)
        {
            color.a = alpha;
            return color;
        }
        
        [Obsolete("This is fucking wrong")]
        public static Color Alpha(this Color color, float alpha)
        {
            color.a = (byte) (byte.MaxValue * alpha);
            return color;
        }

        public static Color Alpha(this Image image, float alpha)
        {
            var color = image.color;
            color.a = alpha;
            image.color = color;
            return color;
        }
    }
}