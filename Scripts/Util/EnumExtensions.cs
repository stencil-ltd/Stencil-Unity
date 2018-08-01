using System;

namespace Util
{
    public static class EnumExtensions
    {
        public static T Next<T>(this T src) where T : struct
        {
            if (!typeof(T).IsEnum) throw new ArgumentException($"Argumnent {typeof(T).FullName} is not an Enum");
            var Arr = (T[])Enum.GetValues(src.GetType());
            var j = Array.IndexOf<T>(Arr, src) + 1;
            return (Arr.Length==j) ? Arr[0] : Arr[j];            
        }
    }
}