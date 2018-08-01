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

        private void OnEquipChanged(object sender, Buyable e)
        {
            UpdateBuyable();
        }

        private void OnAcquireChanged(object sender, Buyable e)
        {
            UpdateBuyable();
        }

        protected virtual void OnDestroy()
        {
            Buyable.OnAcquireChanged -= OnAcquireChanged;
            Buyable.OnEquipChanged -= OnEquipChanged;
        }

        protected abstract void UpdateBuyable();
    }
}