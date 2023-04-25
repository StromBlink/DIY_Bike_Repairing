using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEditor.PackageManager.Requests;
using UnityEditor.PackageManager;
//1. TTP Billing package is installed - inside there are 2 unity packages - Unity IAP and Google Plugins
//	both defines are false
//2. Check unity version
//if 2018 or 2019.3 and down or 2020.1 - install legacy unity pacakge of Unity IAP
//else - install Unity IAP from package manager
//3. Check platform
//if android AND legacy Unity IAP is installed - install Google Plugins - toggle TTP_GOOGLE_PLAY_PLUGINS
//4. Toggle UNITY_PURCHASING
[InitializeOnLoad]
public class InitIAP
{
    private const string UNITY_IAP_PACKAGE_ID = "com.unity.purchasing";
    private const string UNITY_IAP_VERSION_LEGACY = "4.0.3";
    private const string UNITY_IAP_VERSION = "4.6.0";

    private static string uipPath = "Assets/Tabtale/TTPlugins/TTPBilling/Packages/UnityIAP.unitypackage";
    private static string googlePath = "Assets/Tabtale/TTPlugins/TTPBilling/Packages/com.google.play.billing-3.2.0.unitypackage";
    private static string lagacyPackage = "com.unity.purchasing@2.0.6";
#if UNITY_2020_3_OR_NEWER
    private static string newPackage = UNITY_IAP_PACKAGE_ID + "@" + UNITY_IAP_VERSION;
#else
    private static string newPackage = UNITY_IAP_PACKAGE_ID + "@" + UNITY_IAP_VERSION_LEGACY;
#endif
    private static string purchasingSymbol = "UNITY_PURCHASING";
    private static string uiapSymbol = "UIAP_INSTALLED";
    private static string googleSymbol = "TTP_GOOGLE_PLAY_PLUGINS";
    private static string androidDuplicationFilePath = "Assets/Plugins/UnityPurchasing/Bin/Android/billing-3.0.3.aar";
    static AddRequest addRequest;
    static RemoveRequest removeRequest;
    static ListRequest listRequest;

    static InitIAP()
    {
#if !UIAP_INSTALLED
        if (NeedLegacyPackage())
        {
            AssetDatabase.importPackageStarted += AssetDatabaseOnimportPackageStarted;
            AssetDatabase.importPackageCancelled += AssetDatabaseOnimportPackageCancelled;
            AssetDatabase.importPackageFailed += AssetDatabaseOnimportPackageFailed;
            AssetDatabase.importPackageCompleted += AssetDatabaseOnimportPackageCompleted;
        }
#endif
    }

    [MenuItem("CLIK/Validate Unity IAP Installation")]
    public static void ValidateUnityIAP()
    {
        listRequest = Client.List();
        EditorApplication.update += ListPackagesUpdate;
    }

    //[MenuItem("CLIK/Install Unity IAP")]
    public static void InstallUnityIAP()
    {
        if (NeedLegacyPackage())
        {
            InstallUnityIAPPackage();
        }
        else
        {
            InstallUnityIAPWithPackageManager();
        }
    }


    [MenuItem("CLIK/Uninstall Unity IAP")]
    public static void UninstallUnityIAPPackage()
    {
        removeRequest = Client.Remove("com.unity.purchasing");
        EditorApplication.update += RemovePackageUpdate;
    }
    
    static bool NeedLegacyPackage()
    {
#if UNITY_2019_4_OR_NEWER && !UNITY_2020_1
        return false;
#else
        return true;
#endif
    }

    static bool IsAndroidPlatform()
    {
#if UNITY_ANDROID
        return true;
#else 
        return false;
#endif
    }

    static void InstallUnityIAPPackage() {
        if (File.Exists(uipPath))
        {
            Debug.Log("InitIAP:: InstallUnityIAPPackage: Legacy Unity IAP is installing");
            AssetDatabase.ImportPackage(uipPath, false);
        }
        else
        {
            Debug.Log("InitIAP:: InstallUnityIAPPackage: Legacy Unity IAP does not exist at " + uipPath);
            ExitApplication(1);
        }
    }
    
