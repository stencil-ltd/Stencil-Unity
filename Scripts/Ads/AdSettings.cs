using System;
using Plugins.Ads;
using UnityEngine;
using Util;

namespace Ads
{
    [Serializable]
    public struct AdId
    {
        public string Android;
        public string Ios;
    }
    
    [CreateAssetMenu(menuName = "Settings/Ads")]
    public class AdSettings : Singleton<AdSettings>
    {
        public AdId AppId;
        public AppIdConfiguration AppConfiguration { get; private set; }        
        
        public AdId BannerId;
        public BannerConfiguration BannerConfiguration { get; private set; }
        
        public AdId InterstitialId;
        public InterstitialConfiguration InterstitialConfiguration { get; private set; }
        
        public AdId RewardedId;
        public RewardedConfiguration RewardConfiguration { get; private set; }

        private void OnEnable()
        {
            AppConfiguration = new AppIdConfiguration(AppId.Android, AppId.Ios);
            BannerConfiguration = new BannerConfiguration(BannerId.Android, BannerId.Ios);
            InterstitialConfiguration = new InterstitialConfiguration(InterstitialId.Android, InterstitialId.Ios);
            RewardConfiguration = new RewardedConfiguration(RewardedId.Android, RewardedId.Ios);
        }
    }
}