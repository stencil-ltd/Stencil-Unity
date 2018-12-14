using System;
using Standard.States;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Standard.Ftui
{
    [Obsolete]
    public class FtuiController : Controller<FtuiController>
    {
        private static int _ftuiIndex = 0;
        
        public GameObject[] Ftuis;
        public bool RandomizeSequence = false; // alternate by default.

        private void OnEnable()
        {
            int index;
            if (RandomizeSequence)
                index = Random.Range(0, Ftuis.Length);
            else
            {
                index = _ftuiIndex++ % Ftuis.Length;
            }
            for (var i = 0; i < Ftuis.Length; i++)
                Ftuis[i].SetActive(i == index);
        }

        private void OnDisable()
        {
            PlayStates.Instance.RequestState(PlayStates.State.Playing);
        }

        public static void Show()
        {
            PlayStates.Instance.RequestState(PlayStates.State.Loading);
        }
    }
}