    static void InstallGooglePluginsPackage() {
        if (File.Exists(googlePath))
        {
            Debug.Log("InitIAP:: InstallGooglePluginsPackage: Legacy Google Plugin is installing");
            File.Delete(androidDuplicationFilePath);
            AssetDatabase.ImportPackage(googlePath, false);
        }
        else
        {
            Debug.Log("InitIAP:: InstallGooglePluginsPackage: Legacy Google Plugin does not exist at " + googlePath);
            ExitApplication(1);
        }
    }

    private static void AssetDatabaseOnimportPackageStarted(string packagename)
    {
        Debug.Log("InitIAP:: AssetDatabaseOnimportPackageStarted: " + packagename);
    }

    private static void AssetDatabaseOnimportPackageFailed(string packagename, string errormessage)
    {
        Debug.Log("InitIAP:: AssetDatabaseOnimportPackageFailed: " + packagename + " errormessage: " + errormessage);
        ExitApplication(1);
    }

    private static void AssetDatabaseOnimportPackageCancelled(string packagename)
    {
        Debug.Log("InitIAP:: AssetDatabaseOnimportPackageCancelled: " + packagename);
        ExitApplication(1);
    }

    private static void AssetDatabaseOnimportPackageCompleted(string packagename)
    {
        Debug.Log("InitIAP:: AssetDatabaseOnimportPackageCompleted: " + packagename);
        if (IsAndroidPlatform() && uipPath.Contains(packagename))
        {
            Debug.Log("InitIAP:: AssetDatabaseOnimportPackageCompleted: Start InstallGooglePluginsPackage");
            InstallGooglePluginsPackage();
        }
        else  if (!IsAndroidPlatform() || googlePath.Contains(packagename))
        {
            Debug.Log("InitIAP:: AssetDatabaseOnimportPackageCompleted: Start InstallLegacyUnityIAPWithPackageManager");
            InstallLegacyUnityIAPWithPackageManager();
        }
        else
        {
            Debug.Log("InitIAP:: AssetDatabaseOnimportPackageCompleted: " + packagename + " Error: unknown package");
            ExitApplication(1);
        }
    }
    
    private static void GoogleAssetDatabaseOnimportPackageCompleted(string packagename)
    {
        Debug.Log("InitIAP:: GoogleAssetDatabaseOnimportPackageCompleted: " + packagename);
        if (IsAndroidPlatform() && packagename.Contains(googlePath))
        {
            AddPurchasingSymbols(new List<string> { googleSymbol } );
        }
    }
    
    static void InstallLegacyUnityIAPWithPackageManager()
    {
        addRequest = Client.Add(lagacyPackage);
        EditorApplication.update += ProgressLegacyInstallation;
    }

    static void InstallUnityIAPWithPackageManager()
    {
        addRequest = Client.Add(newPackage);
        EditorApplication.update += ProgressInstallation;
    }
    
    static void ProgressLegacyInstallation()
    {
        if (addRequest.IsCompleted)
        {
            if (addRequest.Status == StatusCode.Success)
            {
                Debug.Log("InitIAP:: ProgressLegacyInstallation: Installed " + addRequest.Result.packageId);
                GoogleAssetDatabaseOnimportPackageCompleted(addRequest.Result.packageId);
                AddPurchasingSymbols(new List<string> { uiapSymbol } );
            }
            else if (addRequest.Status >= StatusCode.Failure)
            {
                Debug.Log("InitIAP:: ProgressLegacyInstallation: Failed " + addRequest.Error.message);
                ExitApplication(1);
            }
            EditorApplication.update -= ProgressLegacyInstallation;
            ExitApplication(0);
        }
    }
    static void ProgressInstallation()
    {
        if(addRequest != null)
        {
            if (addRequest.Status == StatusCode.InProgress)
            {
                Debug.Log("InitIAP:: ProgressInstallation: In Progress");
            }
            if (addRequest.IsCompleted)
            {
                if (addRequest.Status == StatusCode.Success)
                {
                    Debug.Log("InitIAP:: ProgressInstallation: Installed " + addRequest.Result.packageId);
                    AddPurchasingSymbols(new List<string> { uiapSymbol, purchasingSymbol });
                }
                else if (addRequest.Status >= StatusCode.Failure)
                {
                    Debug.Log("InitIAP:: ProgressInstallation: Failed " + addRequest.Error.message);
                    ExitApplication(1);
                }
                EditorApplication.update -= ProgressInstallation;
                addRequest = null;
                ExitApplication(0);
            }
        }
        
    }

