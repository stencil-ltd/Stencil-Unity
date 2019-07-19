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
            return CheckPurchase(productId) == true;
        }

        public static bool? CheckPurchase(string productId)
        {
            return CheckPurchase(productId, out _);
        }
        
        public static bool? CheckPurchase(string productId, out Product product)
        {
            product = null;
            if (!IsReady()) return null;
            product = GetProduct(productId);
            if (product == null) return null;
            return product.hasReceipt;
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

        public static int GetInt([CanBeNull] this PayoutDefinition def)
        {
            var retval = 0;
            if (def != null)
            {
                if (!int.TryParse(def.data, out retval)) 
                    retval = (int) def.quantity;
            }
            return retval;
        }
        
        public static float GetFloat([CanBeNull] this PayoutDefinition def)
        {
            var retval = 0f;
            if (def != null)
            {
                if (!float.TryParse(def.data, out retval))
                    retval = (float) def.quantity;
            }
            return retval;
        }
        
#endif
    }
}