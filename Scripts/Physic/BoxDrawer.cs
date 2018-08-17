using Binding;
using UnityEngine;

namespace Physic
{
    [RequireComponent(typeof(BoxCollider))]
    [ExecuteInEditMode]
    public class BoxDrawer : MonoBehaviour
    {
        public Color Color = Color.magenta;
        [Bind] public BoxCollider Box { get; private set; }
        
        private Vector3 _v3FrontTopLeft;
        private Vector3 _v3FrontTopRight;
        private Vector3 _v3FrontBottomLeft;
        private Vector3 _v3FrontBottomRight;
        private Vector3 _v3BackTopLeft;
        private Vector3 _v3BackTopRight;
        private Vector3 _v3BackBottomLeft;
        private Vector3 _v3BackBottomRight;

        private void Awake()
        {
            this.Bind();
        }

        private void Update()
        {
//            if (Application.isPlaying) return;
            CalcPositons();
            DrawBox();
        }

        void CalcPositons()
        {
            var bounds = Box.bounds;
            var v3Center = bounds.center;
            var v3Extents = bounds.extents;
  
            _v3FrontTopLeft     = new Vector3(v3Center.x - v3Extents.x, v3Center.y + v3Extents.y, v3Center.z - v3Extents.z);  // Front top left corner
            _v3FrontTopRight    = new Vector3(v3Center.x + v3Extents.x, v3Center.y + v3Extents.y, v3Center.z - v3Extents.z);  // Front top right corner
            _v3FrontBottomLeft  = new Vector3(v3Center.x - v3Extents.x, v3Center.y - v3Extents.y, v3Center.z - v3Extents.z);  // Front bottom left corner
            _v3FrontBottomRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y - v3Extents.y, v3Center.z - v3Extents.z);  // Front bottom right corner
            _v3BackTopLeft      = new Vector3(v3Center.x - v3Extents.x, v3Center.y + v3Extents.y, v3Center.z + v3Extents.z);  // Back top left corner
            _v3BackTopRight     = new Vector3(v3Center.x + v3Extents.x, v3Center.y + v3Extents.y, v3Center.z + v3Extents.z);  // Back top right corner
            _v3BackBottomLeft   = new Vector3(v3Center.x - v3Extents.x, v3Center.y - v3Extents.y, v3Center.z + v3Extents.z);  // Back bottom left corner
            _v3BackBottomRight  = new Vector3(v3Center.x + v3Extents.x, v3Center.y - v3Extents.y, v3Center.z + v3Extents.z);  // Back bottom right corner   
        }
        
        void DrawBox() 
        {
            //if (Input.GetKey (KeyCode.S)) {
            Debug.DrawLine (_v3FrontTopLeft, _v3FrontTopRight, Color);
            Debug.DrawLine (_v3FrontTopRight, _v3FrontBottomRight, Color);
            Debug.DrawLine (_v3FrontBottomRight, _v3FrontBottomLeft, Color);
            Debug.DrawLine (_v3FrontBottomLeft, _v3FrontTopLeft, Color);
         
            Debug.DrawLine (_v3BackTopLeft, _v3BackTopRight, Color);
            Debug.DrawLine (_v3BackTopRight, _v3BackBottomRight, Color);
            Debug.DrawLine (_v3BackBottomRight, _v3BackBottomLeft, Color);
            Debug.DrawLine (_v3BackBottomLeft, _v3BackTopLeft, Color);
         
            Debug.DrawLine (_v3FrontTopLeft, _v3BackTopLeft, Color);
            Debug.DrawLine (_v3FrontTopRight, _v3BackTopRight, Color);
            Debug.DrawLine (_v3FrontBottomRight, _v3BackBottomRight, Color);
            Debug.DrawLine (_v3FrontBottomLeft, _v3BackBottomLeft, Color);
            //}
        }        
    }
}