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
            if (Config == null)
            {
                Debug.LogWarning($"No config specified for {gameObject}");
                return;
            }
            var renders = _materials.Renders;
            if (!Application.isPlaying)
                renders = _materials.GetRenders();
            foreach (var r in renders)
            {
                var prop = new MaterialPropertyBlock();
                if (r == null) continue;
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