using Binding;
using UnityEngine;

namespace Menu.Meter
{
    [ExecuteInEditMode]
    public class FillFollow : MonoBehaviour
    {
        public float Offset;
        public RectTransform Follow;

        [Bind]
        private RectTransform _rect;

        private void Awake()
        {
            this.Bind();
        }

        // Update is called once per frame
        void LateUpdate()
        {
            UpdateFollow();   
        }

        private void UpdateFollow()
        {
            var size = Follow.offsetMin.x + Offset;
            _rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
            var offset = _rect.offsetMin;
            offset.x = 0;
            _rect.offsetMin = offset;   
        }
    }
}
