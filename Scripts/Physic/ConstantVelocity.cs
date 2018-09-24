using UnityEngine;

namespace Physic
{
    public class ConstantVelocity : MonoBehaviour
    {
        public Vector3 Velocity;

        public float x
        {
            get { return Velocity.x; }
            set { Velocity.x = value; }
        }

        public float y
        {
            get { return Velocity.y; }
            set { Velocity.y = value; }
        }

        public float z
        {
            get { return Velocity.z; }
            set { Velocity.z = value; }
        }

        private void FixedUpdate()
        {
            var pos = transform.position;
            pos += Velocity * Time.deltaTime;
            transform.position = pos;
        }
    }
}