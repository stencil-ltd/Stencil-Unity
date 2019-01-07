using System;
using System.Collections;
using Dirichlet.Numerics;
using Scripts.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Util
{
    public static class TextExtensions
    {
        private static readonly AnimationCurve Curve 
            = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        
        public static void SetAmount(this Text text, string format, string numberType, UInt128 to)
        {
            text.text = string.Format(format, to.ToString(numberType));
        }
        
        public static void SetAmount(this Text text, string format, string numberType, long to)
        {
            text.text = string.Format(format, to.ToString(numberType));
        }
        
        public static void SetAmount(this Text text, string format, NumberFormats.Format numberType, UInt128 to)
        {
            text.text = string.Format(format, numberType.FormatAmount(to));
        }
        
        public static void SetAmount(this Text text, string format, NumberFormats.Format numberType, long to)
        {
            text.text = string.Format(format, numberType.FormatAmount(to));
        }
    }
}