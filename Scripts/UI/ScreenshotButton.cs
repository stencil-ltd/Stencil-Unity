#if STENCIL_NATSHARE
using System.Collections;
using Binding;
using NatShareU;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class ScreenshotButton : MonoBehaviour
    {
        public GameObject[] Hide = {};
        public GameObject Flash;
        
        [Bind] private Button _button;

        private void Awake()
        {
            this.Bind();
            _button.onClick.AddListener(() => Objects.StartCoroutine(_Screenshot()));
        }

        private IEnumerator _Screenshot()
        {
            foreach (var o in Hide)
                o.SetActive(false);
            
            yield return new WaitForEndOfFrame();
            if (Application.isEditor)
            {
                ScreenCapture.CaptureScreenshot("Screenshot.png");
                Debug.Log("Screenshot Captured");
            }
            else
            {
                var image = ScreenCapture.CaptureScreenshotAsTexture();
                NatShare.ShareImage(image);
            }
            Flash?.SetActive(false);
            Flash?.SetActive(true);
            yield return null;
            foreach (var o in Hide)
                o.SetActive(true);
        }
    }
}
#endif