﻿using System;
using Standard.Audio;
using Store.Util;
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
        public bool Selectable;
        public bool ShowLocked;
        
        [Header("Audio")]
        public AudioClip OnPurchase;
        public AudioClip OnEquip;

        [Header("Events")] 
        public BaseBuyableListing.BuyableEvent OnSelect;

        [Header("Query")] 
        public BuyableQueryUi Query = new BuyableQueryUi();
        
        private void Start()
        {
            Refresh();
        }

        private void Refresh()
        {
            Container.DestroyAllChildren();
            if (Manager == null) return;
            var q = Query.ToQuery();
            foreach (var b in Manager.Buyables(q))
            {
                var listing = Instantiate(ListingPrefab, Container, false);
                listing.transform.localPosition = Vector3.zero;
                listing.transform.localRotation = Quaternion.identity;
                listing.transform.localScale = Vector3.one;
                listing.Configure(b, b.Equipped);
                if (Selectable)
                    listing.OnClick += (sender, args) => Select(listing);
                if (b.Equipped)
                    OnSelect?.Invoke(b);
            }
        }

        private void Select(BaseBuyableListing listing)
        {
            if (listing.Selected && listing.Buyable.Acquired)
            {
                listing.Buyable.Equipped = true;
                return;
            }
            
            foreach (var t in Container.GetChildren())
            {
                var other = t.GetComponent<BaseBuyableListing>();
                if (!other) continue;
                other.Select(listing == other);
            }
            OnSelect?.Invoke(listing.Buyable);
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