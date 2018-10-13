using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildScript : IPreprocessBuildWithReport {

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
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.X86 | AndroidArchitecture.ARMv7;
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        PerformAndroidBuild();
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, backend);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
    }
    
    [MenuItem("Stencil/Production/iOS")]
    public static void ProductionIphone()
    {
        PlayerSettings.iOS.buildNumber = "" + (int.Parse(PlayerSettings.iOS.buildNumber) + 1);
        PerformiOSBuild();
    }

    [MenuItem("Stencil/Production/Bump Versions")]
    public static void BumpVersions()
    {
        PlayerSettings.Android.bundleVersionCode++;
        PlayerSettings.iOS.buildNumber = "" + (int.Parse(PlayerSettings.iOS.buildNumber) + 1);
        WriteVersionCodes();
    }

    [MenuItem("Stencil/Production/Write Version")]
    public static void WriteVersionCodes()
    {
        Debug.Log("Writing Version Codes");
        if (!Directory.Exists("Assets/Resources"))
            Directory.CreateDirectory("Assets/Resources");
        
        var path = "Assets/Resources/VersionCodes.json";
        var writer = new StreamWriter(path, false);
        var json = new VersionJson
        {
            android = new VersionPlatform
            {
                versionCode = PlayerSettings.Android.bundleVersionCode,
                versionString = PlayerSettings.bundleVersion
            },
            ios = new VersionPlatform
            {
                versionCode = int.Parse(PlayerSettings.iOS.buildNumber),
                versionString = PlayerSettings.bundleVersion
            }
        };
        
        writer.Write(JsonUtility.ToJson(json));
        writer.Close();
        AssetDatabase.ImportAsset(path); 
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

    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        WriteVersionCodes();
    }
    
    [Serializable]
    public class VersionPlatform
    {
        public int versionCode;
        public string versionString;
    }

    [Serializable]
    public class VersionJson
    {
        public VersionPlatform android;
        public VersionPlatform ios;
    }
}
