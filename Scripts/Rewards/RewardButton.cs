using System;
using System.Collections;
using Binding;
using Currencies;
using Lobbing;
using Ads;
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
        
        public Lobber OptionalLobber;
        public GameObject Glow;

        public RewardEvent OnReward;

        [Bind] private Button _button;
        [Bind] private CanvasGroup _group;

        private bool _isShowing;
        private bool _lobbing;

        private void Awake()
        {
            this.Bind();
            _button.onClick.AddListener(_OnButton);
        }

        private void OnEnable()
        {
            StencilAds.Rewarded.OnResult += _OnComplete;
            OptionalLobber?.OnLobEnded.AddListener(OnLobEnded);
        }

        private void OnDisable()
        {
            StencilAds.Rewarded.OnResult -= _OnComplete;
            OptionalLobber?.OnLobEnded.RemoveListener(OnLobEnded);
        }

        private void OnLobEnded(Lob arg0)
        {
            Currency.Commit(arg0.Amount).AndSave();
        }

        private void _OnButton()
        {
            _isShowing = true;
            StencilAds.Rewarded.Show();
        }

        private void _OnComplete(object sender, bool b)
        {
            if (!_isShowing) return;
            _isShowing = false;
            if (!b) return;
            _lobbing = true;
            if (OptionalLobber == null)
            {
                Currency.Add(Amount).AndSave();
                _lobbing = false;
            }
            else
            {
                Currency.Stage(Amount).AndSave();
                StartCoroutine(Lob());
            }       
        }

        private IEnumerator Lob()
        {
            yield return StartCoroutine(OptionalLobber.LobMany(Amount));
            _lobbing = false;
            OnReward?.Invoke(Currency, Amount);
        }

        private void Update()
        {
            var hasAd = StencilAds.Rewarded.IsReady;
            if (!hasAd || _lobbing)
            {
                _button.enabled = false;
                _group.alpha = 0.7f;
                Glow.SetActive(false);
            }
            else
            {
                _button.enabled = true;
                _group.alpha = 1f;
                Glow.SetActive(true);
            }
        }
    }
}