using System;
using Binding;
using UnityEngine;
using UnityEngine.Events;

namespace Plugins.Physics
{
    [Serializable]
    public class CollisionEvent : UnityEvent<Collision>
    {}
    
    [Serializable]
    public class ColliderEvent : UnityEvent<Collider>
    {}
    
    [RequireComponent(typeof(Collider))]
    public class ColliderHelper : MonoBehaviour
    {
        public CollisionEvent CollisionEnter;
        public CollisionEvent CollisionExit;

        public ColliderEvent TriggerEnter;
        public ColliderEvent TriggerExit;
        
        private void Awake()
        {
            this.Bind();
        }

        private void OnCollisionEnter(Collision other) 
            => CollisionEnter?.Invoke(other);
        
        private void OnCollisionExit(Collision other) 
            => CollisionExit?.Invoke(other);

        private void OnTriggerEnter(Collider other) 
            => TriggerEnter?.Invoke(other);

        private void OnTriggerExit(Collider other) 
            => TriggerExit?.Invoke(other);
    }
    
}