using System;
using System.Collections;
using Runner.Levels;
using UI;
using UnityEngine;
using Util;

namespace Standard.Shaders.Colors
{
    public class ColorController : Controller<ColorController>
    {
        private RunnerLevel _level;

        public ColorState CurrentColor;
        public event EventHandler<ColorState> OnColor;

        public override void DidRegister()
        {
            base.DidRegister();
            LevelController.Instance.OnLevelChanged += OnLevel;
        }

        public override void WillUnregister()
        {
            base.WillUnregister();
            if (LevelController.Instance != null)
                LevelController.Instance.OnLevelChanged -= OnLevel;
        }

        private void OnLevel(object sender, LevelChange e)
        {
            SetLevel(e.Next);
        }

        public void SetLevel(RunnerLevel level)
        {
            if (_level == null)
            {
                _level = level;
                SetCurrentColors();
            }
            else
            {
                Objects.StartCoroutine(LerpLevel(level));
            }
        }

        private void Start()
        {
            SetCurrentColors();
        }

        private void SetCurrentColors()
        {
            if (_level == null) return;
            var render = _level.Render;
            _SetCurrentColors(new ColorState
            {
                Add = render.AddColor,
                Mult = render.MultColor
            });
        }

        private void _SetCurrentColors(ColorState state)
        {
            CurrentColor = state;
            var color = Color.white;
            color *= state.Mult;
            color += state.Add;
            RenderSettings.fogColor = color;
            OnColor?.Invoke(CurrentColor);
        }
        
        private IEnumerator LerpLevel(RunnerLevel change)
        {
            var prev = _level;
            var next = change;
            _level = next;
            
            var style = LevelSettings.Instance.TransitionStyle;
            var curve = style.TransitionCurve;
            var duration = style.TransitionTime;
            var elapsed = -Time.deltaTime;
            var oldMult = prev.Render.MultColor;
            var newMult = next.Render.MultColor;
            var oldAdd = prev.Render.AddColor;
            var newAdd = next.Render.AddColor;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                var norm = curve.Evaluate(elapsed / duration);
                var mult = Color.Lerp(oldMult, newMult, norm);
                var add = Color.Lerp(oldAdd, newAdd, norm);
                _SetCurrentColors(new ColorState
                {
                    Add = add,
                    Mult = mult
                });
                OnColor?.Invoke(CurrentColor);
                yield return null;    
            }
            SetCurrentColors();
        }
    }

    [Serializable]
    public struct ColorState
    {
        public Color Mult;
        public Color Add;
    }
}