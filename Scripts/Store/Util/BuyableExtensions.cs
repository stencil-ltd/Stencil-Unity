using System.Collections.Generic;
using System.Linq;

namespace Store.Util
{
    public static class BuyableExtensions
    {
        public static bool CanBuy(this Buyable buyable) 
            => buyable.Unlocked && !buyable.Acquired && buyable.Currency.CanSpend(buyable.Price);

        public static bool Affordable(this Buyable buyable)
            => buyable.Currency.CanSpend(buyable.Price);

        public static Buyable[] Buyables(this BuyableManager mgr, BuyableQuery query = null)
        {
            IEnumerable<Buyable> retval = mgr.Buyables;
            if (query != null)
            {
                if (query.Unlocked != null)
                    retval = retval.Where(buyable => buyable.Unlocked == query.Unlocked);
                if (query.Acquired != null)
                    retval = retval.Where(buyable => buyable.Acquired == query.Acquired);
                if (query.Equipped != null)
                    retval = retval.Where(buyable => buyable.Equipped == query.Equipped);
                if (query.Affordable != null)
                    retval = retval.Where(buyable => buyable.Affordable() == query.Affordable);
            }
            return retval.ToArray();
        }
    }
}