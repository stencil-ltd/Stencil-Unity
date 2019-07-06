using System.Collections;
using Binding;
using UnityEngine;
using UnityEngine.UI;
using Util;
using Util.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class ScreenshotButton : MonoBehaviour
    {
        public GameObject[] Show = {};
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
            foreach (var o in Show) 
                o.SetActive(true);

            yield return new WaitForEndOfFrame();
            yield return StartCoroutine(Screenshots.ShareScreenshotSimple());
            Flash?.SetActive(false);
            Flash?.SetActive(true);
            yield return null;
            foreach (var o in Hide)
                o.SetActive(true);
            foreach (var o in Show) 
                o.SetActive(false);
        }
    }
}