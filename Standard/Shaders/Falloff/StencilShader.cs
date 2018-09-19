using Binding;
using Standard.Fog;
using UnityEngine;

namespace Standard.Shaders.Falloff
{
    [RequireComponent(typeof(MaterialCollector))]
    [ExecuteInEditMode]
    public class StencilShader : MonoBehaviour
    {
        public FogConfig Config;
    
        [Bind] private MaterialCollector _materials;

        private void Awake()
        {
            this.Bind();
        }

        private void Start()
        {
            UpdateShaders();
        }

        private void UpdateShaders()
        {
            foreach (var r in _materials.Renders)
            {
                var prop = new MaterialPropertyBlock();
                r.GetPropertyBlock(prop);
                Config.Apply(prop);
                r.SetPropertyBlock(prop);
            }
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (!Application.isPlaying)
                UpdateShaders();
        }
#endif
    }
}