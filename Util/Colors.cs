using UnityEngine;

namespace Plugins.Util
{
    public static class Colors
    {
        public static Color Alpha(this Color color, byte alpha)
        {
            color.a = alpha;
            return color;
        }
        
        public static Color Alpha(this Color color, float alpha)
        {
            color.a = (byte) (byte.MaxValue * alpha);
            return color;
        }
    }
}