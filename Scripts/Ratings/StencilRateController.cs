using PaperPlaneTools;
using UnityEngine;

namespace Ratings
{
    [RequireComponent(typeof(RateBoxPrefabScript))]
    public class StencilRateController : MonoBehaviour
    {
        public bool CheckAtAwake = true;
        public StencilRater Rater;

        private void Awake()
        {
            Rater.OnNever.AddListener(OnNever);
            Rater.OnPositive.AddListener(OnPositive);
            Rater.OnNegative.AddListener(OnNegative);
        }

        private void OnEnable()
        {
            if (CheckAtAwake)
                Invoke(nameof(Check), 0.1f);
        }

        public bool Check()
        {
            if (!RateBox.Instance.CheckConditionsAreMet()) 
                return false;
            Show();
            return true;
        }

        private void Show()
        {
            var rate = RateBox.Instance;
            rate.Statistics.DialogShownAt = rate.Time();
            rate.SaveStatistics();
            Rater.gameObject.SetActive(true);
        }

        private void OnPositive(int arg0)
        {
            RateBox.Instance.GoToRateUrl();
            Rater.Dismiss();
        }

        private void OnNegative(int arg0)
        {
            Rater.AskForFeedback();
        }

        private void OnNever()
        {
            RateBox.Instance.Statistics.DialogIsRejected = true;
            RateBox.Instance.SaveStatistics();
        }
    }
}