using UnityEngine;

namespace Dev
{
    public static class Developers
    {
        public static bool Enabled => ForceEnabled || Application.isEditor || Debug.isDebugBuild;
        public static bool ForceEnabled = false;
    }
}