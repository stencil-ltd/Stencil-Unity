using UnityEngine;

namespace Physic
{
    public class ConstantVelocity : MonoBehaviour
    {
        public Vector3 Velocity;

        public float x
        {
            get { return Velocity.x; }
            set
            {
                var vel = Velocity;
                vel.x = value;
                Velocity = vel;
            }
        }

        public float y
        {
            get { return Velocity.y; }
            set
            {
                var vel = Velocity;
                vel.y = value;
                Velocity = vel;
            }
        }

        public float z
        {
            get { return Velocity.z; }
            set
            {
                var vel = Velocity;
                vel.z = value;
                Velocity = vel;
            }
        }

        private void FixedUpdate()
        {
            var pos = transform.position;
            pos += Velocity * Time.deltaTime;
            transform.position = pos;
        }
    }
}