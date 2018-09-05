using System;
using Binding;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Store.Equipment
{
    public class EquippedActive : RegisterableBehaviour
    {
        public Buyable Buyable;
        public BuyableManager Manager { get; private set; }

        public override void DidRegister()
        {
            this.Bind();
            Manager = Buyable.Manager;
            if (Manager != null)
                Manager.OnEquipChanged += OnEquip;
            UpdateActive();
        }

        public override void WillUnregister()
        {
            base.WillUnregister();
            if (Manager != null)
                Manager.OnEquipChanged -= OnEquip;
        }

        private void OnDestroy()
        {
            if (Manager != null)
                Manager.OnEquipChanged -= OnEquip;
        }

        private void OnEquip(object sender, EventArgs e)
        {
            if (this == null)
                Debug.LogError($"Destroyed: {this}");
            UpdateActive();
        }

        private void UpdateActive()
        {
            gameObject.SetActive(Buyable.Equipped);
        }
    }
}