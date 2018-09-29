using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Versions
{
    public static class VersionCodes
    {
#if UNITY_IPHONE
        [DllImport ("__Internal")]
        private static extern int unityGetVersionCode();
#endif

        [Serializable]
        public class VersionJson
        {
            public int android;
            public int ios;

            public int platform
            {
                get
                {
                    if (Application.platform == RuntimePlatform.IPhonePlayer)
                        return ios;
                    return android;
                }
            }
        }

        private static VersionJson _json;
        private static bool _failed;

        public static int GetVersionCode()
        {
            if (_json == null && !_failed)
            {
                var res = Resources.Load<TextAsset>("VersionCodes").text;
                _json = JsonUtility.FromJson<VersionJson>(res);
                if (_json == null) _failed = true;
            }
            return _json?.platform ?? 0;
        }
        
        public static int AndroidVersionCode() {
            var contextCls = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            var context = contextCls.GetStatic<AndroidJavaObject>("currentActivity"); 
            var packageMngr = context.Call<AndroidJavaObject>("getPackageManager");
            var packageName = context.Call<string>("getPackageName");
            var packageInfo = packageMngr.Call<AndroidJavaObject>("getPackageInfo", packageName, 0);
            return packageInfo.Get<int>("versionCode");
        }
        
        public static string AndroidVersionName() {
            var contextCls = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            var context = contextCls.GetStatic<AndroidJavaObject>("currentActivity"); 
            var packageMngr = context.Call<AndroidJavaObject>("getPackageManager");
            var packageName = context.Call<string>("getPackageName");
            var packageInfo = packageMngr.Call<AndroidJavaObject>("getPackageInfo", packageName, 0);
            return packageInfo.Get<string>("versionName");
        }

        public static int IosVersionCode()
        {
#if UNITY_IPHONE
            return unityGetVersionCode();
#else
            return 0;
#endif
            
        }
    }
}