    static void RemovePackageUpdate()
    {
        if (removeRequest != null && removeRequest.IsCompleted)
        {
            Debug.Log("InitIAP:: RemovePackageUpdate: Unity IAP Removed Result = " + removeRequest.Status + " Error = " +
                (removeRequest.Error != null ? removeRequest.Error.message : "null"));
            RemovePurchasingSymbols(new List<string> { uiapSymbol, purchasingSymbol, googleSymbol });
            EditorApplication.update -= RemovePackageUpdate;
            removeRequest = null;
        }
    }

    static void ListPackagesUpdate()
    {
        if (listRequest != null && listRequest.IsCompleted)
        {
            if (listRequest.Status == StatusCode.Success)
            {
                string currVersion = null;
                foreach (var package in listRequest.Result)
                {
                    if(package.name == UNITY_IAP_PACKAGE_ID)
                    {
                        currVersion = package.version;
                    }
                }
                if (currVersion != UNITY_IAP_VERSION)
                {
                    DisplayUnityIAPDialog(currVersion);
                }
                else
                {
                    AddPurchasingSymbols(new List<string> { uiapSymbol, purchasingSymbol });
                    DisaplyUnityIAPInstalledDialog();
                }
            }
            else
            {
                Debug.LogError("InitIAP:: ListPackagesUpdate: failed to list upm packages. message - " +
                    (listRequest.Error != null ? listRequest.Error.message : "null"));
            }
            listRequest = null;
            EditorApplication.update -= ListPackagesUpdate;
        }
    }

    static void DisaplyUnityIAPInstalledDialog()
    {
        EditorUtility.DisplayDialog("CLIK Billing - Unity IAP", "Unity IAP is installed correctly. Yay :)", "Close");
    }

    static void DisplayUnityIAPDialog(string version)
    {
        if(EditorUtility.DisplayDialog("CLIK Billing - Unity IAP", ("Unity IAP " +
            (version != null ? "is installed but with version " + version : "is not installed") + "\n " +
			"For CLIK to work correctly, you will need to install Unity IAP correctly."),
                "Install", "Cancel"))
        {
            InstallUnityIAPWithPackageManager();
        }
        else
        {
            Debug.LogWarning("You have chosen to opt out of installing Unity IAP. CLIK Billing will not work without it.");
        }
    }

    static void AddPurchasingSymbols(List<string> symbols)
    {
        AddPurchasingSymbolsForPlatform(symbols, EditorUserBuildSettings.selectedBuildTargetGroup);
        AddPurchasingSymbolsForPlatform(symbols, BuildTargetGroup.Android);
        AddPurchasingSymbolsForPlatform(symbols, BuildTargetGroup.iOS);
    }

    static void RemovePurchasingSymbols(List<string> symbols)
    {
        RemovePurchasingSymbolsForPlatform(symbols, EditorUserBuildSettings.selectedBuildTargetGroup);
        RemovePurchasingSymbolsForPlatform(symbols, BuildTargetGroup.Android);
        RemovePurchasingSymbolsForPlatform(symbols, BuildTargetGroup.iOS);
    }

    static void AddPurchasingSymbolsForPlatform(List<string> symbols, BuildTargetGroup buildTargetGroup)
    {
        string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
        List<string> allDefines = definesString.Split(';').ToList();
        allDefines.AddRange(symbols.Except(allDefines));
        PlayerSettings.SetScriptingDefineSymbolsForGroup(
            buildTargetGroup,
            string.Join(";", allDefines.ToArray()));
    }

    static void RemovePurchasingSymbolsForPlatform(List<string> symbols, BuildTargetGroup buildTargetGroup)
    {
        string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
        List<string> allDefines = definesString.Split(';').ToList();
        List<string> removedDefines = new List<string>();
        foreach(var s in allDefines)
        {
            if (!symbols.Contains(s))
            {
                removedDefines.Add(s);
            }
        }
        PlayerSettings.SetScriptingDefineSymbolsForGroup(
            buildTargetGroup,
            string.Join(";", removedDefines.ToArray()));
    }

    static void ExitApplication(int code)
    {
        if (Application.isBatchMode)
        {
            EditorApplication.Exit(code);
        }
    }
}
