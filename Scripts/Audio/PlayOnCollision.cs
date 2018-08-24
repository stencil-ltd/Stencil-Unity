using Binding;
using UI;
using UnityEngine;

namespace Audio
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(EasySound))]
    [ExecuteInEditMode]
    public class PlayOnCollision : MonoBehaviour
    {
        public bool IsTrigger;
        public string ColliderTag;
        
        [Bind] public Collider Collider { get; private set; }
        [Bind] public EasySound Sound { get; private set; }

        private void Awake()
        {
            this.Bind();
        }

        private void Start()
        {
            Sound.Source.playOnAwake = false;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (IsTrigger) return;
            if (!string.IsNullOrEmpty(ColliderTag) && !other.gameObject.CompareTag(ColliderTag)) return;
            Debug.Log($"Playing {Sound.Clip}");
            Sound.Source.Play();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!IsTrigger) return;
            if (!string.IsNullOrEmpty(ColliderTag) && !other.gameObject.CompareTag(ColliderTag)) return;
            Debug.Log($"Playing {Sound.Clip}");
            Sound.Source.Play();
        }
    }
}