﻿using System.Linq;
using UnityEditor;

namespace Standard
{
    public class BuildScript {

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
            BuildPipeline.BuildPlayer(levels, path, target, BuildOptions.None);
        }
    }
}
