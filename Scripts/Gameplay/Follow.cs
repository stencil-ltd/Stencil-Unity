using UnityEngine;

namespace Gameplay
{
    public class Follow : MonoBehaviour
    {
        public Transform target;

        private Vector3 _offset;

        private void Awake()
        {
            _offset = target.position - transform.position;
        }

        private void LateUpdate()
        {
            transform.position = target.position - _offset;
        }
    }
}