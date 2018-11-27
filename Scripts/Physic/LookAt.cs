using UnityEngine;

namespace Physic
{
    [ExecuteInEditMode]
    public class LookAt : MonoBehaviour
    {
        public Transform target;
        public Vector3 up = Vector3.up;
        public bool inEditor = true;

        private void Update()
        {
            if (!Application.isPlaying && !inEditor) return;
                transform.LookAt(target, up);
        }
    }
}