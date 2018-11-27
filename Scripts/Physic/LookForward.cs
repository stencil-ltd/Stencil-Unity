using Binding;
using UnityEngine;

namespace Physic
{
    [RequireComponent(typeof(VelocityTracker))]
    public class LookForward : MonoBehaviour
    {
        public Vector3 up = Vector3.up;
        public Vector3 multiply = Vector3.one;

        [Bind] 
        private VelocityTracker _tracker;

        private void Awake()
        {
            this.Bind();
        }

        private void LateUpdate()
        {
            var vec = _tracker.Velocity;
            vec.Scale(multiply);
            var target = transform.position + vec;
            transform.LookAt(target, up);
        }
    }
}