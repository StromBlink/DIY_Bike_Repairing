#if !CRAZY_LABS_CLIK
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Tabtale.TTPlugins;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEditor.Android;

public class TTPAndroidPostProcess: IPostGenerateGradleAndroidProject
{
    private const string ANDROID_NAME_ATTRIBUTE = "android:name";
    public int callbackOrder  {
        get { return 0; }
    }
    
    [PostProcessBuild(0)]
    private static void OnPostProcess(BuildTarget target, string pathToBuiltProject)
    {
        Debug.Log("TTPAndroidPostProcess::OnPostProcess:pathToBuiltProject=" + pathToBuiltProject);
        
        if (IsAndroidWebDebugEnabled())
        {
            ProcessWebDebug(pathToBuiltProject);
        }

        ProcessManifest(pathToBuiltProject);
    }

    private static void ProcessManifest(string pathToBuiltProject)
    {
        string pathToAndroidManifest = GetPathToManifest(pathToBuiltProject);
        Debug.Log("TTPAndroidPostProcess::ProcessManifest:pathToAndroidManifest=" + pathToAndroidManifest);
        if (File.Exists(pathToAndroidManifest))
        {
            Debug.Log("TTPAndroidPostProcess::ProcessManifest:Manifest exists");
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(pathToAndroidManifest);
            var node = xmlDocument.DocumentElement;
            XmlNode applicationNode = node.SelectSingleNode("application");
            bool saveManifest = false;
            foreach (XmlNode xmlNode in applicationNode.ChildNodes)
            {
                if (xmlNode.Name != "activity")
                {
                    continue;
                }
                XmlAttribute activityNameAttribute = xmlNode.Attributes[ANDROID_NAME_ATTRIBUTE];
                if (activityNameAttribute != null)
                {
                    string activityName = activityNameAttribute.Value;
                    if (activityName == "com.tabtale.ttplugins.ttpunity.TTPUnityMainActivity")
                    {
                        Debug.Log("TTPAndroidPostProcess::ProcessManifest:change TTPUnityMainActivity to Unity default activity");
                        ((XmlElement)xmlNode).SetAttribute(
                            ANDROID_NAME_ATTRIBUTE, "com.unity3d.player.UnityPlayerActivity");
                        saveManifest = true;
                    }
                    else if (activityName == "com.google.android.gms.ads.AdActivity")
                    {
                        Debug.Log("TTPAndroidPostProcess::ProcessManifest:remove " + activityName + " from manifest");
                        applicationNode.RemoveChild(xmlNode);
                        saveManifest = true;
                    }
                }
            }

            if (saveManifest)
            {
                xmlDocument.Save(pathToAndroidManifest);
            }
        }
    }
    
    private static void ProcessWebDebug(string pathToBuiltProject)
    {
        Debug.Log("TTPAndroidPostProcess::ProcessWebDebug:");
        AndroidWebDebug(pathToBuiltProject);
        CreateOrMergeNetworkSecurityXml(TTPUtils.CombinePaths(new List<string>() { pathToBuiltProject, "RootProject",  "app", "src", "main", "res"}));
#if UNITY_2019_3_OR_NEWER
        CreateOrMergeNetworkSecurityXml(TTPUtils.CombinePaths(new List<string>() { pathToBuiltProject, "RootProject", "app", "launcher",  "src", "main", "res"}));
        CreateOrMergeNetworkSecurityXml(TTPUtils.CombinePaths(new List<string>() { pathToBuiltProject, "RootProject", "app", "unityLibrary",  "src", "main", "res"}));
#endif
    }
    
    private static bool IsAndroidWebDebugEnabled()
    {
#if UNITY_ANDROID && !CRAZY_LABS_CLIK
        Debug.Log("TTPAndroidPostProcess::IsAndroidWebDebugEnabled 1");
        var pathToAdditionalConfig = CoreConfigurationDownloader.CONFIGURATIONS_PATH + "/additionalConfig.json";
        if (File.Exists(pathToAdditionalConfig))
        {
            Debug.Log("TTPAndroidPostProcess::IsAndroidWebDebugEnabled 2");
            var jsonStr = File.ReadAllText(pathToAdditionalConfig);
            if (!string.IsNullOrEmpty(jsonStr) && jsonStr.Trim() != "{}")
            {
                Debug.Log("TTPAndroidPostProcess::IsAndroidWebDebugEnabled 3");
                var dict = TTPJson.Deserialize(jsonStr) as Dictionary<string, object>;
                if (dict != null)
                {
                    Debug.Log("TTPAndroidPostProcess::IsAndroidWebDebugEnabled 4");
                    object val = null;
                    if (dict.ContainsKey("androidWebDebug") && dict.TryGetValue("androidWebDebug", out val) &&
                        val is bool && (bool) val)
                    {
                        Debug.Log("TTPAndroidPostProcess::IsAndroidWebDebugEnabled 5");
                        return true;
                    }
                }
            }
        }
#endif
        return false;
    }

