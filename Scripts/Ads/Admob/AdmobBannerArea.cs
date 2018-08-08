#if STENCIL_ADMOB

using System;
using GoogleMobileAds.Api;
using Plugins.UI;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Util;

namespace Ads.Admob
{    
    [Serializable]
    public class BannerEvent : UnityEvent
    {}
    
    public class AdmobBannerArea : Controller<AdmobBannerArea>
    {            
        public static BannerEvent OnChange;

        private static BannerView _banner;
        private static BannerConfiguration _config;
        private static bool _bannerFailed;
        
        private static bool _visible;
        public static bool IsBannerVisible() => _visible;
                
        public RectTransform Content;
        public RectTransform Scrim;
        
        public static float BannerHeight
        {
            get
            {
                if (Application.isEditor || _banner == null)
                    return 225f;
                if (!_visible)
                    return 0f;
                return _banner.GetHeightInPixels();
            }
        }
        
        public static void ShowBanner()
        {
            if (_visible) return;
            _visible = true;
            _banner.Show();
            Change();
        }

        public static void HideBanner()
        {
            if (!_visible) return;
            _visible = false;
            _banner.Hide();
            Change();
        }

        private static bool _init;
        private void Start()
        {            
            if (!_init)
            {
                _init = true;
                _config = AdSettings.Instance.BannerConfiguration;
                MobileAds.Initialize(AdSettings.Instance.AppConfiguration);
                MobileAds.SetiOSAppPauseOnBackground(true);
            
                _banner = new BannerView(_config, AdSize.SmartBanner, AdPosition.Bottom);
                _banner.LoadAd(new AdRequest.Builder().Build());
                _banner.OnAdFailedToLoad += (sender, args) => _bannerFailed = true;
                
                ShowBanner();
            }

            if (_bannerFailed)
            {
                _bannerFailed = false;
                _banner.LoadAd(new AdRequest.Builder().Build());                
            }
            
            Change();
        }
        
        private static void Change()
        {
            Instance.SetBannerSize(BannerHeight);
            OnChange?.Invoke();
        }
        
        private void SetBannerSize(float pixelHeight)
        {            
            var scaler = Frame.Instance.GetComponentInParent<CanvasScaler>();
            var ratio = scaler.referenceResolution.x / Screen.width;
            pixelHeight *= ratio;
            Scrim.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, pixelHeight);
            Content.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, pixelHeight, 
                ((RectTransform) Content.parent).rect.height - pixelHeight);  
            Debug.Log($"Setting banner height to {pixelHeight}");
        }
    }
}
#endif