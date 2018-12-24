using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class TapToDismiss : MonoBehaviour, IPointerClickHandler, IPointerDownHandler
    {
        public bool downToDismiss;
        
        public GameObject ToDismiss;
        public UnityEvent CustomDismiss;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (downToDismiss) return;
            Dismiss();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!downToDismiss) return;
            Dismiss();
        }

        private void Dismiss()
        {
            CustomDismiss?.Invoke();
            if (ToDismiss != null) ToDismiss.SetActive(false);
        }
    }
}