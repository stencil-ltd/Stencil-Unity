using Binding;
using Scripts.RemoteConfig;
using UnityEngine;

namespace Standard.Fog
{
    [CreateAssetMenu(menuName = "Stencil/Fog")]
    public class FogConfig : ScriptableObject, IRemoteId
    {
        public string remoteKey;
        
        [Header("Fog Distance")] 
        [RemoteField("fog_use_distance")]
        public bool UseDistance = false;
        
        [RemoteField("fog_min_distance")]
        public float MinDist = 30f;
        
        [RemoteField("fog_max_distance")]
        public float MaxDist = 100f;
        
        [Header("Fog Height")]
        [RemoteField("fog_use_height")]
        public bool UseHeight = false;
        
        [RemoteField("fog_min_height")]
        public float MinHeight = 30f;
        
        [RemoteField("fog_max_height")]
        public float MaxHeight = 100f;
        
        [Header("Drop Distance")]
        [RemoteField("fog_use_drop")]
        public bool UseDropDistance = false;
        
        [RemoteField("fog_min_drop")]
        public float MinDropDist = 30f;
        
        [RemoteField("fog_max_drop")]
        public float MaxDropDist = 100f;

        public string ProcessRemoteId(string field) => 
            $"{remoteKey}__{field}";

        private bool _init;
        public void MaybeInit()
        {
            if (_init) return;
            _init = true;
            if (!string.IsNullOrEmpty(remoteKey)) 
                this.BindRemoteConfig();
        }

        public void Apply(MaterialPropertyBlock prop)
        {
            prop.SetInt("_UseHeight", UseHeight ? 1 : 0);
            prop.SetFloat("_HeightMin", MinHeight);
            prop.SetFloat("_HeightMax", MaxHeight);

            prop.SetInt("_UseDistance", UseDistance ? 1 : 0);
            prop.SetFloat("_DistMin", MinDist);
            prop.SetFloat("_DistMax", MaxDist);

            prop.SetInt("_DistanceDropoff", UseDropDistance ? 1 : 0);
            prop.SetFloat("_DropDistMin", MinDropDist);
            prop.SetFloat("_DropDistMax", MaxDropDist);
        }
    }
}