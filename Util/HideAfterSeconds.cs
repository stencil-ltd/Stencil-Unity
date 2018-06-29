using System.Collections;
using UnityEngine;

namespace Plugins.Util
{
    public class HideAfterSeconds : MonoBehaviour
    {
        public float Seconds;

        private void OnEnable()
        {
            StartCoroutine(Countdown());
        }

        private IEnumerator Countdown()
        {
            yield return new WaitForSeconds(Seconds);
            if (gameObject != null)
                gameObject.SetActive(false);   
        }
    }
}