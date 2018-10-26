using System;

namespace Store.Util
{
    [Serializable]
    public class BuyableQueryUi
    {
        public bool QueryUnlocked;
        public bool Unlocked;
        
        public bool QueryAcquired;
        public bool Acquired;
        
        public bool QueryEquipped;
        public bool Equipped;
        
        public bool QueryAffordable;
        public bool Affordable;

        public BuyableQuery ToQuery()
        {
            var query = new BuyableQuery();
            query.Unlocked = QueryUnlocked ? Unlocked : (bool?) null;
            query.Acquired = QueryAcquired ? Acquired : (bool?) null;
            query.Equipped = QueryEquipped ? Equipped : (bool?) null;
            query.Affordable = QueryAffordable ? Affordable : (bool?) null;
            return query;
        }
        
    }
}