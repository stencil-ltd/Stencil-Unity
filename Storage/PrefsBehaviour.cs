using UnityEngine;

namespace Storage
{
    public class PrefsBehaviour : MonoBehaviour
    {
        private void OnApplicationQuit()
        {
            Debug.Log("Quit Detected. Saving...");
            Prefs.SaveAll();       
        }
    }
}