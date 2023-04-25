#if TTP_SHARE
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Reflection;

namespace Tabtale.TTPlugins
{
    public class TTPShare
    {
        private const string SS_NAME = "screenshot.png";
        private const string IMAGE_BASE64_TAG = "data:image/png;base64,";

        private static IEnumerator ShareRoutine(string ssPath)
        {

            while (!File.Exists(ssPath))
            {
                yield return new WaitForSeconds(.05f);
            }

            NativeShare.Share(null, ssPath, null, null, "image/png", true, "Select App To Share With:");
        }

        public static void ShareScreenshot()
        {
            Debug.Log(TTPLogger.LOGLabel + "TTPShare::ShareScreenshot:");
            String screenShotPath = Application.persistentDataPath + "/" + SS_NAME;
            Debug.Log("TTPShare::ShareScreenshot:screenShotPath=" + screenShotPath);
            if (File.Exists(screenShotPath)) File.Delete(screenShotPath);
#if UNITY_2017_1_OR_NEWER
            ScreenCapture.CaptureScreenshot(SS_NAME);
#else
            Application.CaptureScreenshot(SS_NAME);
#endif
            System.Reflection.MethodInfo method = typeof(TTPCore).GetMethod("GetTTPGameObject", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            if (method != null)
            {
                GameObject gameObject = method.Invoke(null, null) as GameObject;
                if (gameObject != null)
                {
                    MonoBehaviour mono = gameObject.GetComponent<MonoBehaviour>();
                    mono.StartCoroutine(ShareRoutine(screenShotPath));
                }
            }
        }

        public static void ShareVideo(string pathToVideo)
        {
            Debug.Log(TTPLogger.LOGLabel + "TTPShare::ShareVideo:pathToVideo=" + pathToVideo);
            NativeShare.Share(null, pathToVideo, null, null, "video/mp4", true, "Select App To Share With:");
        }

        public static void ShareImage(string pathToImage)
        {
            Debug.Log(TTPLogger.LOGLabel + "TTPShare::ShareImage:pathToImage=" + pathToImage);
            NativeShare.Share(null, pathToImage, null, null, "image/png", true, "Select App To Share With:");
        }

        public static void ShareImage(byte[] imageBytes)
        {
            Debug.Log(TTPLogger.LOGLabel + "TTPShare::ShareImage:imageBytes=" + imageBytes.Length);
            System.Reflection.MethodInfo method = typeof(TTPCore).GetMethod("GetTTPGameObject", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            if (method != null)
            {
                GameObject gameObject = method.Invoke(null, null) as GameObject;
                if (gameObject != null)
                {
                    MonoBehaviour mono = gameObject.GetComponent<MonoBehaviour>();
                    mono.StartCoroutine(ShareImageBytes(imageBytes));
                }
            }
        }

        public static void ShareLink(string link)
        {
            Debug.Log(TTPLogger.LOGLabel + "TTPShare::ShareLink:link=" + link);
            NativeShare.Share(null, null, link, null, "text/plain", true, "Select App To Share With:");
        }

        [Serializable]
        private class GlobalConfiguration
        {
            public string appId = "";
        }

        public static void ShareAppLink()
        {
            Debug.Log(TTPLogger.LOGLabel + "TTPShare::ShareAppLink:");
            String link = null;
#if UNITY_ANDROID
            link = "https://play.google.com/store/apps/details?id=" + TTPUtils.BundleIdentifier;
#else
            string configurationJson = ((TTPCore.ITTPCoreInternal)TTPCore.Impl).GetConfigurationJson("global");
            if (!string.IsNullOrEmpty(configurationJson))
            {
                GlobalConfiguration configuration = JsonUtilityWrapper.FromJson<GlobalConfiguration>(configurationJson);
                if (configuration != null)
                {
                    String appId = configuration.appId;
                    if (!string.IsNullOrEmpty(appId))
                    {
                        link = "https://itunes.apple.com/app/id" + appId;
                    }
                }
            }

#endif
            Debug.Log("TTPShare::ShareAppLink:link=" + link);
            if (link != null)
            {
                NativeShare.Share(null, null, link, null, "text/plain", true, "Select App To Share With:");
            }
        }

        public static void ShareFirebaseInstanceId()
        {
            TTPLogger.Log("TTPShare::ShareFirebaseInstanceId:");
            var analyticsClsType = System.Type.GetType("Tabtale.TTPlugins.TTPAnalytics");
            if (analyticsClsType != null)
            {
                var method = analyticsClsType.GetMethod("GetInstanceIdFirebase", BindingFlags.NonPublic | BindingFlags.Static);
                if (method != null)
                {
                    var id = (string)method.Invoke(null, null);
                    if (!string.IsNullOrEmpty(id))
                    {
                        NativeShare.ShareMultiple("My Firebase instance ID is - " + id, null, null, "My Firebase Instance ID",  "text/plain", true, "Select App To Share With:");
                        // NativeShare.Share("My Firebase instance ID is - " + id, null, null, "My Firebase Instance ID", "text/plain", true, "Select App To Share With:");
                    }
                }
                else
                {
                    Debug.LogWarning("ShareFirebaseInstanceId:: could not find method GetInstanceIdFirebase");
                }
            }
            else
            {
                Debug.LogWarning("ShareFirebaseInstanceId:: could not find TTPAnalytics class");
            }
        }

        private static IEnumerator ShareImageBytes(byte[] imageBytes)
        {
            string dst = Application.persistentDataPath + "/temp_share_image.png"; 
            Debug.Log("TTPShare::ShareImageBytes:Creating file " + dst);
            yield return dst;
            File.WriteAllBytes(dst, imageBytes);
            Debug.Log("TTPShare::ShareImageBytes:Finished writing to " + dst);
            if (File.Exists(dst))
            {
                TTPShare.ShareImage(dst);
            }
            else
            {
                Debug.Log("TTPShare::ShareImageBytes:Failed to find - " + dst);
            }
        }

    }
}
#endif