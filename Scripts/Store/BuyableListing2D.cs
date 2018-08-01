﻿using System.Net.Configuration;
using Binding;
using UnityEngine;
using UnityEngine.UI;

namespace Store
{
    [RequireComponent(typeof(Button))]
    public class BuyableListing2D : BuyableListing
    {
        public Image Icon;
        
        public GameObject CoinSection;
        public Text CoinText;

        public GameObject Highlight;
        public Image Checkmark;
        public GameObject Equip;

        [Bind] 
        private Button _button;

        private void Awake()
        {
            this.Bind();
            _button.onClick.AddListener(() =>
            {
                if (!Buyable.Acquired)
                    Buyable.AttemptToBuy();
                else if (!Buyable.Equipped)
                    Buyable.Equipped = true;
            });
        }

        protected override void UpdateBuyable()
        {
            var acquired = Buyable.Acquired;
            var equipped = Buyable.Equipped;
            Icon.sprite = Buyable.Icon;
            CoinSection.gameObject.SetActive(!acquired);
            CoinText.text = $"x{Buyable.Price}";
            Checkmark.gameObject.SetActive(equipped);
            Equip.gameObject.SetActive(acquired && !equipped);
            if (Highlight != null)
                Highlight.SetActive(equipped);
        }
    }
}