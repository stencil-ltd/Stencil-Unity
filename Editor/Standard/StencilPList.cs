using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.IO;

public class StencilPlist
{
    [PostProcessBuild]
    public static void ChangeXcodePlist(BuildTarget buildTarget, string pathToBuiltProject) {

        if (buildTarget == BuildTarget.iOS) {
   
            // Get plist
            string plistPath = pathToBuiltProject + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));
   
            // Get root
            PlistElementDict rootDict = plist.root;
   
            rootDict.SetBoolean("ITSAppUsesNonExemptEncryption", false);
            rootDict.SetString("NSCameraUsageDescription", "This app wants to save a photo to your device.");
            rootDict.SetString("NSPhotoLibraryUsageDescription", "This app wants to save a photo to your device.");
            rootDict.SetString("NSPhotoLibraryAddUsageDescription", "This app wants to save a photo to your device.");
   
            // Write to file
            File.WriteAllText(plistPath, plist.WriteToString());
        }
    }
}