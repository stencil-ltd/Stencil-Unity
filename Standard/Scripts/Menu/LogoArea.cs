using UnityEngine;

namespace Standard.Menu
{
    public class LogoArea : MonoBehaviour
    {
        public GameObject Logo;
        public HighScore HighScore;

        private void Awake()
        {
            Logo.SetActive(HighScore.Score == null);
            HighScore.gameObject.SetActive(!Logo.activeSelf);
        }
    }
}