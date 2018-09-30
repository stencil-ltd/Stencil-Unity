using UnityEngine;

namespace Store
{
    [CreateAssetMenu(menuName = "Buyables/Tag")]
    public class BuyableTag : ScriptableObject
    {
        public bool SingleEquip = true;
    }
}