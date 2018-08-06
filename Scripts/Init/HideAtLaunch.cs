using UnityEngine;

namespace Init
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