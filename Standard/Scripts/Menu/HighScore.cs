using UnityEngine;
using UnityEngine.UI;

namespace Standard.Menu
{
    public class HighScore : MonoBehaviour
    {
        public static int? Score
        {
            get
            {
                var retval = PlayerPrefs.GetInt("stencil_high_score", -1);
                return retval == -1 ? null : (int?) retval;
            }
            private set
            {
                PlayerPrefs.SetInt("stencil_high_score", value ??  -1);
                PlayerPrefs.Save();
            }
        }

        public static bool GotScore(int score)
        {
            var current = Score ?? 0;
            if (current > score) return false;
            Score = score;
            return true;
        }
        
        public Text Text;

        private void OnEnable()
        {
            Text.text = $"{Score}";
        }

        public void Click_Camera()
        {
            // TODO
        }
    }
}