using System;
using System.Collections;
using Binding;
using JetBrains.Annotations;
using Lobbing;
using UnityEngine;
using UnityEngine.UI;

namespace Currencies.UI
{
    [RequireComponent(typeof(Button))]
    public class CurrencyButton : MonoBehaviour
    {
        [Header("Configure")]
        public Price Price;
        public bool DisableOnFail;

        [Header("UI")] 
        public Text AmountText;
        [CanBeNull] public Lobber Lobber;
        public PriceEvent OnSuccess;
        public PriceEvent OnFailure;
        
        [Bind] 
        private Button _button;

        [Bind] 
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            this.Bind();
            if (_button == null)
                throw new Exception($"{gameObject} doesn't have a button");
            _button.onClick.AddListener(Purchase);
        }

        private void Update()
        {
            var canAfford = Price.CanAfford;
            if (DisableOnFail)
                _button.enabled = canAfford; 
            if (_canvasGroup != null) 
                _canvasGroup.alpha = canAfford ? 1 : 0.5f;
        }

        private void Purchase()
        {
            if (Price.Purchase().AndSave())
                StartCoroutine(Success());
            else OnFailure?.Invoke(Price);
        }

        private IEnumerator Success()
        {
            if (Lobber)
            {
                var overrides = new LobOverrides
                {
                    From = AmountText.transform
                };
                yield return StartCoroutine(Lobber?.LobMany(Price.Amount, overrides));
            }
            OnSuccess?.Invoke(Price);
        }

        public void SetAmount(long amount)
        {
            Price.Amount = amount;
            RefreshUi();
        }

        public void SetPrice(Price price)
        {
            Price = price;
            RefreshUi();
        }

        private void RefreshUi()
        {
            if (AmountText)
                AmountText.text = $"x{Format(Price)}";
        }

        protected virtual string Format(Price price)
            => $"{Price.Amount}";
    }
}