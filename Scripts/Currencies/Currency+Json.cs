using System;
using System.Collections.Generic;
using UnityEngine;

namespace Currencies
{
    public partial class Currency
    {
        [NonSerialized]
        private CurrencyModifier _data;

        private void InitializeData(bool reset)
        {
            var str = PlayerPrefs.GetString(Key);
            var fresh = reset || string.IsNullOrEmpty(str);
            if (!fresh)
                _data = JsonUtility.FromJson<CurrencyModifier>(str);
            else
                _data = new CurrencyModifier
                {
                    Multipliers = new List<CurrencyModifier.Multiplier>(),
                    Total = StartAmount
                };
            
            var force = ForceAmount;
            if (PlayerPrefs.HasKey(LegacyKey))
            {
                force = PlayerPrefs.GetInt(LegacyKey);
                PlayerPrefs.DeleteKey(LegacyKey);
            }
            if (force >= 0) _data.Total = force;
            _dirty = fresh || force >= 0;
            UpdateTracking();
        }

        private void MarkAdded() => _data.HasAdded = true;
        public bool HasAdded() => _data.HasAdded;

        private void SetInfinite(DateTime? until) => _data.InfiniteUntil = until?.ToBinary() ?? 0L;

        private void SetTotal(long total) => _data.Total = total;
        private long GetTotal() => _data.Total;

        private void SetStaged(long staged) => _data.Staged = staged;
        private long GetStaged() => _data.Staged;

        private void SetMultipliers(List<CurrencyModifier.Multiplier> mults) => _data.Multipliers = mults;
        private List<CurrencyModifier.Multiplier> GetMultipliers() => _data.Multipliers;
    }
}