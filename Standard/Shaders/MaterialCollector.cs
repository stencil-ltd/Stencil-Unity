using System.Linq;
using UnityEngine;

namespace Runner.Colors
{
    public class MaterialCollector : MonoBehaviour
    {
        public Material[] Materials { get; private set; } = { };

        private void Awake()
        {
            var renders = GetComponentsInChildren<MeshRenderer>();
            Materials = renders.SelectMany(m => m.materials).ToArray();
        }
    }
}