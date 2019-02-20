using Lobbing;
using UnityEngine;
using UnityEngine.Purchasing;
using Price = Currencies.Price;
using Tracking = Analytics.Tracking;

namespace Rewards
{
    public class PurchaseRewardHandler : MonoBehaviour
    {
        public Price Reward;
        public Lobber Lobber;

        public Transform From;

        public void OnPurchaseSuccess(Product product)
        {
            var overrides = new LobOverrides
            {
                From = From
            };
            StartCoroutine(Lobber.LobCurrency(Reward.Currency, (ulong) Reward.GetAmount(), overrides));
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
        {
            Tracking.Record($"Purchase failed {product} ({reason})");
        }
    }
}