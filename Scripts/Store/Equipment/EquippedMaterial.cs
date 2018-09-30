using System;
using Binding;
using JetBrains.Annotations;
using UnityEngine;

namespace Store.Equipment
{
    [RequireComponent(typeof(Renderer))]
    public class EquippedMaterial : MonoBehaviour
    {
        public BuyableManager Manager;
        [CanBeNull] public BuyableTag Tag;

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
            if (Tag != null)
            {
                _renderer.material = Manager.GetForTag(Tag)?.Material ?? _renderer.material;
            }
            else
            {
                _renderer.material = Manager.SingleEquipped.Material ?? _renderer.material;
            }
        }        
    }
}