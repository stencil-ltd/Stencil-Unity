using Lobbing;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Currencies
{
    public class CoinCounter : MonoBehaviour
    {
        public Currency Currency;
        public Text Text;
        public float Speed = 5f;

        private string _fmt = "x{0}";
        private Coroutine _co;

        private void OnEnable()
        {
            if (Currency != null)
                Currency.OnSpendableChanged += OnChange;
            UpdateText();
        }

        private void OnDisable()
        {
            if (Currency != null)
                Currency.OnSpendableChanged -= OnChange;
        }

        private void OnChange(object sender, Currency e)
        {
            if (_co != null) StopCoroutine(_co);
            _co = StartCoroutine(Text.LerpAmount(_fmt, "N0", e.Spendable(), 1f));
        }

        private void UpdateText()
        {
            Text.SetAmount(_fmt, "N0", Currency?.Spendable() ?? 0);
        }
        
        public void OnLobEnd(Lob lob)
        {
            if (Currency != null)
                Currency.Add(lob.Amount).AndSave();
        }
    }
}
