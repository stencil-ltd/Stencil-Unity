using System;
using Binding;
using Plugins.Util;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Util;

namespace Plugins.Data
{
    [RequireComponent(typeof(Button))]
    public class ResetButton : MonoBehaviour
    {
        public static event EventHandler OnGlobalReset;
        public event EventHandler OnReset;
        
        [Bind] 
        private Button _button;
        
        private void Awake()
        {
            this.Bind();
            _button.onClick.AddListener(() =>
            {
                foreach (var obj in SceneManager.GetActiveScene().GetRootGameObjects())
                    obj.SetActive(false);
                
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();
                Storage.Prefs.ClearAll();
                OnReset?.Invoke();
                OnGlobalReset?.Invoke();
                Scenes.Reload();
            });
        }
    }
}