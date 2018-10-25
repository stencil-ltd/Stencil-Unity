using Binding;
using Particles;
using Physic;
using UnityEngine;
using UnityEngine.UI;

namespace Store
{
    [RequireComponent(typeof(BuyableListing))]
    public class BuyableListing3D : MonoBehaviour
    {
        public MeshFilter Filter;
        public MeshRenderer Mesh;
        public ConstantRotation Rotation;

        [Bind] 
        private BuyableListing _listing;

        private void Awake()
        {
            this.Bind();
            _listing.OnUpdateBuyable.AddListener(OnUpdateBuyable);
        }
        
        private void OnUpdateBuyable(Buyable arg0)
        {
            Filter.mesh = arg0.Mesh;
            Mesh.material.mainTexture = arg0.Texture;
            Rotation.enabled = arg0.Equipped;
            var scale = new Vector3(1, 1, 1);
            if (!arg0.Equipped)
                scale *= 0.8f;
            Mesh.transform.localScale = scale;
        }
    }
}