using System;
using Binding;
using UnityEngine;

namespace Store.Equipment
{
    [RequireComponent(typeof(Renderer))]
    public class EquippedMaterial : MonoBehaviour
    {
        public BuyableManager Manager;

        [Bind]
        private MeshRenderer _renderer;

        private void Awake()
        {
            this.Bind();
            Manager.OnEquipChanged += OnEquip;
            Refresh();
        }

        private void OnDestroy()
        {
            Manager.OnEquipChanged -= OnEquip;
        }

        private void OnEquip(object sender, EventArgs e)
        {
            Refresh();
        }

        private void Refresh()
        {
            _renderer.material = Manager.SingleEquipped.Material ?? _renderer.material;
        }        
    }
}