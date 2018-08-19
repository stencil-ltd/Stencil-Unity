using UnityEngine;

namespace Physic
{
    public class ConstantVelocity : MonoBehaviour
    {
        public Vector3 Velocity;
        
        private void FixedUpdate()
        {
            var pos = transform.position;
            pos += Velocity * Time.deltaTime;
            transform.position = pos;
        }
    }
}