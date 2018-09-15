using Binding;
using UnityEngine;

namespace Standard.Shaders.Falloff
{
    [RequireComponent(typeof(MaterialCollector))]
    public class StencilShader : MonoBehaviour
    {
        public bool DistanceFade;
        public bool HeightFade;
        public bool DistanceDropoff;
    
        [Bind] private MaterialCollector _materials;

        private void Awake()
        {
            this.Bind();
        }

        private void Start()
        {
            foreach (var r in _materials.Renders)
            {
                var prop = new MaterialPropertyBlock();
                r.GetPropertyBlock(prop);
                prop.SetInt("_UseDistance", DistanceFade ? 1 : 0);
                prop.SetInt("_UseHeight", HeightFade ? 1 : 0);
                prop.SetInt("_DistanceDropoff", DistanceDropoff ? 1 : 0);
                r.SetPropertyBlock(prop);
            }
        }
    }
}