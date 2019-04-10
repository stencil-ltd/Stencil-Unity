using System;
using Dirichlet.Numerics;
using Res;
using UnityEngine;

namespace Currencies
{
    [Serializable]
    public class CurrencyExchange
    {
        [Tooltip("Null if IAP.")]
        public Currency currency;
        public UInt128 amount;
    }
}