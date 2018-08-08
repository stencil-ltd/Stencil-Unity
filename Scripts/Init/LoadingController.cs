using System;
using System.Collections;
using Ads;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Init
{
    public class LoadingController : MonoBehaviour
    {
        public float LoadTime = 2f;
        public ScriptableObject[] Scriptables;
        private AsyncOperation _scene;

        private void Awake()
        {
            StencilAds.InitHouse();
            StencilAds.House.OnError += OnError;
            StencilAds.House.OnComplete += OnComplete;
        }

        private IEnumerator Start()
        {
            yield return null;
            _scene = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
            _scene.allowSceneActivation = false;
        
            yield return new WaitForSeconds(LoadTime);
            if (StencilAds.House.IsReady)
            {
                Debug.Log("Found House Ad. Showing.");
                StencilAds.House.Show();
            }
            else
            {
                Debug.Log("No House Ads. Moving On.");
                Continue();
            }
        }

        private void OnDestroy()
        {
            StencilAds.House.OnError -= OnError;
            StencilAds.House.OnComplete -= OnComplete;
        }

        private void Continue()
        {
            _scene.allowSceneActivation = true;
            if (_scene.isDone)
                SceneManager.UnloadSceneAsync("Startup");
            else
                _scene.completed += operation => SceneManager.UnloadSceneAsync("Startup");
        }

        private void OnComplete(object sender, EventArgs e)
        {
            Debug.Log("Loading Game...");
            Continue();
        }

        private void OnError(object sender, EventArgs e)
        {
//        Continue();
        }
    }
}