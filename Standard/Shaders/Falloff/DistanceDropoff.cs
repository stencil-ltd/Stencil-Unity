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
                foreach (var m in r.materials)
                {
                    m.SetVector("_DropoffScale", r.transform.lossyScale);
                    m.SetInt("_DistanceDropoff", 1);
                }
            }
        }
    }
}