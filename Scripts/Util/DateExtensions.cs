using System;

namespace Scripts.Util
{
    public static class DateExtensions
    {
        public static string ShortDateString(this DateTime dateTime) => $"{dateTime:MMddyyyy}";

        public static DateTime GetNext(this DayOfWeek day)
        {
            var start = DateTime.Today.AddDays(1);
            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            int daysToAdd = ((int) day - (int) start.DayOfWeek + 7) % 7;
            return start.AddDays(daysToAdd);
        }
    }
}