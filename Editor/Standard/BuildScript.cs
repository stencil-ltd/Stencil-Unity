using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BuildScript {

    [MenuItem("Stencil/Production/Both")]
    public static void ProductionBuild()
    {
#if UNITY_ANDROID
        ProductionAndroid();
        ProductionIphone();
#elif UNITY_IOS
        ProductionIphone();
        ProductionAndroid();
#endif
    }

    [MenuItem("Stencil/Production/Android")]
    public static void ProductionAndroid()
    {
        PlayerSettings.Android.bundleVersionCode++;
        var backend = PlayerSettings.GetScriptingBackend(BuildTargetGroup.Android);
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        PerformAndroidBuild();
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, backend);
    }
    
    [MenuItem("Stencil/Production/iOS")]
    public static void ProductionIphone()
    {
        PlayerSettings.iOS.buildNumber = "" + (int.Parse(PlayerSettings.iOS.buildNumber) + 1);
        PerformiOSBuild();
    }
    
    public static void PerformAndroidBuild()
    { 
        Build(BuildTarget.Android);
    }

    public static void PerformiOSBuild()
    {
        Build(BuildTarget.iOS);
    }

    public static void Build(BuildTarget target)
    {
        var levels = EditorBuildSettings.scenes.ToArray();
        var path = $"Builds/{target}";
        var dir = $"{Application.dataPath}/../";
        var abspath = dir + path;
        if (Directory.Exists(abspath))
            Directory.Delete(abspath, true);
//        var dev = EditorUserBuildSettings.development ? BuildOptions.Development : BuildOptions.None;
        var dev = BuildOptions.None;
        BuildPipeline.BuildPlayer(levels, path, target, dev);
    }
}
