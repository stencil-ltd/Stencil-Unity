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

        [Header("UI")] 
        public Text AmountText;
        [CanBeNull] public Lobber Lobber;
        public PriceEvent OnSuccess;
        public PriceEvent OnFailure;
        
        [Bind] 
        private Button _button;

        private void Awake()
        {
            this.Bind();
            _button.onClick.AddListener(Purchase);
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

        public void SetAmount(int amount)
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
                AmountText.text = $"x{Price.Amount}";
        }
    }
}