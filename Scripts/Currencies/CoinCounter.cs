using System;
using Binding;
using JetBrains.Annotations;
using Lobbing;
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
        public string String = "x{0}";
        public string Format = "N0";
        
        [Header("UI")]
        public Text Text;
        
        [CanBeNull]
        [Bind]
        public Lobber Lobber { get; private set; }
        
        private Coroutine _co;

        private void Awake()
        {
            this.Bind();
        }

        private void OnEnable()
        {
            if (Currency != null)
            {
                switch (Display)
                {
                    case CurrencyDisplay.Spendable:
                        Currency.OnSpendableChanged += OnChange;
                        break;
                    case CurrencyDisplay.Total:
                    case CurrencyDisplay.Staged:
                        Currency.OnTotalChanged += OnChange;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            UpdateText();
        }

        private void OnDisable()
        {
            if (Currency != null)
            {
                Currency.OnSpendableChanged -= OnChange;
                Currency.OnTotalChanged -= OnChange;
            }
        }

        private long Amount
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

        private void OnChange(object sender, Currency e)
        {
            if (_co != null) StopCoroutine(_co);
            _co = StartCoroutine(Text.LerpAmount(String, Format, Amount, 1f));
        }

        private void UpdateText()
        {
            Text.SetAmount(String, Format, Amount);
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