    private static void AndroidWebDebug(string pathToBuiltProject)
    {
        string pathToAndroidManifest = GetPathToManifest(pathToBuiltProject);
        Debug.Log("TTPAndroidPostProcess::AndroidWebDebug:pathToAndroidManifest=" + pathToAndroidManifest);
        if (File.Exists(pathToAndroidManifest))
        {
            Debug.Log("TTPAndroidPostProcess::AndroidWebDebug:Manifest exists");
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(pathToAndroidManifest);
            var node = xmlDocument.DocumentElement;
            var applicationNode = node.SelectSingleNode("application");
            Debug.Log(applicationNode.Name);
            var netSecConfigAttr = xmlDocument.CreateAttribute("andorid", "networkSecurityConfig","http://schemas.android.com/apk/res/android");
            netSecConfigAttr.Value = "@xml/network_security_config";
            var andoridDebugAttr = xmlDocument.CreateAttribute("andorid", "debuggable","http://schemas.android.com/apk/res/android");
            andoridDebugAttr.Value = "true";
            if (applicationNode.Attributes == null) return;
            Debug.Log("TTPAndroidPostProcess::AndroidWebDebug:application node exists");
            applicationNode.Attributes.Append(netSecConfigAttr);
            applicationNode.Attributes.Append(andoridDebugAttr);
            xmlDocument.Save(pathToAndroidManifest);
        }
    }
    
    private static void CreateOrMergeNetworkSecurityXml(string pathToRes)
    {
        Debug.Log("TTPAndroidPostProcess::CreateOrMergeNetworkSecurityXml: " +pathToRes);
        if (!Directory.Exists(pathToRes))
        {
            Debug.Log("TTPAndroidPostProcess::CreateOrMergeNetworkSecurityXml: Ensured res folder");
            Directory.CreateDirectory(pathToRes);
        }
        var pathToXmlRes = Path.Combine(pathToRes, "xml");
        if (!Directory.Exists(pathToXmlRes))
        {
            Debug.Log("TTPAndroidPostProcess::CreateOrMergeNetworkSecurityXml: Creating xml folder in res");
            Directory.CreateDirectory(pathToXmlRes);
        }
        var pathToNetworkSecurityXml = Path.Combine(pathToXmlRes, "network_security_config.xml");
        var xmlDocument = new XmlDocument();
        if (File.Exists(pathToNetworkSecurityXml))
        {
            Debug.Log("TTPAndroidPostProcess::CreateOrMergeNetworkSecurityXml: Loading XML file");
            xmlDocument.Load(pathToNetworkSecurityXml);
        }
        else
        {
            Debug.Log("TTPAndroidPostProcess::CreateOrMergeNetworkSecurityXml: Creating XML file");
            var xmlDeclaration = xmlDocument.CreateXmlDeclaration( "1.0", "UTF-8", null );
            var root = xmlDocument.CreateElement("network-security-config");
            xmlDocument.AppendChild(root);
            xmlDocument.InsertBefore( xmlDeclaration, root );
        }
        var networkSecConfig = xmlDocument.DocumentElement;
        var debugOverridesNode = AddXmlElement(xmlDocument, "debug-overrides", networkSecConfig);
        var trustAnchors = AddXmlElement(xmlDocument, "trust-anchors", debugOverridesNode);
        var attr = xmlDocument.CreateAttribute("src");
        attr.Value = "user";
        var certificates = AddXmlElement(xmlDocument, "certificates", trustAnchors);
        if (certificates.Attributes != null)
        {
            certificates.Attributes.Append(attr);
        }
        Debug.Log("TTPAndroidPostProcess::CreateOrMergeNetworkSecurityXml: Saving XML file = " + xmlDocument.ToString());
        xmlDocument.Save(pathToNetworkSecurityXml);
    }

    private static XmlNode AddXmlElement(XmlDocument xmlDocument, string elementName, XmlNode parentElement)
    {
        XmlNode node = null;
        try
        {
            node = parentElement.SelectSingleNode(elementName);
        }
        catch (Exception e)
        {
            Debug.Log("TTPAndroidPostProcess::OnPostProcess: did not find element " + elementName);
            Console.WriteLine(e);
        }
        if (node == null)
        {
            node = xmlDocument.CreateElement(elementName);
            Debug.Log("TTPAndroidPostProcess::OnPostProcess: node = " + node + " parentElement = " + parentElement);
            parentElement.AppendChild(node);
        }

        return node;
    }

    private static string GetPathToManifest(string pathToBuiltProject)
    {
#if UNITY_2019_3_OR_NEWER
        var pathToAndroidManifest = TTPUtils.CombinePaths(new List<string>() { pathToBuiltProject, "RootProject", "app", "unityLibrary",  "src", "main", "AndroidManifest.xml"});
#else
        var pathToAndroidManifest = TTPUtils.CombinePaths(new List<string>() { pathToBuiltProject, "RootProject",  "app", "src", "main", "AndroidManifest.xml"});
#endif
        return pathToAndroidManifest;
    }
    
    public void OnPostGenerateGradleAndroidProject(string path)
    {
        Debug.Log("TTPAndroidPostProcess: Start change jpackagingOptions in build.gradle files");
        var buildGradleFilePaths = Directory.GetFiles(path + "/..", "build.gradle",SearchOption.AllDirectories);
        foreach(var buildGradleFilePath in buildGradleFilePaths)
        {
            var buildGradleContent = File.ReadAllText(buildGradleFilePath);
            var idxSection = buildGradleContent.IndexOf("packagingOptions");
            if (idxSection > 0)
            {
                var idxBraces = buildGradleContent.IndexOf("{", idxSection);
                if (idxBraces > 0)
                {
                    Debug.Log("TTPAndroidPostProcess: Change packagingOptions in " + buildGradleFilePath);
                    buildGradleContent = buildGradleContent.Insert(idxBraces + 1, "\n        exclude 'META-INF/DEPENDENCIES'");
                }
            }
            File.WriteAllText(buildGradleFilePath, buildGradleContent);
        }
        Debug.Log("TTPAndroidPostProcess: End change packagingOptions in build.gradle files");
    }
}
#endif