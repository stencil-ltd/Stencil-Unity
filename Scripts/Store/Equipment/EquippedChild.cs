using System;
using JetBrains.Annotations;
using UnityEngine;
using Util;

namespace Store.Equipment
{
    public class EquippedChild : MonoBehaviour
    {
        [CanBeNull] public BuyableManager Manager;
        public string SpawnName;
        public bool ShowRoomMode = false;

        public Buyable Buyable { get; private set; }
        public GameObject Prefab => Buyable.Prefab;
        
        public GameObject Equipped { get; private set; }

        public event EventHandler OnRefresh;

        public void Configure(Buyable buyable)
        {
            Buyable = buyable;
            Refresh();
        }

        private void OnEnable()
        {
            Refresh();
            if (Manager)
                Manager.OnEquipChanged += OnEquip;
        }

        private void OnDisable()
        {
            if (Manager)
                Manager.OnEquipChanged -= OnEquip;
        }

        private void OnEquip(object sender, EventArgs e)
        {
            Refresh();
        }

        public void Refresh()
        {
            var buyable = Manager?.SingleEquipped ?? Buyable;
            Buyable = buyable;
            transform.DestroyAllChildren();
            Equipped = Instantiate(Prefab, Vector3.zero, Quaternion.identity, transform);
            if (ShowRoomMode)
            {
                Equipped.GetComponent<StoreListable>()?.ConfigureForStore(buyable, true);
                Equipped.transform.localScale = Vector3.one;
            } else Equipped.GetComponent<StoreListable>()?.ConfigureForPlay(buyable);
            
            if (!string.IsNullOrEmpty(SpawnName))
                Equipped.name = SpawnName;
            OnRefresh?.Invoke();
        }
    }
}