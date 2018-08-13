using System;
using UnityEngine;
using Util;

namespace Store
{
    public class BuyableCollection : MonoBehaviour
    {
        public AudioSource OnPurchase;
        public AudioSource OnEquip;
        
        public RectTransform Container;

        public BuyableManager Manager;
        public BuyableListing ListingPrefab;

        private void Awake()
        {
            Container.DestroyAllChildren();
            if (Manager == null) return;
            foreach (var b in Manager.Buyables)
            {
                var listing = Instantiate(ListingPrefab, Container);
                listing.Configure(b);
            }
        }

        private void OnEnable()
        {
            Manager.OnAcquireChanged += OnAcquire;
            Manager.OnEquipChanged += OnEquipped;
        }

        private void OnDisable()
        {
            Manager.OnAcquireChanged -= OnAcquire;
            Manager.OnEquipChanged -= OnEquipped;
        }

        private void OnEquipped(object sender, EventArgs e)
        {
            OnEquip?.Play();
        }

        private void OnAcquire(object sender, EventArgs e)
        {
            OnPurchase?.Play();   
        }
    }
}