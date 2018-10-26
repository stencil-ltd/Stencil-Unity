using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Store
{
    public class BaseBuyableListing : MonoBehaviour, IPointerClickHandler
    {
        [Serializable]
        public class BuyableEvent : UnityEvent<Buyable>
        {}
        
        public Buyable Buyable { get; private set; }
        
        [Header("Events")]
        public BuyableEvent OnUpdateBuyable;
        
        [Header("Equipped")] 
        public GameObject[] EquippedUI = { };
        public GameObject[] UnequippedUI = { };
        
        [Header("Acquired")]
        public GameObject[] AcquiredUI = { };
        public GameObject[] UnacquiredUI = { };
        
        [Header("Selected")]
        public GameObject[] SelectedUI = { };
        public GameObject[] UnselectedUI = { };

        public event EventHandler OnClick;
        
        public bool Selected { get; private set; }

        public void Configure(Buyable buyable, bool selected = false)
        {
            if (Buyable != null)
            {
                Buyable.OnAcquireChanged -= OnAcquireChanged;
                Buyable.OnEquipChanged -= OnEquipChanged;
            }
            Buyable = buyable;
            Buyable.OnAcquireChanged += OnAcquireChanged;
            Buyable.OnEquipChanged += OnEquipChanged;
            Selected = selected;
            UpdateBuyable();
        }

        public void Select(bool selected)
        {
            if (selected == Selected) return;
            Selected = selected;
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
            foreach (var ui in AcquiredUI)
                ui.SetActive(Buyable.Acquired);
            foreach (var ui in UnacquiredUI)
                ui.SetActive(!Buyable.Acquired);
            foreach (var ui in EquippedUI)
                ui.SetActive(Buyable.Equipped);
            foreach (var ui in UnequippedUI)
                ui.SetActive(!Buyable.Equipped);
            foreach (var ui in SelectedUI)
                ui.SetActive(Selected);
            foreach (var ui in UnselectedUI)
                ui.SetActive(!Selected);

            OnBuyableUpdated();
            OnUpdateBuyable?.Invoke(Buyable);
        }

        protected virtual void OnBuyableUpdated()
        {
            
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke(this, EventArgs.Empty);
        }
    }
}