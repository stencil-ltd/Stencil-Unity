using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using System.IO;
using UnityEditor.iOS.Xcode;

[InitializeOnLoad]
public class EntitlementsPostProcess : ScriptableObject
{
    static EntitlementsPostProcess()
    {}

    public DefaultAsset m_entitlementsFile;

    [PostProcessBuild]
    public static void OnPostProcess(BuildTarget buildTarget, string buildPath)
    {
        if (buildTarget != BuildTarget.iOS)
        {
            return;
        }

        var dummy = ScriptableObject.CreateInstance<EntitlementsPostProcess>();
        var file = dummy.m_entitlementsFile;
        ScriptableObject.DestroyImmediate(dummy);
        if (file == null)
        {
            return;
        }

        var proj_path = PBXProject.GetPBXProjectPath(buildPath);
        var proj = new PBXProject();
        proj.ReadFromFile(proj_path);

        // target_name = "Unity-iPhone"
        var target_name = PBXProject.GetUnityTargetName();
        var target_guid = proj.TargetGuidByName(target_name);
        var src = AssetDatabase.GetAssetPath(file);
        var file_name = Path.GetFileName(src);
        var dst = buildPath + "/" + target_name + "/" + file_name;
        try
        {
            FileUtil.CopyFileOrDirectory(src, dst);
            proj.AddFile(target_name + "/" + file_name, file_name);
            proj.AddBuildProperty(target_guid, "CODE_SIGN_ENTITLEMENTS", target_name + "/" + file_name);
            proj.WriteToFile(proj_path);
        }
        catch (IOException e)
        {
            Debug.LogWarning($"Could not copy entitlements. Probably already exists. ({e})");
        }
    }
}