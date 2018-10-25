using System;
using UnityEngine;
using UnityEngine.Events;

namespace Store
{
    public class BaseBuyableListing : MonoBehaviour
    {
        [Serializable]
        public class BuyableEvent : UnityEvent<Buyable>
        {}
        
        public Buyable Buyable;
        public BuyableEvent OnUpdateBuyable;

        private void Start()
        {
            if (Buyable != null)
                Configure(Buyable);
        }

        public void Configure(Buyable buyable)
        {
            if (Buyable != null)
            {
                Buyable.OnAcquireChanged -= OnAcquireChanged;
                Buyable.OnEquipChanged -= OnEquipChanged;
            }
            Buyable = buyable;
            Buyable.OnAcquireChanged += OnAcquireChanged;
            Buyable.OnEquipChanged += OnEquipChanged;
            UpdateBuyable();
        }

        private void OnDestroy()
        {
            if (Buyable == null) return;
            Buyable.OnEquipChanged -= OnEquipChanged;
            Buyable.OnAcquireChanged -= OnAcquireChanged;
        }

        private void OnEquipChanged(object sender, Buyable buyable)
        {
            UpdateBuyable();
        }

        private void OnAcquireChanged(object sender, Buyable buyable)
        {
            UpdateBuyable();
        }
        
        private void UpdateBuyable()
        {
            OnBuyableUpdated();
            OnUpdateBuyable?.Invoke(Buyable);
        }

        protected virtual void OnBuyableUpdated()
        {
            
        }
    }
}