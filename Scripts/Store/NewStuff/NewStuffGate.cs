using System.Linq;
using Currencies;
using State.Active;
using Store.Util;
using UnityEngine;

namespace Store.NewStuff
{
    public class NewStuffGate : ActiveGate
    {
        public BuyableManager Manager;
        public Currency Currency;

        public int BuyableCount 
            => Manager.Buyables.Sum(buyable => buyable.CanBuy() ? 1 : 0);

        public int? LastBuyableCount
        {
            get
            {
                var ret = PlayerPrefs.GetInt($"last_buyable_{Manager.Id}", -1);
                if (ret == -1) return null;
                return ret;
            }
            set
            {
                PlayerPrefs.SetInt($"last_buyable_{Manager.Id}", value ?? -1);
                PlayerPrefs.Save();
            }
        }

        public override void Register(ActiveManager manager)
        {
            base.Register(manager);
            Currency.OnSpendableChanged += OnChange;            
        }

        private void OnDestroy()
        {
            LastBuyableCount = BuyableCount;
            Currency.OnSpendableChanged -= OnChange;
        }

        private void OnChange(object sender, Currency e)
        {
            ActiveManager.Check();
        }

        public override bool? Check()
        {
            var last = LastBuyableCount;
            var count = BuyableCount;
            if (last == null) return count > 0;
            return BuyableCount > LastBuyableCount;
        }
    }
}