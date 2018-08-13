using System;
using UnityEngine;
using UnityEngine.UI;

namespace Standard.Menu
{
    public class ScreenshotZone : MonoBehaviour
    {
        public Text ScoreText;
        public void SetScore(int score)
        {
            ScoreText.text = $"{score:N0}";
        }
    }
}