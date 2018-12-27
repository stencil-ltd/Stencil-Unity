using System;
using System.Collections.Generic;

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
        public long Total;
        [NonSerialized] public long Staged;
        public long InfiniteUntil;
        public List<Multiplier>Multipliers;

        public long Lifetime;
    }
}