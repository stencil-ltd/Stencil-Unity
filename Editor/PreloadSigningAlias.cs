using UnityEditor;

[InitializeOnLoad]
public class PreloadSigningAlias
{ 
    static PreloadSigningAlias ()
    {
        PlayerSettings.Android.keystorePass = "dude pool";
        PlayerSettings.Android.keyaliasName = "stencil";
        PlayerSettings.Android.keyaliasPass = "dude pool";
    } 
}