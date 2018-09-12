using Binding;
using Plugins.UI;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(RectTransform))]
    public class IgnoreSafeArea : MonoBehaviour
    {
        [Bind] private RectTransform _rect;
        
        private void Awake()
        {
            this.Bind();
        }

        private void OnEnable()
        {
            _rect.offsetMax = new Vector2(0, Frame.Instance.TopSafePadding);
            _rect.offsetMin = new Vector2(0, -Frame.Instance.BottomSafePadding);
        }
    }
}