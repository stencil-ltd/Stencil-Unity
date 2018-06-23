using UnityEditor;

namespace SplashScreen
{
    [InitializeOnLoad]
    public class SplashScreenFuckOff {
        
        static SplashScreenFuckOff()
        {
            PlayerSettings.SplashScreen.showUnityLogo = false;
        }
    }
}
