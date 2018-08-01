using System;
using Binding;
using Currencies;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Rewards
{
    [Serializable]
    public class RewardEvent : UnityEvent<Currency, long>
    {}

    [RequireComponent(typeof(Button), typeof(CanvasGroup))]
    public class RewardButton : MonoBehaviour
    {
        public Currency Currency;
        public long Amount;

        public RewardEvent OnReward;

        [Bind] private Button _button;
        [Bind] private CanvasGroup _group;

        private bool _isShowing;

        private void Awake()
        {
            this.Bind();
            _button.onClick.AddListener(_OnButton);
        }

        private void OnEnable()
        {
            GameAds.Rewarded.OnResult += _OnComplete;
        }

        private void OnDisable()
        {
            GameAds.Rewarded.OnResult -= _OnComplete;
        }

        private void _OnButton()
        {
            _isShowing = true;
            GameAds.Rewarded.Show();
        }

        private void _OnComplete(object sender, bool b)
        {
            if (!_isShowing) return;
            _isShowing = false;
            if (!b) return;
            Currency.Add(Amount).AndSave();
            OnReward?.Invoke(Currency, Amount);
        }

        private void Update()
        {
            var hasAd = GameAds.Rewarded.IsReady;
            if (!hasAd)
            {
                _button.enabled = false;
                _group.alpha = 0f;
            }
            else
            {
                _button.enabled = true;
                _group.alpha = 1f;
            }
        }
    }
}