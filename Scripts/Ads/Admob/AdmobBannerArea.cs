using System;
using Plugins.Ads;
using Plugins.UI;
using UnityEngine;
using UnityEngine.Events;
using Util;

namespace Ads.Admob
{
    [Serializable]
    public enum BannerLocation
    {
        Bottom
    }
    
    [Serializable]
    public class BannerEvent : UnityEvent
    {}
    
    public class AdmobBannerArea : Permanent<AdmobBannerArea>
    {
        public string AndroidUnitId;
        public string IOSUnitId;
        
        public BannerLocation Location;
        public BannerEvent OnChange;

        private BannerConfiguration _config;
        
        protected override void Awake()
        {
            base.Awake();
            if (!Valid) return;
            _config = new BannerConfiguration(AndroidUnitId, IOSUnitId);
            
            
        }
    }
}