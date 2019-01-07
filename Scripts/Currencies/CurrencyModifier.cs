using System;
using System.Collections.Generic;
using Dirichlet.Numerics;

namespace Currencies
{
    [Serializable]
    internal class CurrencyModifier
    {
        [Serializable]
        internal class Multiplier
        {
            public float Amount;
            public long Until;
        }

        public bool HasAdded;
        public ulong Total;
        [NonSerialized] public ulong Staged;
        public long InfiniteUntil;
        public List<Multiplier>Multipliers;

        public UInt128 Lifetime
        {
            set => lifetime = value.ToString();
            get
            {
                UInt128.TryParse(lifetime, out var retval);
                return retval;
            }
        }

        public string lifetime;
    }
}