using System;
using System.Linq;

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
            None, K, M, B, T, Q, Qt, Sx, Sp, Oc, N, D, Ud, Dd, Td, Qd, Qnd, Sxd, Spd, Ocd, Nvd, Vgt 
        }

        public static string FormatAmount(this Format format, long amount)
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

        public static string SanitizeNumber(this string s) 
            => s.ToLower().Replace(",", "").Replace("$", "").Replace("x", "");

        public static bool TryParse(string s, Format format, out long result)
        {
            switch (format)
            {
                case Format.MassiveAmount:
                    return TryParseMassiveAmount(s, out result);
                default:
                    return long.TryParse(s, out result);
            }
        }

        public static bool TryParseMassiveAmount(string s, out long result)
        {
            var last = s.LastOrDefault();
            var mult = 1L;
            if (char.IsLetter(last))
            {
                last = char.ToLower(last);
                var all = (Suffix[]) Enum.GetValues(typeof(Suffix));
                for (int i = 1; i < all.Length; i++)
                {
                    mult *= 1000L;
                    var name = $"{all[i]}".ToLower().Last();
                    if (last == name) break;
                }
                s = s.Remove(s.Length - 1, 1);
            }

            var retval = long.TryParse(s, out result);
            if (retval) result *= mult;
            return retval;
        }
    }
}