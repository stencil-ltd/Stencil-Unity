using System.Linq;
using UnityEngine;

namespace Standard.Shaders
{
    public class MaterialCollector : MonoBehaviour
    {
        public Material[] Materials { get; private set; } = { };
        public MeshRenderer[] Renders { get; private set; } = { };

    private void Awake()
        {
            Renders = GetComponentsInChildren<MeshRenderer>(true);
            Materials = Renders.SelectMany(m => m.materials).ToArray();
        }
    }
}