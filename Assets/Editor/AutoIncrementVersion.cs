using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class AutoIncrementVersion : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        // Increment version code
        PlayerSettings.Android.bundleVersionCode++;

        // Optionally, update version name (if needed)
        var versionParts = PlayerSettings.bundleVersion.Split('.');
        int major = int.Parse(versionParts[0]);
        int minor = int.Parse(versionParts[1]);
        int build = int.Parse(versionParts[2]) + 1;
        PlayerSettings.bundleVersion = $"{major}.{minor}.{build}";

        Debug.Log($"Auto-incremented versionCode to {PlayerSettings.Android.bundleVersionCode}");
        Debug.Log($"Updated versionName to {PlayerSettings.bundleVersion}");
    }
}