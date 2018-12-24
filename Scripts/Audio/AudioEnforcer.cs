using UnityEngine;

namespace Scripts.Audio
{
    public class AudioEnforcer : MonoBehaviour
    {
        private void Start()
        {
            AudioSystem2.Instance.UpdateMixers();
        }
    }
}