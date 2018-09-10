using PaperPlaneTools;
using UnityEngine;
using UnityEngine.UI;

namespace Ratings
{
    [RequireComponent(typeof(Button))]
    public class RateButton : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() => RateBox.Instance.GoToRateUrl());
        }
    }
}