
#if UNITY_IOS
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.IO;

namespace Share
{
    public class SharePhotoPlist
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
       
                rootDict.SetString("NSPhotoLibraryUsageDescription", "This app wants to save a photo to your device.");
                rootDict.SetString("NSPhotoLibraryAddUsageDescription", "This app wants to save a photo to your device.");
       
                // Write to file
                File.WriteAllText(plistPath, plist.WriteToString());
            }
        }
    }
}
#endif