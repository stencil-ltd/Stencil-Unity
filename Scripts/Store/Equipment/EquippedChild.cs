using System;
using UnityEngine;
using Util;

namespace Store.Equipment
{
    public class EquippedChild : MonoBehaviour
    {
        public BuyableManager Manager;
        public string SpawnName;

        public Buyable Buyable { get; private set; }
        public GameObject Prefab => Buyable.Prefab;
        
        public GameObject Equipped { get; private set; }

        public event EventHandler OnRefresh; 

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
            Equipped = Instantiate(Prefab, Vector3.zero, Quaternion.identity, transform);
            Equipped.GetComponent<StoreListable>()?.ConfigureForPlay();
            
            if (!string.IsNullOrEmpty(SpawnName))
                Equipped.name = SpawnName;
            OnRefresh?.Invoke();
        }
    }
}