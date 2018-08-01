using System;
using Binding;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Store
{
    [RequireComponent(typeof(Image))]
    public class EquippedIcon : MonoBehaviour
    {
        public BuyableManager Manager;

        [Bind] private Image _image;

        private void Awake()
        {
            this.Bind();
            Manager.OnEquipChanged += OnEquip;
            UpdateSprite();
        }

        private void OnDestroy()
        {
            Manager.OnEquipChanged -= OnEquip;
        }

        private void OnEquip(object sender, EventArgs e)
        {
            UpdateSprite();
        }

        private void UpdateSprite()
        {
            _image.sprite = Manager.SingleEquipped.Icon;
        }
    }
}