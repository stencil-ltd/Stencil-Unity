using UnityEngine;

namespace Dev
{
    public class DevelopersOnly : MonoBehaviour
    {
        private void Awake()
        {
            if (!Developers.Enabled)
                gameObject.SetActive(false);
        }
    }
}