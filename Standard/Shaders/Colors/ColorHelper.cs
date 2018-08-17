using Binding;
using UnityEngine;

namespace Standard.Shaders.Colors
{    
    [RequireComponent(typeof(MaterialCollector))]
    public class ColorHelper : MonoBehaviour
    {
        [Bind] private MaterialCollector _materials;

        private void Awake()
        {
            this.Bind();
        }

        private void Start()
        {
            SetColors(ColorController.Instance.CurrentColor);
            ColorController.Instance.OnColor += OnColor;
        }

        private void OnDestroy()
        {
            if (ColorController.Instance != null)
                ColorController.Instance.OnColor -= OnColor;
        }

        public void SetColors(ColorState color)
        {
            foreach (var m in _materials.Materials)
            {
                m.SetColor("_MultColor", color.Mult);
                m.SetColor("_AddColor", color.Add);
            }
        }

        private void OnColor(object sender, ColorState e)
        {
            SetColors(e);
        }
    }
}