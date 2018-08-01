using System;
using System.Security.Cryptography.X509Certificates;
using GoogleMobileAds.Api;
using Plugins.Ads;
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
    
    public class AdmobBannerArea : Permanent<AdmobBannerArea>
    {
        public RectTransform Content;
        public RectTransform Scrim;
            
        public BannerEvent OnChange;

        private BannerView _banner;
        private BannerConfiguration _config;
        private bool _bannerFailed;
        
        private bool _visible;
        public bool IsBannerVisible() => _visible;
        
        public float BannerHeight
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
        
        public void ShowBanner()
        {
            if (_visible) return;
            _visible = true;
            _banner.Show();
            Change();
        }

        public void HideBanner()
        {
            if (!_visible) return;
            _visible = false;
            _banner.Hide();
            Change();
        }

        protected override void Awake()
        {
            base.Awake();
            if (!Valid) return;
            _config = AdSettings.Instance.BannerConfiguration;
            SceneManager.sceneLoaded += OnSceneLoad;
        }

        private void Start()
        {            
            MobileAds.Initialize(AdSettings.Instance.AppConfiguration);
            MobileAds.SetiOSAppPauseOnBackground(true);
            
            _banner = new BannerView(_config, AdSize.SmartBanner, AdPosition.Top);
            _banner.LoadAd(new AdRequest.Builder().Build());
            _banner.OnAdFailedToLoad += (sender, args) => _bannerFailed = true;
            ShowBanner();
        }

        protected override void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoad;
        }

        private void OnSceneLoad(Scene arg0, LoadSceneMode arg1)
        {
            if (!_bannerFailed) return;
            _bannerFailed = false;
            _banner.LoadAd(new AdRequest.Builder().Build());
        }

        private void Change()
        {
            SetBannerSize(BannerHeight);
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
//            Debug.Log($"Banner height is {pixelHeight}. Setting content offset to {offset} [factor = {ratio}]");
        }
    }
}