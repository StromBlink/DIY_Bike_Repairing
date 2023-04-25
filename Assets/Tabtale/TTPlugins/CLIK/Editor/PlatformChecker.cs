using UnityEditor;
using UnityEditor.Build;

public class PlatformChecker : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPreprocessBuild(UnityEditor.Build.Reporting.BuildReport report)
    {
        if (!CLIKConfiguration.IsPlatformSpecified())
        {
            EditorUtility.DisplayDialog(
                "Error",
                "You have built an app with CLIK but did not load a configuration ZIP. Please load a ZIP file through CLIK → Load Configuration….",
                "Got it");
        }
        else if (CLIKConfiguration.IsInvalidPlatform())
        {
            EditorUtility.DisplayDialog(
                "Error",
                "You have started a build for your game in platform: " + (CLIKConfiguration.IsAndroid() ? "Android" : "iOS") +
                ", but the configuration you have loaded is meant for " + (CLIKConfiguration.IsAndroid() ? "iOS" : "Android") + 
                ". While the build might pass and run, publishing functions will misbehave. Please load the correct zip file for this platform",
                "Got it");
        }
    }
}
