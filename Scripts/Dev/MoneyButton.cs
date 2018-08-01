using Currencies;
using UnityEngine;
using UnityEngine.UI;

namespace Dev
{
    [RequireComponent(typeof(Button))]
    public class MoneyButton : MonoBehaviour
    {
        public Currency Currency;
        public long Amount;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() => Currency.Add(Amount).AndSave());
        }
    }
}