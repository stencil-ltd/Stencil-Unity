using System.Linq;
using JetBrains.Annotations;
using UnityEngine.Purchasing;

namespace Purchasing
{
    public static class StencilIap
    {
#if UNITY_PURCHASING
        public static bool IsReady()
        {
            return CodelessIAPStoreListener.initializationComplete;
        }
        
        [CanBeNull]
        public static Product GetProduct(this IAPButton button)
        {
            return CodelessIAPStoreListener.Instance.GetProduct(button.productId);
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