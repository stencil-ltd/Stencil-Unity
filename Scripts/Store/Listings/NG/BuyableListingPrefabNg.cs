using Physic;
using UnityEngine;
using Util;

namespace Store.NG
{
    public class BuyableListingPrefabNg : BaseBuyableListing
    {
        [Header("Prefab")]
        public Transform Parent;
        public ConstantRotation Rotation;

        protected override void OnBuyableUpdated()
        {
            var arg0 = Buyable;
            Parent.DestroyAllChildren();
            var obj = Instantiate(arg0.Prefab, Parent);
            var listable = obj.GetComponent<StoreListable>();
            if (listable) listable.ConfigureForStore(arg0, false);
            Rotation.enabled = arg0.Equipped;
            var scale = Vector3.one;
            if (!Selected) scale *= 0.8f;
            Parent.localScale = scale;
        }
    }
}