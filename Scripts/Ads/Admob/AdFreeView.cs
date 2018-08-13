#if STENCIL_ADMOB
using Ads.Admob;
using UnityEngine;

namespace UI
{
    public class AdFreeView : MonoBehaviour
    {
        public static int Count { get; private set; }

        private void OnEnable()
        {
            Count++;
            AdmobBannerArea.HideBanner();
        }

        private void OnDisable()
        {
            if (--Count == 0)
                AdmobBannerArea.ShowBanner();
        }
    }
}
#endif