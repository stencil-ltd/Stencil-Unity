using PaperPlaneTools;
using Runner.Challenges;
using Runner.Progression;
using Runner.Scoring;
using Standard.States;
using UI;
using UnityEngine;

namespace Standard.Menu
{
    public class MainMenu : Controller<MainMenu>
    {
        public ScoreTrackMeter Track;
        public OptionsMenu Options;

        private void OnEnable()
        {
            var score = ScoreController.Instance.LifetimeScore;
            ChallengeMeta meta;
            var challenge = ChallengeController.Instance.GetChallenge((int) score, out meta);
            Track.ResetAmounts(meta.ScoreThisChallenge, challenge.Score, meta.Index);
        }

        public void Click_Play()
        {
            PlayStates.Instance.RequestState(PlayStates.State.Loading);
        }

        public void Click_Rating()
        {
            RateBox.Instance.GoToRateUrl();
        }

        public void Click_Social()
        {
            Application.OpenURL("https://www.instagram.com/stencil.ltd");
        }

        public void Click_Options()
        {
            Options.gameObject.SetActive(true);
        }
    }
}