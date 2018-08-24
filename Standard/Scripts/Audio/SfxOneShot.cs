using Binding;
using UI;
using UnityEngine;

namespace Standard.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class SfxOneShot : Controller<SfxOneShot>
    {
        [Bind] public AudioSource Source { get; private set; }

        private void Awake()
        {
            this.Bind();
        }

        public void Play(AudioClip clip) 
            => Source.PlayOneShot(clip);
    }
}