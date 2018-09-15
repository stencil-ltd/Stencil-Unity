using Binding;
using UnityEngine;

namespace Standard.Shaders.Falloff
{
    [RequireComponent(typeof(MaterialCollector))]
    public class DistanceDropoff : MonoBehaviour
    {
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
                prop.SetInt("_DistanceDropoff", 1);
                r.SetPropertyBlock(prop);
            }
        }
    }
}