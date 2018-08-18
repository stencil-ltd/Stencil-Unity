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
            Shader.SetGlobalVector("_BikePosition", transform.position);
            Shader.SetGlobalFloat("_MinBikeFog", Fog.MinDist);
            Shader.SetGlobalFloat("_MaxBikeFog", Fog.MaxDist);
        }
    }

    [Serializable]
    public class FogSettings
    {
        public float MinDist = 30f;
        public float MaxDist = 100f;
    }
}