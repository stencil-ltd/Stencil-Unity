using System;
using Binding;
using UnityEngine;
using UnityEngine.UI;

namespace Store
{
    [RequireComponent(typeof(Renderer))]
    public class EquippedTexture : MonoBehaviour
    {
        public BuyableManager Manager;

        private Material _material;

        private void Awake()
        {
            this.Bind();
            _material = GetComponent<Renderer>().material;
            Manager.OnEquipChanged += OnEquip;
            UpdateTexture();
        }

        private void OnDestroy()
        {
            Manager.OnEquipChanged -= OnEquip;
        }

        private void OnEquip(object sender, EventArgs e)
        {
            UpdateTexture();
        }

        private void UpdateTexture()
        {
            _material.mainTexture = Manager.SingleEquipped.Texture;
        }        
    }
}