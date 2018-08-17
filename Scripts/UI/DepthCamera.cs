using Binding;
using UnityEngine;

namespace UI
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class DepthCamera : MonoBehaviour
    {
        [Bind] 
        private Camera _cam;

        private void Start()
        {
            this.Bind();
            _cam.depthTextureMode = DepthTextureMode.Depth;
        }
    }
}