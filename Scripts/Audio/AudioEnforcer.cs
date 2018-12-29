using UnityEngine;

namespace Scripts.Audio
{
    public class AudioEnforcer : MonoBehaviour
    {
        private void Awake()
        {
            AudioSystem2.Instance.UpdateMixers();
        }

        private void Start()
        {
            AudioSystem2.Instance.UpdateMixers();
        }
    }
}