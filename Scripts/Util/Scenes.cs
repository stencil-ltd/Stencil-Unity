using UnityEngine.SceneManagement;

namespace Plugins.Util
{
    public static class Scenes
    {
        public static void Reload()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
    }
}