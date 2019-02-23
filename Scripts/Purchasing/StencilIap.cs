using System.Linq;
using JetBrains.Annotations;

#if UNITY_PURCHASING
using UnityEngine.Purchasing;
#endif

namespace Purchasing
{
    public static class StencilIap
    {
#if UNITY_PURCHASING
        public static bool IsReady()
        {
            return CodelessIAPStoreListener.initializationComplete;
        }

        public static bool HasPurchased(string productId)
        {
            return IsReady() && GetProduct(productId)?.hasReceipt == true;
        }
        
        [CanBeNull]
        public static Product GetProduct(this IAPButton button)
        {
            return GetProduct(button.productId);
        }

        [CanBeNull]
        public static Product GetProduct(string id)
        {
            return CodelessIAPStoreListener.Instance.GetProduct(id);
        }
        
        public static bool HasProduct(this IAPButton button)
        {
            return IsReady() && CodelessIAPStoreListener.Instance.HasProductInCatalog(button.productId);
        }

        [CanBeNull]
        public static PayoutDefinition GetPayout(this ProductDefinition def, string name)
        {
            return def.payouts?.FirstOrDefault(definition => definition.subtype == name);
        }
#endif
    }
}