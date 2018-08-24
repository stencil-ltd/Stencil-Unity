using System.Collections.Generic;
using Binding;
using UI;
using UnityEngine;

namespace Standard.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class SfxOneShot : Controller<SfxOneShot>
    {
        private AudioSource[] _sources;
        private int _index;

        private void Awake()
        {
            _sources = GetComponents<AudioSource>();
        }

        public void Play(AudioClip clip)
        {
            var source = _sources[_index];
            source.PlayOneShot(clip);
            _index = (_index + 1) % _sources.Length;
        }
    }
}