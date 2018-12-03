using Binding;
using Scripts.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    [RequireComponent(typeof(Text))]
    public class NumberFormatText : MonoBehaviour
    {
        public NumberFormats.Format format;
        
        [Bind] private Text _text;

        private void Awake()
        {
            this.Bind();
        }

        private void Update()
        {
            long result;
            if (!long.TryParse(_text.text.SanitizeNumber(), out result)) return;
            _text.text = NumberFormats.FormatAmount(result, format);
        }
    }
}