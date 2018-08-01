using UnityEngine;
using Util;

namespace Store
{
    public class BuyableCollection : MonoBehaviour
    {
        public RectTransform Container;

        public BuyableManager Manager;
        public BuyableListing ListingPrefab;

        private void Awake()
        {
            Container.DestroyAllChildren();
            foreach (var b in Manager.Buyables)
            {
                var listing = Instantiate(ListingPrefab, Container);
                listing.Configure(b);
            }
        }
    }
}