using System;
using Standard.Audio;
using UnityEngine;
using Util;

namespace Store
{
    public class BuyableCollection : MonoBehaviour
    {
        public AudioClip OnPurchase;
        public AudioClip OnEquip;
        
        public RectTransform Container;

        public BuyableManager Manager;
        public BuyableListing ListingPrefab;
        
        private void Start()
        {
            Container.DestroyAllChildren();
            if (Manager == null) return;
            foreach (var b in Manager.Buyables)
            {
                var listing = Instantiate(ListingPrefab, Container, false);
                listing.transform.localPosition = Vector3.zero;
                listing.transform.localRotation = Quaternion.identity;
                listing.transform.localScale = Vector3.one;
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
            SfxOneShot.Instance.Play(OnEquip);
        }

        private void OnAcquire(object sender, EventArgs e)
        {            
            SfxOneShot.Instance.Play(OnPurchase);
        }
    }
}