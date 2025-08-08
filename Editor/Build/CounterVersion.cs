using System;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace LK.Editor.Build
{
    public class CounterVersion : IPreprocessBuildWithReport
    {
        public int callbackOrder { get; }
        
        public void OnPreprocessBuild(BuildReport report)
        {
            try
            {
                var versionText = PlayerSettings.bundleVersion;
                var split = versionText.Split('.');
                var major = int.Parse(split[0]);
                var minor = int.Parse(split[1]);
                var patch = int.Parse(split[2]);
                var buildNumber = int.Parse(split[3]);
                buildNumber++;
                var newVersionText = $"{major}.{minor}.{patch}.{buildNumber}";
                PlayerSettings.bundleVersion = newVersionText;
                Debug.Log($"CounterVersion : {newVersionText}");
            }
            catch (Exception)
            {
                throw new BuildFailedException("The version number is in the wrong format.");
            }
        }
    }
}
