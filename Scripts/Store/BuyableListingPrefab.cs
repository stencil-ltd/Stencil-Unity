using Binding;
using Physic;
using UnityEngine;
using Util;

namespace Store
{
    [RequireComponent(typeof(BuyableListing))]
    public class BuyableListingPrefab : MonoBehaviour
    {
        public Transform Parent;
        public ConstantRotation Rotation;

        [Bind] 
        private BuyableListing _listing;

        private void Awake()
        {
            this.Bind();
            _listing.OnUpdateBuyable.AddListener(OnUpdateBuyable);
        }

        private void Start()
        {
            // enableable
        }

        private void OnUpdateBuyable(Buyable arg0)
        {
            Parent.DestroyAllChildren();
            var obj = Instantiate(arg0.Prefab, Parent);
            var listable = obj.GetComponent<StoreListable>();
            if (listable) listable.ConfigureForStore(arg0, false);
            Rotation.enabled = arg0.Equipped;
            var scale = new Vector3(1, 1, 1);
            if (!arg0.Equipped)
                scale *= 0.8f;
            Parent.localScale = scale;
        }
    }
}