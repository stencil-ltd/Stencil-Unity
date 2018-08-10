using UnityEngine;

namespace Store
{
    public abstract class BuyableListing : MonoBehaviour
    {
        public Buyable Buyable;

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

        private void OnEquipChanged(object sender, Buyable buyable)
        {
            UpdateBuyable();
        }

        private void OnAcquireChanged(object sender, Buyable buyable)
        {
            UpdateBuyable();
        }

        protected virtual void OnDestroy()
        {
            if (Buyable == null) return;
            Buyable.OnEquipChanged -= OnEquipChanged;
            Buyable.OnAcquireChanged -= OnAcquireChanged;
        }

        protected abstract void UpdateBuyable();
    }
}