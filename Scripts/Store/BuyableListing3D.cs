using Binding;
using Particles;
using UnityEngine;
using UnityEngine.UI;

namespace Store
{
    [RequireComponent(typeof(BuyableListing))]
    public class BuyableListing3D : MonoBehaviour
    {
        public MeshFilter Filter;
        public MeshRenderer Mesh;

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
        }
    }
}