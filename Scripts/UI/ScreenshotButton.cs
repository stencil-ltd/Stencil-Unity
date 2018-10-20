using System.Collections;
using Binding;
using UnityEngine;
using UnityEngine.UI;
using Util;

#if !EXCLUDE_NATSHARE
using NatShareU;
#endif

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
#if EXCLUDE_NATSHARE
                Debug.LogWarning("Need to install NatShare!");
#else
                NatShare.ShareImage(image);
#endif
            }
            Flash?.SetActive(false);
            Flash?.SetActive(true);
            yield return null;
            foreach (var o in Hide)
                o.SetActive(true);
        }
    }
}