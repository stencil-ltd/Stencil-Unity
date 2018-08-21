using UnityEngine;

namespace Physic
{
    public class ConstantRotation : MonoBehaviour
    {
        public Vector3 Velocity;
        
        private void FixedUpdate()
        {
            transform.Rotate(Velocity * Time.deltaTime);
        }
    }
}