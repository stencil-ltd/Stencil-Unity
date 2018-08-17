using System.Collections;
using Plugins.Util;
using Runner.Levels;
using Standard.States;
using UI;
using UnityEngine;

namespace Standard.Fog
{
    [ExecuteInEditMode]
    public class FogController : Controller<FogController>
    {
        public LevelController Levels;
        
        public override void DidRegister()
        {
            base.DidRegister();
            LevelController.Instance.OnLevelChanged += OnLevel;
        }

        private void OnEnable()
        {            
            SetCurrentColors();
        }

        private void SetCurrentColors()
        {
            var settings = LevelSettings.Instance;
            var level = Levels.Level ?? Levels.Levels[0];
            var fog = level.Render.FogColor;
            Shader.SetGlobalColor("_BikeFogColor", fog);
            Shader.SetGlobalFloat("_MinBikeFog", settings.Fog.MinDist);
            Shader.SetGlobalFloat("_MaxBikeFog", settings.Fog.MaxDist);
        }

        private void OnLevel(object sender, LevelChange e)
        {
            if (e.Prev == null)
                SetCurrentColors();
            else
                StartCoroutine(LerpLevel(e));
        }

        private IEnumerator LerpLevel(LevelChange change)
        {
            var style = LevelSettings.Instance.TransitionStyle;
            var curve = style.TransitionCurve;
            var duration = style.TransitionTime;
            var elapsed = -Time.deltaTime;
            var oldMult = change.Prev.Render.FogColor;
            var newMult = change.Next.Render.FogColor;
            Debug.Log($"Lerping fog from <color={oldMult.LogString()}>{oldMult}</color> to <color={newMult.LogString()}>{newMult}</color>");
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                var norm = curve.Evaluate(elapsed / duration);
                var mult = Color.Lerp(oldMult, newMult, norm);        
                Shader.SetGlobalColor("_BikeFogColor", mult);
                yield return null;    
            }
            SetCurrentColors();
        }

        private void Update()
        {
            if (!Application.isPlaying || PlayStates.Instance.State == PlayStates.State.Playing)
                Shader.SetGlobalVector("_BikePosition", Camera.main.transform.position);
        }
    }
}