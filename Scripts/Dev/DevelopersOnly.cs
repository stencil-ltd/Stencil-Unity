using UnityEngine;

namespace Dev
{
    public class DevelopersOnly : MonoBehaviour
    {
        private void Start()
        {
            if (!Developers.Enabled)
                gameObject.SetActive(false);
        }
    }
}