using System;
using UnityEngine;

namespace Standard.Fog
{
    [ExecuteInEditMode]
    public class FogPoint : MonoBehaviour
    {
        public FogSettings Fog;
        
        private void Update()
        {
            Shader.SetGlobalVector("_FogPoint", transform.position);
            Shader.SetGlobalFloat("_FogDistMin", Fog.MinDist);
            Shader.SetGlobalFloat("_FogDistMax", Fog.MaxDist);
            Shader.SetGlobalFloat("_FogHeightMin", Fog.MinHeight);
            Shader.SetGlobalFloat("_FogHeightMax", Fog.MaxHeight);
        }
    }

    [Serializable]
    public class FogSettings
    {
        public float MinDist = 30f;
        public float MaxDist = 100f;
        public float MinHeight = 30f;
        public float MaxHeight = 100f;
    }
}