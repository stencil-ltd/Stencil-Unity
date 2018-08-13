using System;
using UnityEngine; 
using UnityEngine.UI;
using Frame = UI.Frame;

namespace Ads
{
    [Obsolete]
    public class BannerParent : MonoBehaviour
    {
        public RectTransform Scrim;
        public RectTransform Content;
        public Canvas Canvas;

        private CanvasScaler _scaler;

        private void Awake()
        {
            _scaler = Canvas.GetComponent<CanvasScaler>();
        }

        private void Start()
        {
//            MyAds.OnBannerCreated += OnBanner;
//            SetBannerSize(MyAds.BannerHeight);
        }

        private void OnDestroy()
        {
//            MyAds.OnBannerCreated -= OnBanner;
        }

        private void OnBanner(object sender, EventArgs e)
        {
            if (this == null) return;
//            SetBannerSize(MyAds.BannerHeight);
        }

        private void SetBannerSize(float height)
        {
            if (Scrim == null) return;
            var ratio = _scaler.referenceResolution.x / Screen.width;
            height *= ratio;
            var offset = Content.offsetMax;
            offset.y = -height - Frame.Instance.TopSafePadding;
            Content.offsetMax = offset;
            Scrim.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, -offset.y);
            Debug.Log($"Banner height is {height}. Setting content offset to {offset} [factor = {ratio}]");
        }
    }
}