using Binding;
using UnityEngine;
using UnityEngine.UI;

namespace Store
{
    [RequireComponent(typeof(Button))]
    public class BuyableListing2D : MonoBehaviour
    {
        public Image Icon;
        
        public GameObject CoinSection;
        public Text CoinText;

        public GameObject Highlight;
        public Image Checkmark;
        public GameObject Equip;

        public Buyable Buyable;

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

        private void Start()
        {
            Buyable.OnAcquireChanged += OnAcquireChanged;
            UpdateBuyable();
        }

        private void OnDestroy()
        {
            Buyable.OnAcquireChanged += OnAcquireChanged;
        }

        private void OnAcquireChanged(object sender, Buyable e)
        {
            UpdateBuyable();
        }

        private void UpdateBuyable()
        {
            var acquired = Buyable.Acquired;
            var equipped = Buyable.Equipped;
            Icon.sprite = Buyable.Icon;
            CoinSection.gameObject.SetActive(!acquired);
            CoinText.text = $"x{Buyable.Price}";
            Checkmark.gameObject.SetActive(equipped);
            Equip.gameObject.SetActive(!Checkmark.gameObject.activeSelf);
            if (Highlight != null)
                Highlight.SetActive(equipped);
        }
    }
}