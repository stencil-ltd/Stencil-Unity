using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Image))]
    public class TapToDismiss : MonoBehaviour, IPointerClickHandler
    {
        public GameObject ToDismiss;
        public UnityEvent CustomDismiss;

        public void OnPointerClick(PointerEventData eventData)
        {
            CustomDismiss?.Invoke();
            if (ToDismiss != null) ToDismiss.SetActive(false);
        }
    }
}