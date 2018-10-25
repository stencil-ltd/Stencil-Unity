using Binding;
using UnityEngine;
using UnityEngine.UI;

namespace Store
{
    [RequireComponent(typeof(BuyableListing))]
    public class BuyableListing2D : MonoBehaviour
    {
        public Image Icon;

        [Bind] 
        private BuyableListing _listing;

        private void Awake()
        {
            this.Bind();
            _listing.OnUpdateBuyable.AddListener(OnUpdateBuyable);
        }

        private void OnUpdateBuyable(Buyable arg0)
        {
            Icon.sprite = arg0.Icon;
        }
    }
}