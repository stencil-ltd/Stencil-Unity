using Binding;
using UnityEngine;
using UnityEngine.UI;

namespace Ratings
{
    public class RateStar : MonoBehaviour
    {
        public int Index { get; set; }
        public Image Fill;

        [Bind] public Button Button { get; private set; }

        private void Awake()
        {
            this.Bind();
        }
    }
}