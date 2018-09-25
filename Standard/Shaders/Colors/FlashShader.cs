﻿using System;
using System.Collections;
using Binding;
using UnityEngine;

namespace Standard.Shaders.Colors
{
    [RequireComponent(typeof(MaterialCollector))]
    public class FlashShader : MonoBehaviour
    {
        public FlashRender FlashSettings = new FlashRender();
        [Bind] private MaterialCollector _materials;

        private void Awake()
        {
            this.Bind();
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
                foreach (var r in _materials.Renders)
                {
                    var prop = new MaterialPropertyBlock();
                    r.GetPropertyBlock(prop);
                    prop.SetColor("_FlashColor", color);
                    prop.SetFloat("_Flash", alpha);
                    r.SetPropertyBlock(prop);
                }
                yield return null;
            }
            
            foreach (var r in _materials.Renders)
            {
                var prop = new MaterialPropertyBlock();
                r.GetPropertyBlock(prop);
                prop.SetFloat("_Flash", 0);
                r.SetPropertyBlock(prop);
            }
        }
        
    }
    
    [Serializable]
    public class FlashRender
    {
        public Color Color = Color.white;
        public AnimationCurve Curve;
        public float Duration = 0.3f;
        public float Cooldown = 0.8f;
    }
}