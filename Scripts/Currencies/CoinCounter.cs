using System;
using Binding;
using Dirichlet.Numerics;
using JetBrains.Annotations;
using Lobbing;
using Scripts.Util;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Currencies
{
    public class CoinCounter : MonoBehaviour
    {
        [Serializable]
        public enum CurrencyDisplay
        {
            Spendable,
            Total,
            Staged
        }
        
        [Header("Config")]
        public Currency Currency;
        public float Speed = 5f;
        public bool OnlyCommitAmount;
        public CurrencyDisplay Display = CurrencyDisplay.Spendable;

        [Header("Format")]
        public string String = "{0}";
        public string Format = "N0";
        public NumberFormats.Format CustomFormatter;

        [Header("UI")] 
        public Image Icon;
        public Text Text;
        
        [CanBeNull]
        [Bind]
        public Lobber Lobber { get; private set; }
        
        private void Awake()
        {
            this.Bind();
            if (Icon && Currency)
            {
                Icon.sprite = Currency.BestSprite();
                Icon.SetNativeSize();
            }
        }

        private void OnEnable()
        {
            UpdateText();
        }

        private ulong Amount
        {
            get
            {
                if (Currency == null) return 0L;
                switch (Display)
                {
                    case CurrencyDisplay.Spendable:
                        return Currency.Spendable();
                    case CurrencyDisplay.Total:
                        return Currency.Total();
                    case CurrencyDisplay.Staged:
                        return Currency.Staged();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void Update()
        {
            if (!NumberFormats.TryParse(Text.text.SanitizeNumber(), CustomFormatter, out UInt128 amount)) return;
            var target = Amount;
            UpdateText(UInt128.Lerp(amount, target, Speed * Time.smoothDeltaTime));
        }

        private void UpdateText(UInt128? amount = null)
        {
            amount = amount ?? Amount;
            Text.SetAmount(String, CustomFormatter, amount.Value);
        }
        
        public void OnLobEnd(Lob lob)
        {
            if (Currency == null) return;
            if (OnlyCommitAmount)
                Currency.Commit(lob.Amount).AndSave();
            else
                Currency.Add(lob.Amount).AndSave();
        }

        [CanBeNull]
        public static implicit operator Lobber(CoinCounter counter)
            => counter.Lobber;
    }
}
