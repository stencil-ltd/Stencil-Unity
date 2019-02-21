using System.Numerics;

namespace Dirichlet.Numerics
{
    public struct Fraction
    {
        public readonly BigInteger numerator;
        public readonly BigInteger denominator;

        public static Fraction Get(decimal d)
        {
            int[] bits = decimal.GetBits(d);
            BigInteger numerator = (1 - ((bits[3] >> 30) & 2)) *
                                   unchecked(((BigInteger)(uint)bits[2] << 64) |
                                             ((BigInteger)(uint)bits[1] << 32) |
                                             (BigInteger)(uint)bits[0]);
            BigInteger denominator = BigInteger.Pow(10, (bits[3] >> 16) & 0xff);
            return new Fraction(numerator, denominator);
        }

        public Fraction(BigInteger numerator, BigInteger denominator)
        {
            this.numerator = numerator;
            this.denominator = denominator;
        }

        public bool Equals(Fraction other)
        {
            return numerator == other.numerator && denominator == other.denominator;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Fraction other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (numerator.GetHashCode() * 397) ^ denominator.GetHashCode();
            }
        }
    }
}