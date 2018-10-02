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
                    _materials = Renders.SelectMany(m => m.materials).ToArray();
                return _materials;
            }
        }

        public MeshRenderer[] Renders;

        private void Awake()
        {
            Renders = GetRenders();
        }

        public MeshRenderer[] GetRenders() => GetComponentsInChildren<MeshRenderer>(true);
    }
}