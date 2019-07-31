using System;

namespace Scripts.Util
{
    public static class DateExtensions
    {
        public static string ShortDateString(this DateTime dateTime) => $"{dateTime:MMddyyyy}";

    }
}