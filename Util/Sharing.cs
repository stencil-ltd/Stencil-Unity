using UnityEngine;

namespace Util
{
    public static class Sharing
    {
        private const string VideoMime = "video/mp4";
        
        public static void ShareVideo(string path, string body = "") => Share(path, VideoMime, body);

        private static void Share(string path, string mime, string body = "")
        {
            if (Application.isEditor)
                Debug.Log($"Share {path} ({mime})");
            else NativeShare.Share(body, path, mimeType: mime, chooser: true);
        }
    }
}