using System;
using Binding;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Toggle : MonoBehaviour
    {
        public Image Checkmark;

        [Bind]
        private Button _button;

        public event EventHandler<bool> OnChanged; 

        public bool Toggled
        {
            get { return Checkmark.gameObject.activeSelf; }
            set
            {
                if (value == Toggled) return;
                Checkmark.gameObject.SetActive(value);
                NotifyChanged();
            }
        }

        private void Awake()
        {
            this.Bind();
            _button.onClick.AddListener(() => Toggled = !Toggled);
        }

        private void NotifyChanged()
        {
            OnChanged?.Invoke(this, Toggled);
        }
    }
}