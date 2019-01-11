﻿using System;
using System.Linq;
using Analytics;
using Binding;
using Common;
using Dirichlet.Numerics;
using JetBrains.Annotations;
using Plugins.Data;
using Scripts.Prefs;
using Scripts.RemoteConfig;
using UnityEngine;
using Util;

namespace Currencies
{
    [CreateAssetMenu(menuName = "New Currency")]
    public partial class Currency : ScriptableObject, INameable, IRemoteId
    {
        public string Name;

        [RemoteField("currency_max")]
        public long Max = -1;
        [RemoteField("currency_start")]
        public ulong StartAmount = 0;
        [RemoteField("currency_force")]
        public long ForceAmount = -1;

        public Sprite ColorSprite;
        public Sprite PlainSprite;
        public SpriteMultiplier[] MultiplierSprites;
        public SpriteSpecial[] SpecialSprites;
        [CanBeNull] public Sprite InfiniteSprite;
        [CanBeNull] public GameObject Sprite3D;

        public bool Silent;
        
        [HideInInspector]
        public StencilPrefs Prefs = StencilPrefs.Default;

        public string GetName() => Name;
        public string ProcessRemoteId(string id) => $"{Key.ToLower()}_{id}";

        [CanBeNull]
        public Sprite SpecialSprite(string tag)
        {
            return SpecialSprites?.FirstOrDefault(special => special.Tag == tag).Sprite;
        }

        [CanBeNull]
        public Sprite SpriteForMultiplier(int mult)
        {
            return MultiplierSprites?.FirstOrDefault(multiplier => multiplier.Multiplier == mult).Sprite;
        }

        public event EventHandler<Currency> OnTotalChanged;
        public event EventHandler<Currency> OnSpendableChanged;
        public event EventHandler<Currency> OnLifetimeChanged;

        private string LegacyKey => Name?.ToLower() ?? "";
        private string Key => $"resource_{Name}";
        [NonSerialized] private bool _dirty;

        public ulong Total() => GetTotal();
        public ulong Spendable() => Total() - Staged();
        public ulong Staged() => GetStaged();
        public UInt128 Lifetime() => GetLifetime();
        
        private void OnEnable()
        {
            if (Application.isPlaying) this.BindRemoteConfig();
            CurrencyManager.Instance.Register(this);
            InitializeData(false);
            ResetButton.OnGlobalReset += (sender, args) => Clear();
        }

        public void Clear()
        {
            InitializeData(true);
        }

        public CurrencyOperation Add(ulong amount, bool raw = false) => Add(amount, false, raw);
        public CurrencyOperation Stage(ulong amount, bool raw = false) => Add(amount, true, raw);

        public void Unstage()
        {
            var oldTotal = Total();
            var oldSpendable = Spendable();
            SetStaged(0);
            AmountsChanged(oldTotal, oldSpendable);
        }

        private CurrencyOperation Add(ulong amount, bool staged, bool raw)
        {
            if (amount == 0) return Unchanged();
            if (amount < 0) return Fail();
            var mult = raw ? 1 : Multiplier();
            if (mult <= 0) mult = 1;
            amount = (ulong) (amount * mult);
            
            var oldTotal = Total();
            var oldSpendable = Spendable();

            var newTotal = oldTotal + amount;
            if (Max >= 0) newTotal = (ulong) Math.Min(Max, (long) newTotal);
            SetTotal(newTotal);
            if (staged) SetStaged(GetStaged() + amount);
            MarkAdded();
            AmountsChanged(oldTotal, oldSpendable);
            if (!Silent) Debug.Log($"Add {Name} x{amount} [staged={staged}]");
            return Succeed();
        }

        private bool CanSpendInternal(ulong amount, out ulong total, out ulong spendable, out bool shortCircuit)
        {
            total = Total();
            spendable = Spendable();
            shortCircuit = false;
            if (amount == 0 || IsInfinite())
            {
                shortCircuit = true;
                return true;
            }
            return amount <= total;
        }

        public bool CanSpend(ulong amount)
        {
            return CanSpendInternal(amount, out _, out _, out _);
        }

        public CurrencyOperation Spend(ulong amount)
        {
            ulong total;
            ulong spendable;
            bool shortCircuit;
            if (!CanSpendInternal(amount, out total, out spendable, out shortCircuit))
                return Fail();
            if (shortCircuit)
                return Unchanged();
            var oldTotal = Total();
            var oldSpendable = Spendable();
            if (amount > spendable)
            {
                SetStaged(amount - spendable);
                amount = spendable;
            }
            SetTotal(total - amount);
            AmountsChanged(oldTotal, oldSpendable);
            if (!Silent) Debug.Log($"Spend {Name} x{amount}");
            Tracking.Instance.Track($"spend_{Name}", "amount", amount);
            return Succeed();
        }

