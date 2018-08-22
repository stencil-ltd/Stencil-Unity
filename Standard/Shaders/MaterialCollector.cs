using System.Linq;
using UnityEngine;

namespace Standard.Shaders
{
    public class MaterialCollector : MonoBehaviour
    {
        public Material[] Materials { get; private set; } = { };

        private void Awake()
        {
            var renders = GetComponentsInChildren<MeshRenderer>(true);
            Materials = renders.SelectMany(m => m.materials).ToArray();
        }
    }
}