using System.Linq;
using UnityEngine;

namespace Standard.Shaders
{
    public class MaterialCollector : MonoBehaviour
    {
        [Header("Debug")]
        public Material[] Materials;
        public MeshRenderer[] Renders;

    private void Awake()
        {
            Renders = GetComponentsInChildren<MeshRenderer>(true);
            Materials = Renders.SelectMany(m => m.materials).ToArray();
        }
    }
}