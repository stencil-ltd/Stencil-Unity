using System.Linq;
using UnityEngine;

namespace Standard.Shaders
{
    public class MaterialCollector : MonoBehaviour
    {
        [Header("Debug")]

        private Material[] _materials;

        public Material[] Materials
        {
            get
            {
                if (_materials == null)
                    _materials = Renders.SelectMany(m => m?.materials).Where(material => material != null).ToArray();
                return _materials;
            }
        }

        public Renderer[] Renders;

        private void Awake()
        {
            Renders = GetRenders();
        }

        public Renderer[] GetRenders() => GetComponentsInChildren<Renderer>(true);

        #region Shader Setters

        public MaterialCollector SetShader(Shader shader)
        {
            foreach (var mat in Materials) 
                mat.shader = shader;
            return this;
        }

        public MaterialCollector SetFloat(string name, float value)
        {
            foreach (var mat in Materials) 
                mat.SetFloat(name, value);
            return this;
        }

        public MaterialCollector SetColor(string name, Color value)
        {
            foreach (var mat in Materials) 
                mat.SetColor(name, value);
            return this;
        }

        #endregion
    }
}