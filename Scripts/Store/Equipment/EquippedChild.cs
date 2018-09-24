using UnityEngine;
using Util;

namespace Store.Equipment
{
    public class EquippedChild : MonoBehaviour
    {
        public BuyableManager Manager;

        public Buyable Buyable { get; private set; }
        public GameObject Prefab => Buyable.Prefab;
        
        public GameObject Equipped { get; private set; }

        private void OnEnable()
        {
            Refresh();
        }

        public void Refresh()
        {
            var buyable = Manager.SingleEquipped;
            if (buyable == Buyable) return;
            Buyable = buyable;
            transform.DestroyAllChildren();
            Equipped = Instantiate(Prefab, transform);
        }
    }
}