using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BuildScript {
    
    [MenuItem("Stencil/Build/Increment + Build")]
    public static void IncrementAndBuild()
    {
        IncrementVersion();
        PerformBothBuild();
    }
    
    [MenuItem("Stencil/Build/Increment Version")]
    public static void IncrementVersion()
    {
        PlayerSettings.Android.bundleVersionCode++;
        PlayerSettings.iOS.buildNumber = "" + (int.Parse(PlayerSettings.iOS.buildNumber) + 1);
    }
    
    [MenuItem("Stencil/Build/Both")]
    public static void PerformBothBuild()
    {
#if UNITY_ANDROID
        PerformAndroidBuild();
        PerformiOSBuild();
#elif UNITY_IOS
        PerformiOSBuild();
        PerformAndroidBuild();
#endif
    }
    
    [MenuItem("Stencil/Build/Android")]
    public static void PerformAndroidBuild()
    { 
        Build(BuildTarget.Android);
    }

    [MenuItem("Stencil/Build/iOS")]
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
        var dev = EditorUserBuildSettings.development ? BuildOptions.Development : BuildOptions.None;
        BuildPipeline.BuildPlayer(levels, path, target, dev);
    }
}
