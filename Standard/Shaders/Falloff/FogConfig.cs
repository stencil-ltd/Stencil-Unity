using UnityEngine;

namespace Standard.Fog
{
    [CreateAssetMenu(menuName = "Stencil/Fog")]
    public class FogConfig : ScriptableObject
    {
        [Header("Fog Distance")] 
        public bool UseDistance = false;
        public float MinDist = 30f;
        public float MaxDist = 100f;
        
        [Header("Fog Height")]
        public bool UseHeight = false;
        public float MinHeight = 30f;
        public float MaxHeight = 100f;
        
        [Header("Drop Distance")]
        public bool UseDropDistance = false;
        public float MinDropDist = 30f;
        public float MaxDropDist = 100f;

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