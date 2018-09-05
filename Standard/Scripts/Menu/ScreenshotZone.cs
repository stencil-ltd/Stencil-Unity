using System;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Standard.Menu
{
    public class ScreenshotZone : Controller<ScreenshotZone>
    {
        public Text ScoreText;
        public void SetScore(int score)
        {
            ScoreText.text = $"{score:N0}";
        }
    }
}