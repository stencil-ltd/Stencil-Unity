using System.Linq;
using UnityEngine;

namespace Standard.Shaders
{
    public class MaterialCollector : MonoBehaviour
    {
        public bool Shared;
        
        [Header("Debug")]
        public MeshRenderer[] Renders;

        private void Awake()
        {
            Renders = GetComponentsInChildren<MeshRenderer>(true);
        }
    }
}