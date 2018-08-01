using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Util
{
    public static class TextExtensions
    {
        private static readonly AnimationCurve Curve 
            = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        
        public static IEnumerator LerpAmount(this Text text, string format, long to, float duration)
        {
            long from = 0;
            long.TryParse(text.text.ToLower().Replace(",", "").Replace("$", "").Replace("x", ""), out from);
            var start = DateTime.UtcNow;
            for (;;)
            {
                var elapsed = (float) (DateTime.UtcNow - start).TotalSeconds;
                var normed = from + (to - from) * Curve.Evaluate(elapsed / duration);
                text.text = string.Format(format, (long) normed);
                if (elapsed >= duration) yield break;
                yield return null;
            }
        }
    }
}