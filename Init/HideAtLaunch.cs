using UnityEngine;

namespace Plugins.Init
{
    public class HideAtLaunch : MonoBehaviour
    {
        private void Awake()
        {
            if (!GameInit.Instance.Started)
                gameObject.SetActive(false);
        }
    }
}