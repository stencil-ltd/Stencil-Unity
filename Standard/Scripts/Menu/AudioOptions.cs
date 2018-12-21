using Audio;
using Scripts.Audio;
using UI;
using UnityEngine;

namespace Standard.Menu
{
    public class AudioOptions : MonoBehaviour
    {
        public Toggle Sfx;
        public Toggle Music;

#if STENCIL_AUDIO_LEGACY
        private AudioSystem _audio;
#else
        private AudioSystem2 _audio;
#endif
        
        private void Awake()
        {
            
#if STENCIL_AUDIO_LEGACY
            _audio = AudioSystem.Instance;
#else
            _audio = AudioSystem2.Instance;
#endif
            UpdateAudio();
            
            _audio.OnSfxChanged += OnAudioChange;
            Sfx.OnChanged += (sender, b) => _audio.SoundEnabled = b;
            
            _audio.OnMusicChanged += OnAudioChange;
            Music.OnChanged += (sender, b) => _audio.MusicEnabled = b;
        }

        private void OnDestroy()
        {
            _audio.OnSfxChanged -= OnAudioChange;
            _audio.OnMusicChanged -= OnAudioChange;
        }

        private void OnAudioChange(object sender, bool e)
        {
            UpdateAudio();
        }

        private void UpdateAudio()
        {
            Music.Toggled = _audio.MusicEnabled;
            Sfx.Toggled = _audio.SoundEnabled;
        }
    }
}