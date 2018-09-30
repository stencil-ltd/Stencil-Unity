using UnityEngine;

namespace Store
{
    public abstract class StoreListable : MonoBehaviour
    {
        public abstract void ConfigureForStore(Buyable buyable);
        public abstract void ConfigureForPlay(Buyable buyable);
    }
}