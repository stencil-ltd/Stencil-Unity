using System;
using System.Linq;
using Currencies;
using Standard.Currencies;
using State.Active;

namespace Store
{
    public class BuyableSelectionGate : ActiveGate
    {
        [Serializable]
        public enum SelectionType
        {
            None, Owned, OwnedEquipped, 
            RequirementsWrench, RequirementsCash,
            GetWrench, GetCash, GetSpecial
        }

        public bool Invert;
        public SelectionType[] Selections;

        private Currency _wrenches;
        private Currency _cash;
        
        private Buyable _buyable;

        private void Awake()
        {
            _wrenches = CurrencyController.Instance.Wrenches();
            _cash = CurrencyController.Instance.Cash();
        }

        public void Configure(Buyable buyable)
        {
            _buyable = buyable;
            ActiveManager.Check();
        }
        
        public override bool? Check()
        {
            if (_buyable == null)
                return false;

            if (Selections == null || Selections.Length == 0)
                return null;

            var retval = Selections.Contains(GetSelection(_buyable));
            if (Invert) retval = !retval;
            return retval;
        }

        private static SelectionType GetSelection(Buyable buyable)
        {
            if (buyable.Equipped)
                return SelectionType.OwnedEquipped;
            if (buyable.Acquired)
                return SelectionType.Owned;
            
        }
    }
}