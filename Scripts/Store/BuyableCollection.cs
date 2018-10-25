using System;
using Standard.Audio;
using UnityEngine;
using Util;

namespace Store
{
    public class BuyableCollection : MonoBehaviour
    {
        [Header("UI")]
        public RectTransform Container;
        
        [Header("Config")]
        public BuyableManager Manager;
        public BaseBuyableListing ListingPrefab;
        
        [Header("Audio")]
        public AudioClip OnPurchase;
        public AudioClip OnEquip;
        
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
                listing.Configure(b, b.Equipped);
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