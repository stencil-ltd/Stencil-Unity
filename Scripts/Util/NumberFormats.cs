using System;

namespace Scripts.Util
{
    public static class NumberFormats
    {
        [Serializable]
        public enum Format
        {
            None,
            Thousands,
            MassiveAmount
        }

        private enum Suffix
        {
            None, K, M, B, T, Q, Qt
        }

        public static string FormatAmount(long amount, Format format)
        {
            switch (format)
            {
                case Format.MassiveAmount:
                    return FormatMassiveAmount(amount);
                default:
                    return FormatThousands(amount);
            }
        }

        public static string FormatThousands(long amount)
            => $"{amount:N0}";

        public static string FormatMassiveAmount(long amount)
        {
            var suffix = Suffix.None;
            while (amount > 100000)
            {
                amount /= 1000;
                suffix++;
            }
            var strSuffix = suffix == Suffix.None ? "" : $"{suffix}";
            return $"{amount:N0}{strSuffix}";
        }
    }
}