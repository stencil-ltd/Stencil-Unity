using Binding;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI
{
    [RequireComponent(typeof(Text))]
    public class TextAmountLerping : MonoBehaviour
    {
        public string Format = "x{0}";
        public string NumberType = "N0";
        public float Duration = 1f;

        [Bind] 
        public Text Text { get; private set; }

        private Coroutine _co;

        private void Awake()
        {
            this.Bind();
        }

        public void SetAmount(long amount, bool animated = true)
        {
            if (Text == null) this.Bind();
            if (_co != null) StopCoroutine(_co);
            _co = null;
            if (animated)
                _co = StartCoroutine(Text.LerpAmount(Format, NumberType, amount, 1f));
            else
                Text.SetAmount(Format, NumberType, amount);
        }
    }
}