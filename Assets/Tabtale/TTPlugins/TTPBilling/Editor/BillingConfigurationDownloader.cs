#if !CRAZY_LABS_CLIK
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Tabtale.TTPlugins
{

    [InitializeOnLoad]
    public class BillingConfigurationDownloader
    {
        private const string BILLING_URL_ADDITION = "/billing/";
        private const string BILLING_JSON_FN = "billing";

        static BillingConfigurationDownloader()
        {
            TTPMenu.OnDownloadConfigurationCommand += DownloadConfiguration;
        }

        private static void DownloadConfiguration(string domain)
        {
            string store = "google";
            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
            {
                store = "apple";
            }
            string url = domain + BILLING_URL_ADDITION + store + "/" + PlayerSettings.applicationIdentifier;
            bool result = TTPMenu.DownloadConfiguration(url, BILLING_JSON_FN);
            if (!result)
            {
                Debug.LogWarning("BillingConfigurationDownloader:: DownloadConfiguration: failed to download configuration for privacy settings.");
            }
            
        }
    }
}
#endif