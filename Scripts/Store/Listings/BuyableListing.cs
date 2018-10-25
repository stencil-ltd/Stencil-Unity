using System;
using Binding;
using Particles;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Store
{
    [RequireComponent(typeof(Button))]
    public class BuyableListing : BaseBuyableListing
    {
        public GameObject CoinSection;
        public Text CoinText;

        public GameObject Highlight;
        public Image Checkmark;
        public GameObject Equip;

        public StandardParticle BuyParticlePrefab;

        [Bind] 
        private Button _button;

        private void Awake()
        {
            this.Bind();
            _button.onClick.AddListener(() =>
            {
                if (!Buyable.Acquired)
                {
                    if (Buyable.AttemptToBuy() && BuyParticlePrefab)
                        Instantiate(BuyParticlePrefab, CoinSection.transform.parent).gameObject.SetActive(true);
                }
                else if (!Buyable.Equipped || !Buyable.Manager.SingleEquip)
                {
                    Buyable.Equipped = !Buyable.Equipped;
                }
            });

            foreach (var i in GetComponentsInChildren<Image>())
                i.enabled = true;

            _button.enabled = true;
        }

        protected override void OnBuyableUpdated()
        {
            var acquired = Buyable.Acquired;
            var equipped = Buyable.Equipped;
            CoinSection.gameObject.SetActive(!acquired);
            CoinText.text = $"x{Buyable.Price}";
            Checkmark.gameObject.SetActive(equipped);
            Equip.gameObject.SetActive(acquired && !equipped);
            if (Highlight != null)
                Highlight.SetActive(equipped);
        }
    }
}