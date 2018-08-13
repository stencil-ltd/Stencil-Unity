using Binding;
using NatShareU;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class ScreenshotButton : MonoBehaviour
    {
        [Bind] private Button _button;

        private void Awake()
        {
            this.Bind();
            _button.onClick.AddListener(ClickScreenshot);
        }

        private void ClickScreenshot()
        {
            if (Application.isEditor) return;
            var image = ScreenCapture.CaptureScreenshotAsTexture();
            image.Share();
        }
    }
}