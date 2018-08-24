using Binding;
using Standard.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class ButtonSfx : MonoBehaviour
    {
        public AudioClip Sfx;
        
        [Bind] private Button _button;

        private void Awake()
        {
            this.Bind();
            _button.onClick.AddListener(() => SfxOneShot.Instance.Play(Sfx));
        }
    }
}