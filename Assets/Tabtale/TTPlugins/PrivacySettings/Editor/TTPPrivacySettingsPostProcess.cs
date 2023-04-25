using System.Collections;
using System.Collections.Generic;
using System.IO;
using Tabtale.TTPlugins;
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif
using UnityEngine;

public class TTPPrivacySettingsPostProcess
{
    private const string PATH_TO_INFO_PLIST_LANGS = "StreamingAssets/ttp/iOS";
    
    [PostProcessBuild(40006)]
    public static void OnPostProcessBuild(BuildTarget target, string path)
    {
        Debug.Log("TTPPrivacySettingsPostProcess::OnPostprocessBuild: path = " + path);
#if CRAZY_LABS_CLIK
        Debug.Log("TTPPrivacySettingsPostProcess::OnPostprocessBuild: in CLIK");
        var includedServicesPath = "Assets/Tabtale/TTPlugins/CLIK/Resources/ttpIncludedServices.asset";
        if (File.Exists(includedServicesPath))
        {
            Debug.Log("TTPPrivacySettingsPostProcess::OnPostprocessBuild: found included services asset");
            var includedServices = AssetDatabase.LoadAssetAtPath<TTPIncludedServicesScriptableObject>(includedServicesPath);
            if (!includedServices.privacySettings)
            {
                Debug.Log("TTPPrivacySettingsPostProcess::OnPostprocessBuild: privacy settings not included");
                return;
            }
        }
#endif

#if UNITY_IOS
        string attText = "Your device data will only be used to show you relevant ads";
        var additionalConfigPath =
            "Assets/StreamingAssets/ttp/configurations/additionalConfig.json";
        if (File.Exists(additionalConfigPath))
        {
            var json = File.ReadAllText(additionalConfigPath);
            if (!string.IsNullOrEmpty(json))
            {
                var dic = TTPJson.Deserialize(json) as Dictionary<string, object>;
                if (dic != null)
                {
                    if (dic.ContainsKey("attText") && dic["attText"] is string)
                    {
                        attText = dic["attText"] as string;
                    }
                }
            }
        }
        
        if (target == BuildTarget.iOS)
        {
            NativeLocale.AddLocalizedStringsIOS(path, Path.Combine(Application.dataPath, PATH_TO_INFO_PLIST_LANGS));
        }

        var pbxProjectPath = UnityEditor.iOS.Xcode.PBXProject.GetPBXProjectPath(path);
        var pbxProject = new UnityEditor.iOS.Xcode.PBXProject();
        pbxProject.ReadFromString(System.IO.File.ReadAllText(pbxProjectPath));
        var plistPath = Path.Combine(path, "Info.plist");
        var plist = new PlistDocument();
        plist.ReadFromFile(plistPath);
        var rootDict = plist.root;
        rootDict.SetString("NSUserTrackingUsageDescription", attText);

        File.WriteAllText(plistPath, plist.WriteToString());
#endif
    }
}
