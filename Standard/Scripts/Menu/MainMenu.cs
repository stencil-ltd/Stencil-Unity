using System;
using PaperPlaneTools;
using Plugins.UI;
using Standard.States;
using States;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace Menu
{
    public class MainMenu : Controller<MainMenu>
    {
        public OptionsMenu Options;

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