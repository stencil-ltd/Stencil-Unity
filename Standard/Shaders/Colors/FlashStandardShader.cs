using System;
using System.Collections;
using Binding;
using Plugins.Util;
using UnityEngine;

namespace Standard.Shaders.Colors
{
    public class FlashStandardShader : MonoBehaviour
    {
        public FlashRender FlashSettings = new FlashRender();

        [Bind] private Renderer _render;
        private Material[] _materials;

        private void Awake()
        {
            this.Bind();
            _materials = _render.materials;
            foreach (var m in _materials)
            {
                m.EnableKeyword("_EMISSION");
            }
        }
        
        private DateTime _lastFlash;
        public IEnumerator Flash()
        {            
            var flash = FlashSettings;
            var duration = flash.Duration;
            var curve = flash.Curve;
            var color = flash.Color;
            var now = DateTime.UtcNow;
            if ((now - _lastFlash).TotalSeconds < flash.Cooldown) yield break;
            _lastFlash = now;
            var elapsed = -Time.deltaTime;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                var alpha = curve.Evaluate(elapsed / duration);
                foreach (var m in _materials)
                {
                    var c1 = color * Mathf.LinearToGammaSpace(alpha);
                    m.SetColor ("_EmissionColor", c1);
                }
                yield return null;
            }
            
            foreach (var m in _materials)
            {
                m.SetColor ("_EmissionColor", Color.black * Mathf.LinearToGammaSpace(0f));
            }
        }
        
    }
}