        public CurrencyOperation Commit(ulong amount, bool bestEffort = false)
        {
            var oldTotal = Total();
            var oldSpendable = Spendable();
            if (amount == 0) return Unchanged();
            if (amount < 0) return Fail();
            var staged = GetStaged();
            if (amount > staged)
            {
                if (bestEffort)
                    amount = staged;
                else return Fail();
            }
            var mult = Multiplier();
            if (mult <= 0) mult = 1;
            amount = (ulong) (amount * mult);
            SetStaged(Math.Max(0, staged - amount));
            AmountsChanged(oldTotal, oldSpendable);
            if (!Silent) Debug.Log($"Commit {Name} x{amount}");
            return Succeed();
        }
        
        public DateTime? InfiniteUntil()
        {
            if (!IsValid(_data.InfiniteUntil)) return null;
            return DateTime.FromBinary(_data.InfiniteUntil);
        }

        public bool IsInfinite()
        {
            return IsValid(_data.InfiniteUntil);
        }

        public CurrencyOperation AddInfinite(TimeSpan duration)
        {
            if (duration.Ticks < 0) return Fail();
            var inf = InfiniteUntil() ?? DateTime.Now;
            inf += duration;
            SetInfinite(inf);
            AnythingChanged();
            if (!Silent) Debug.Log($"Infinite {Name} for {duration.Hours} hours");
            return Succeed();
        }

        public int MultiplierInt() => (int) Multiplier();
        public float Multiplier()
        {
            var best = GetBestMultiplier();
            var retval = best?.Amount ?? 1f;
            if (retval <= 0) retval = 1f;
            return retval;
        }

        private CurrencyModifier.Multiplier GetBestMultiplier()
        {
            var mults = GetMultipliers();
            CurrencyModifier.Multiplier best = null;
            foreach (var entry in mults)
            {
                if (!IsValid(entry.Until)) continue;
                if (best == null || best.Amount < entry.Amount)
                    best = entry;
            }

            return best;
        }

        public DateTime? MultiplierBestExpiration()
        {
            var best = GetBestMultiplier();
            if (best == null) return null;
            return DateTime.FromBinary(best.Until);
        }

        public CurrencyOperation AddMultiplier(float multiplier, TimeSpan duration)
        {
            if (multiplier <= 0) return Fail();
            if (duration.Ticks < 0) return Fail();
            var mults = GetMultipliers();
            CurrencyModifier.Multiplier current = null;
            var found = false;
            foreach (var entry in mults)
            {
                if (entry.Amount.IsAbout(multiplier))
                {
                    current = entry;
                    found = true;
                    break;
                }       
            }

            current = current ?? new CurrencyModifier.Multiplier
            {
                Until = DateTime.Now.ToBinary()
            };
            current.Amount = multiplier;
            current.Until = (DateTime.FromBinary(current.Until) + duration).ToBinary();
            if (!found) mults.Add(current);
            AnythingChanged();
            if (!Silent) Debug.Log($"Multiplier {Name} x{multiplier} for {duration.Hours} hours");
            return Succeed();
        }
        
        public void Save()
        {
            if (!_dirty) return;
            _dirty = false;
            SetMultipliers(GetMultipliers().Where(multiplier => IsValid(multiplier.Until)).ToList());
            var json = JsonUtility.ToJson(_data);
            Prefs.SetString(Key, json);
            if (!Silent) Debug.Log($"Saved {Key}:\n{json}");
        }

        private bool IsValid(long until)
        {
            return until != 0 && DateTime.FromBinary(until) > DateTime.Now;
        }
        
        private void AnythingChanged()
        {
            _dirty = true;
            if (CurrencyController.Instance.SaveOnWrite) Save();
        }

        private void UpdateTracking()
        {
            Tracking.Instance.SetUserProperty(Name, Total());
            Tracking.Instance.SetUserProperty($"{Name}_lifetime", Lifetime());
        }

        private void AmountsChanged(ulong oldTotal, ulong oldSpendable)
        {
            var total = GetTotal();
            if (total > oldTotal) SetLifetime(GetLifetime() + total - oldTotal);
            if (total != oldTotal) OnTotalChanged?.Invoke(this, this);
            if (Spendable() != oldSpendable) OnSpendableChanged?.Invoke(this, this);
            UpdateTracking();
            AnythingChanged();
        }
        
        private CurrencyOperation Fail() => CurrencyOperation.Fail(this);
        private CurrencyOperation Unchanged() => CurrencyOperation.Unchanged(this);
        private CurrencyOperation Succeed() => CurrencyOperation.Succeed(this);

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(Name)}: {Name}, {nameof(Key)}: {Key}";
        }
    }
}