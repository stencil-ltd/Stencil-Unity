﻿using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BuildScript {

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
    
    [MenuItem("Stencil/Build/Both")]
    public static void PerformBothBuild()
    {
#if UNITY_ANDROID
        Build(BuildTarget.Android);
        Build(BuildTarget.iOS);
#elif UNITY_IOS
        Build(BuildTarget.iOS);
        Build(BuildTarget.Android);
        #endif
    }

    public static void Build(BuildTarget target)
    {
        var levels = EditorBuildSettings.scenes.ToArray();
        var suffix = "";
        switch (target)
        {
            case BuildTarget.Android:
                suffix = ".apk";
                break;
            default:
                suffix = "";
                break;
        }

        var path = $"Builds/{target}{suffix}";
        var dir = $"{Application.dataPath}/../";
        var abspath = dir + path;
        if (File.Exists(abspath))
        {
            var bk = $"{abspath}.bk";
            if (File.Exists(bk))
                File.Delete(bk);
            File.Move(abspath, bk);
        }
        var dev = EditorUserBuildSettings.development ? BuildOptions.Development : BuildOptions.None;
        BuildPipeline.BuildPlayer(levels, path, target, dev);
    }
}
