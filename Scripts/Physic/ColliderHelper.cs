using System;
using UnityEngine;
using UnityEngine.Events;

namespace Physic
{
    [RequireComponent(typeof(Collider))]
    public class ColliderHelper : MonoBehaviour
    {
        public CollisionEvent CollisionEnter;
        public CollisionEvent CollisionExit;

        public ColliderEvent TriggerEnter;
        public ColliderEvent TriggerExit;
        
        public void OnCollisionEnter(Collision other)
        {
            CollisionEnter?.Invoke(other);
        }

        public void OnCollisionExit(Collision other)
        {
            CollisionExit?.Invoke(other);
        }

        public void OnTriggerEnter(Collider other)
        {
            TriggerEnter?.Invoke(other);
        }

        public void OnTriggerExit(Collider other)
        {
            TriggerExit?.Invoke(other);
        }
    }
    
    [Serializable]
    public class CollisionEvent : UnityEvent<Collision>
    {}
    
    [Serializable]
    public class ColliderEvent : UnityEvent<Collider>
    {}
}