using System;
using Binding;
using UnityEngine;
using UnityEngine.UI;

namespace Store.Equipment
{
    [RequireComponent(typeof(Image))]
    public class EquippedIcon : MonoBehaviour
    {
        public BuyableManager Manager;

        [Bind] private Image _image;

        private void Awake()
        {
            this.Bind();
            if (Manager != null)
                Manager.OnEquipChanged += OnEquip;
            UpdateSprite();
        }

        private void OnDestroy()
        {
            if (Manager != null)
                Manager.OnEquipChanged -= OnEquip;
        }

        private void OnEquip(object sender, EventArgs e)
        {
            UpdateSprite();
        }

        private void UpdateSprite()
        {
            _image.sprite = Manager?.SingleEquipped.Icon;
        }
    }
}