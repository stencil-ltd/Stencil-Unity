using System;
using System.Collections;
using Binding;
using JetBrains.Annotations;
using Lobbing;
using Scripts.Util;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Currencies.UI
{
    [RequireComponent(typeof(Button))]
    public class CurrencyButton : MonoBehaviour
    {
        [Header("Configure")]
        public Price Price;
        public bool DisableOnFail;
        public NumberFormats.Format NumberFormat;

        [Header("UI")] 
        public Text AmountText;
        [CanBeNull] public Lobber Lobber;
        public PriceEvent OnSuccess;
        public PriceEvent OnFailure;
        
        [Bind] 
        private Button _button;

        [Bind] 
        private CanvasGroup _canvasGroup;

        public long Amount
        {
            get { return Price.Amount; }
            set { SetAmount(value); }
        }

        protected virtual void Awake()
        {
            this.Bind();
            _button = _button ?? GetComponent<Button>();
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
                Objects.StartCoroutine(Success());
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
                yield return Objects.StartCoroutine(Lobber?.LobMany(Price.Amount, overrides));
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
            => NumberFormat.FormatAmount(price.Amount);
    }
}