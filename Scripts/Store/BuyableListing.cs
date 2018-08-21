using System;
using Binding;
using Particles;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Store
{
    [RequireComponent(typeof(Button))]
    public class BuyableListing : MonoBehaviour
    {
        public Buyable Buyable;
        
        public GameObject CoinSection;
        public Text CoinText;

        public GameObject Highlight;
        public Image Checkmark;
        public GameObject Equip;

        public StandardParticle BuyParticlePrefab;

        [Bind] 
        private Button _button;

        public BuyableEvent OnUpdateBuyable;

        private void Awake()
        {
            this.Bind();
            _button.onClick.AddListener(() =>
            {
                if (!Buyable.Acquired)
                {
                    if (Buyable.AttemptToBuy() && BuyParticlePrefab)
                        Instantiate(BuyParticlePrefab, CoinSection.transform);
                }
                else if (!Buyable.Equipped)
                {
                    Buyable.Equipped = true;
                }
            });

            foreach (var i in GetComponentsInChildren<Image>())
                i.enabled = true;

            _button.enabled = true;
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

        private void UpdateBuyable()
        {
            var acquired = Buyable.Acquired;
            var equipped = Buyable.Equipped;
            CoinSection.gameObject.SetActive(!acquired);
            CoinText.text = $"x{Buyable.Price}";
            Checkmark.gameObject.SetActive(equipped);
            Equip.gameObject.SetActive(acquired && !equipped);
            if (Highlight != null)
                Highlight.SetActive(equipped);
            OnUpdateBuyable?.Invoke(Buyable);
        }
    }
    
    [Serializable]
    public class BuyableEvent : UnityEvent<Buyable>
    {}
}