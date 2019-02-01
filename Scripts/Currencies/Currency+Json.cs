using System;
using System.Collections.Generic;
using Dirichlet.Numerics;
using UnityEngine;

namespace Currencies
{
    public partial class Currency
    {
        [NonSerialized]
        private CurrencyModifier _data;

        private void InitializeData(bool reset)
        {
            var str = Prefs.GetString(Key);
            var fresh = reset || string.IsNullOrEmpty(str);
            if (!fresh)
                _data = JsonUtility.FromJson<CurrencyModifier>(str);
            else
                _data = new CurrencyModifier
                {
                    Multipliers = new List<CurrencyModifier.Multiplier>(),
                    Total = StartAmount,
                    Lifetime = StartAmount
                };
            
            var force = ForceAmount;
            if (Prefs.HasKey(LegacyKey))
            {
                force = Prefs.GetInt(LegacyKey);
                Prefs.DeleteKey(LegacyKey);
            }
            if (force >= 0) _data.Total = (ulong) force;
            _dirty = fresh || force >= 0;
            UpdateTracking();
        }

        private void MarkAdded() => _data.HasAdded = true;
        public bool HasAdded() => _data.HasAdded;

        private void SetInfinite(DateTime? until) => _data.InfiniteUntil = until?.ToBinary() ?? 0L;

        private void SetTotal(ulong total) => _data.Total = total;
        private ulong GetTotal() => _data.Total;

        private void SetStaged(ulong staged) => _data.Staged = staged;
        private ulong GetStaged() => _data.Staged;

        private void SetLifetime(UInt128 lifetime)
        {
            var old = _data.Lifetime;
            _data.Lifetime = lifetime;
            OnLifetimeChanged?.Invoke(this, new CurrencyEvent(this, old, lifetime));
        }

        private UInt128 GetLifetime() => _data.Lifetime;

        private void SetMultipliers(List<CurrencyModifier.Multiplier> mults) => _data.Multipliers = mults;
        private List<CurrencyModifier.Multiplier> GetMultipliers() => _data.Multipliers;
    }
}