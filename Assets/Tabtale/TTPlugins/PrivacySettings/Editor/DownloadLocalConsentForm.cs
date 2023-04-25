using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEditor.Build;
using UnityEngine.Networking;

namespace Tabtale.TTPlugins
{
    public class DownloadLocalConsentForm
    {
        #if UNITY_ANDROID
        private const string CONFIGURATIONS_PATH = "/Plugins/Android/assets/ttp/configurations";
#else
        private const string CONFIGURATIONS_PATH = "/StreamingAssets/ttp/configurations";
#endif
        
        
        private const string STREAMING_ASSETS_PATH_JSON = CONFIGURATIONS_PATH + "/privacySettings.json";
        private const string STREAMING_ASSETS_PATH_CONSENT_ANDROID = "/Plugins/Android/assets/consentForm";
        private const string STREAMING_ASSETS_PATH_PRIVACY_ANDROID = "/Plugins/Android/assets/privacyForm";
        private const string STREAMING_ASSETS_PATH_CONSENT = "/StreamingAssets/consentForm";
        private const string STREAMING_ASSETS_PATH_PRIVACY = "/StreamingAssets/privacyForm";
        private const string STREAMING_ASSETS_PATH_CONSENT_ZIP = "/StreamingAssets/consentForm.zip";
        private const string STREAMING_ASSETS_PATH_PRIVACY_ZIP = "/StreamingAssets/privacyForm.zip";
        private const string STREAMING_ASSETS_PATH_HTML = "/StreamingAssets/index.html";

        [System.Serializable]
        private class PrivacySettingsConfiguration
        {
            public string consentFormURL = null;
            public string privacySettingsURL = null;
        }

        public void DownloadLocalConsentForms(BuildTarget target)
        {
            Debug.Log("DownloadLocalConsentForm");
            string consentFormUrl = null;
            string privacyFormUrl = null;
            string jsonFp = Application.dataPath + STREAMING_ASSETS_PATH_JSON;
            if (File.Exists(jsonFp))
            {
                try
                {
                    string jsonStr = File.ReadAllText(jsonFp);
                    if (jsonStr != null)
                    {
                        PrivacySettingsConfiguration privacySettingsConfiguration = JsonUtilityWrapper.FromJson<PrivacySettingsConfiguration>(jsonStr);
                        if (privacySettingsConfiguration != null)
                        {
                            consentFormUrl = privacySettingsConfiguration.consentFormURL;
                            privacyFormUrl = privacySettingsConfiguration.privacySettingsURL;
                        }
                    }
                }
                catch (System.Exception e)
                {
                    Debug.Log("DownloadLocalConsentForm::OnPreprocessBuild: failed to parse json. exception - " + e.Message);
                }

            }
            if (string.IsNullOrEmpty(consentFormUrl) || string.IsNullOrEmpty(privacyFormUrl))
            {
                Debug.Log("DownloadLocalConsentForm:: consentFormUrl or privacySettingsURL do not exist - aborting.");
            }
            else
            {
                string consentDst = "";
                string privacyDst = "";
                if (target == BuildTarget.Android)
                {
                    consentDst = Application.dataPath + STREAMING_ASSETS_PATH_CONSENT_ANDROID;
                    privacyDst = Application.dataPath + STREAMING_ASSETS_PATH_PRIVACY_ANDROID;
                }
                else if (target == BuildTarget.iOS)
                {
                    consentDst = Application.dataPath + STREAMING_ASSETS_PATH_CONSENT;
                    privacyDst = Application.dataPath + STREAMING_ASSETS_PATH_PRIVACY;
                }
                DownloadConsentForms(consentFormUrl, Application.dataPath + STREAMING_ASSETS_PATH_CONSENT_ZIP, consentDst);
                DownloadConsentForms(privacyFormUrl, Application.dataPath + STREAMING_ASSETS_PATH_PRIVACY_ZIP, privacyDst);
            }
        }

        private void DownloadConsentForms(string url, string pathToZip, string unzipFolder)
        {
            Debug.Log("DownloadConsentForms:: url - " + url);
            
            string exceptionMessage = null;
            if (TTPEditorUtils.DownloadFile(url, pathToZip))
            {
                try
                {
                    string tmpExistingFolder = unzipFolder + "_tmp";
                    if (Directory.Exists(unzipFolder))
                    {
                        Directory.Move(unzipFolder, tmpExistingFolder);
                    }
                    Directory.CreateDirectory(unzipFolder);
                    ZipUtil.Unzip(pathToZip, unzipFolder);
                    if (!File.Exists(unzipFolder + "/index.html"))
                    {
                        DeleteDirectory(unzipFolder);
                        Directory.Move(tmpExistingFolder, unzipFolder);
                        exceptionMessage = "DownloadConsentForms:: Html for consent form not found. Something must have went wrong with the download process.";
                    }
                    File.Delete(pathToZip);
                    if (Directory.Exists(tmpExistingFolder))
                    {
                        DeleteDirectory(tmpExistingFolder);
                    }
                }
                catch (System.Exception e)
                {
                    exceptionMessage = "DownloadConsentForms:: Failed to write consent form zip to file system. Exception - " + e.Message;
                }
            }
            else
            {
                exceptionMessage = "DownloadConsentForms:: Failed to retrieve consent form from server";
            }
            
            if (exceptionMessage != null)
            {
                throw new System.Exception(exceptionMessage);
            }
            else
            {
                Debug.Log("DownloadConsentForms:: Successfully downloaded consent and privacy forms");
            }
        }
        
        private void DeleteDirectory(string target_dir)
        {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(target_dir, false);
        }
    }


}
