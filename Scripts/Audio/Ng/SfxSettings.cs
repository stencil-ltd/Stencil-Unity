using UI;
using UnityEngine;
using Util;

namespace Game.Audio
{
    [CreateAssetMenu(menuName = "MergeCars/SFX")]
    public class SfxSettings : Singleton<SfxSettings>
    {
        [Header("Music")]
        public SmartSfx musicIdle;
        public SmartSfx musicTheme;
        
        [Header("Voices")]
        public SmartSfx voiceKat;
        public SmartSfx voiceZane;
        public SmartSfx voiceKobe;
        
        [Header("Sfx")]
        public SmartSfx coins;
        public SmartSfx dropCar;
        public SmartSfx mergeCar;
        public SmartSfx tapHeli;
        public SmartSfx bigWord;
        public SmartSfx openBox;
        public SmartSfx buyCar;
        public SmartSfx wteCar;
        public SmartSfx tap;
        public SmartSfx wheelSpin;
        public SmartSfx wheelReward;
        public SmartSfx rewardButton;
        public SmartSfx garageButton;
        public SmartSfx offlineEarnings;
    }
}