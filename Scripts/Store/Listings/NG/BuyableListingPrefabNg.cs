using Physic;
using UnityEngine;
using Util;

namespace Store.NG
{
    public class BuyableListingPrefabNg : BaseBuyableListing
    {
        public Transform Parent;
        public ConstantRotation Rotation;

        protected override void OnBuyableUpdated()
        {
            var arg0 = Buyable;
            Parent.DestroyAllChildren();
            var obj = Instantiate(arg0.Prefab, Parent);
            var listable = obj.GetComponent<StoreListable>();
            if (listable) listable.ConfigureForStore(arg0, false);
//            obj.transform.localPosition = Vector3.zero;
            Rotation.enabled = arg0.Equipped;
            var scale = new Vector3(1, 1, 1);
            if (!arg0.Equipped)
                scale *= 0.8f;
            Parent.localScale = scale;
        }
    }
}