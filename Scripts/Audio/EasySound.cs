using System;
using System.Linq;
using Binding;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Audio;

namespace UI
{
    [RequireComponent(typeof(AudioSource))]
    [ExecuteInEditMode]
    public class EasySound : MonoBehaviour
    {
        public static AudioMixer Mixer;
        
        public static AudioMixerGroup Master;
        public static AudioMixerGroup Music;
        public static AudioMixerGroup Sfx;

        public SoundType Type = SoundType.Sfx;
        public AudioClip Clip;
        
        [Bind] 
        public AudioSource Source { get; private set; }

        [RuntimeInitializeOnLoadMethod]
        public static void OnLoad()
        {
            var mixers = Resources.FindObjectsOfTypeAll<AudioMixer>();
            Mixer = mixers.First(mixer => mixer.name == "Mixer");
            Sfx = Mixer.FindMatchingGroups("Sfx")[0];
            Music = Mixer.FindMatchingGroups("Music")[0];
            Master = Mixer.FindMatchingGroups("Master")[0];
        }

        private void Awake()
        {
            this.Bind();
            UpdateSource();
            
            #if UNITY_EDITOR
            if (!Application.isPlaying)
                InternalEditorUtility.SetIsInspectorExpanded(Source, false);
            #endif
        }
        
        #if UNITY_EDITOR
        private void Update()
        {
            if (Application.isPlaying)
                UpdateSource();
        }

        private void UpdateSource()
        {
            Source.clip = Clip;
            switch (Type)
            {
                case SoundType.Sfx:
                    Source.outputAudioMixerGroup = Sfx;
                    break;
                case SoundType.Music:
                    Source.outputAudioMixerGroup = Music;
                    break;
                case SoundType.None:
                    Source.outputAudioMixerGroup = null;
                    break;
            }
        }
#endif
    }

    [Serializable]
    public enum SoundType
    {
        Sfx, Music, None
    }
}