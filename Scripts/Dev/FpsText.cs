using Binding;
using UnityEngine;
using UnityEngine.UI;

namespace Dev
{
    [RequireComponent(typeof(Text))]
    public class FpsText : MonoBehaviour
    {
        [Bind] private Text _text;
        float _deltaTime = 0.0f;
        
        private void Awake()
        {
            this.Bind();
        }

        private void Update()
        {
            _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
            var fps = (int) (1.0f / _deltaTime);
            _text.text = $"{fps}fps";
        }
    }
}