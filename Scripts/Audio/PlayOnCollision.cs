using Binding;
using UnityEngine;

namespace Audio
{
    [RequireComponent(typeof(Collider))]
    [ExecuteInEditMode]
    public class PlayOnCollision : MonoBehaviour
    {
        public bool Repeatable;
        public bool IsTrigger;
        public string ColliderTag;

        public bool Used;

        public AudioSource Sound;
        
        [Bind] public Collider Collider { get; private set; }

        private void Awake()
        {
            this.Bind();
            Sound = Sound ?? GetComponent<AudioSource>();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (IsTrigger) return;
            if (!string.IsNullOrEmpty(ColliderTag) && !other.gameObject.CompareTag(ColliderTag)) return;
            Play();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!IsTrigger) return;
            if (!string.IsNullOrEmpty(ColliderTag) && !other.gameObject.CompareTag(ColliderTag)) return;
            Play();
        }

        private void Play()
        {
            if (Used && !Repeatable) return;
            Debug.Log($"Playing {Sound.clip}");
            Used = true;
            Sound.enabled = true;
            Sound.Play();
        }
    }
}