using Currencies;
using Lobbing;
using Standard.States;
using UI;
using UnityEngine;
using Util;

namespace Store
{
    public class StoreController : Controller<StoreController>
    {
        public Currency Currency;

        public void Click_StoreTab()
        {
            StoreStates.Instance.RequestState(StoreStates.Instance.State.Next());
        }

        public void OnLob(Lob lob)
        {
            Currency?.Commit(lob.Amount).AndSave();
        }
    }
}
