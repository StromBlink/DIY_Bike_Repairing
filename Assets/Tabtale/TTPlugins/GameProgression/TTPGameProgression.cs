#if TTP_GAMEPROGRESSION
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

namespace Tabtale.TTPlugins
{
    /// <summary>
    /// This class logs game events like tutorial steps, mission status, level up in analytycs service.
    /// There are two analytics supported DeltaDNA and Firebase. DeltaDNA is deprecated 
    /// </summary>
    public class TTPGameProgression
    {
        private static int currentLevel;
        private const string PLAYER_PREFS_GAME_PROGRESSION_LEVEL = "ttpGameProgressionLevel";
        private const string LEVEL_PARAMETER_KEY = "level";
        private const string LEVEL_COMPLETE_EVENT_NAME = "levelComplete";
        private static Dictionary<string, object> miniMissionParams = new Dictionary<string, object>();

        static void InternalInit()
        {
            currentLevel = PlayerPrefs.GetInt(PLAYER_PREFS_GAME_PROGRESSION_LEVEL, 0);
        }

        static void UpdateCurrentLevel(int level)
        {
            currentLevel = level;
            PlayerPrefs.SetInt(PLAYER_PREFS_GAME_PROGRESSION_LEVEL, level);
        }

        /// <summary>
        /// Log tutorial step
        /// </summary>
        /// <param name="isMandatory">Indicates that step is mandatory</param>
        /// <param name="tutorialStepID">Step ID</param>
        /// <param name="tutorialName">Tutorial string name</param>
        /// <param name="tutorialStepName">Step string name</param>
        /// <param name="additionalParams">Other additional parameters of current step</param>
        public static void TutorialStep(bool isMandatory, int tutorialStepID, string tutorialName, string tutorialStepName, Dictionary<string, object> additionalParams)
        {
            TTPLogger.Log("TTPGameProgression::TutorialStep:");
            CallAnalyticsByReflection("TutorialStep", new object[] { isMandatory, tutorialStepID, tutorialName, tutorialStepName, additionalParams });
        }

        /// <summary>
        /// Log moment when player reached main screen
        /// </summary>
        /// <param name="additionalParams">Other additional parameters</param>
        public static void ReachedMainScreen(Dictionary<string, object> additionalParams)
        {
            TTPLogger.Log("TTPGameProgression::ReachedMainScreen:");
            CallAnalyticsByReflection("ReachedMainScreen", new object[] { additionalParams });
        }

        /// <summary>
        /// Log mission started event
        /// </summary>
        /// <param name="id">Mission ID</param>
        /// <param name="name">Mission name</param>
        /// <param name="type">Mission type</param>
        /// <param name="missionStartedType">Type of mission startup</param>
        /// <param name="additionalParams">Other additional parameters</param>
        public static void MissionStarted(string id, string name, string type, string missionStartedType, Dictionary<string, object> additionalParams)
        {
            TTPLogger.Log("TTPGameProgression::MissionStarted:");
            DDNAEvents.MissionStarted(id, name, type, missionStartedType, additionalParams);
        }

        /// <summary>
        /// Log mission completed event
        /// </summary>
        /// <param name="additionalParams">Other additional parameters</param>
        public static void MissionComplete(Dictionary<string, object> additionalParams)
        {
            TTPLogger.Log("TTPGameProgression::MissionComplete:");
            DDNAEvents.MissionComplete(additionalParams);
        }
        
        public static void MiniLevelStarted(int miniMissionID, Dictionary<string, object> additionalParams)
        {
            Debug.Log(TTPLogger.LOGLabel + "TTPGameProgression::DDNAEvents:MiniLevelStarted used from GameProgression plugin");
            miniMissionParams = additionalParams;
            miniMissionParams["miniMissionID"] = miniMissionID;
            CallAnalyticsByReflection("LogEventToFirebase",
                new object[] { TTPEvents.MINI_LEVEL_STARTED, miniMissionParams});
        }
            
        public static void MiniLevelCompleted()
        {
            Debug.Log(TTPLogger.LOGLabel + "TTPGameProgression::DDNAEvents:MiniLevelCompleted used from GameProgression plugin");
            CallAnalyticsByReflection("LogEventToFirebase",
                new object[] { TTPEvents.MINI_LEVEL_COMPLETED, miniMissionParams});
        }

        public static void MiniLevelFailed()
        {
            Debug.Log(TTPLogger.LOGLabel + "TTPGameProgression::DDNAEvents:MiniLevelFailed used from GameProgression plugin");
            CallAnalyticsByReflection("LogEventToFirebase",
                new object[] { TTPEvents.MINI_LEVEL_FAILED, miniMissionParams});
        }
        
        /// <summary>
        /// Log AppsFlyer level completed event
        /// </summary>
        /// <param name="additionalParams">Other additional parameters</param>
        static void AppsFlyerLevelComplete(Dictionary<string, object> additionalParams, string caller)
        {
            Debug.Log("AppsFlyerLevelComplete called:: " + TTPJson.Serialize(additionalParams) + ", " + caller);
#if TTP_APPSFLYER
            if (additionalParams == null)
            {
                additionalParams = new Dictionary<string, object>();
            }
            additionalParams.Add(LEVEL_PARAMETER_KEY, currentLevel);
            additionalParams.Add("caller", caller);

            TTPAppsFlyer.LogEvent(LEVEL_COMPLETE_EVENT_NAME, additionalParams);
#endif
        }

        /// <summary>
        /// Log mission is abandoned by user
        /// </summary>
        /// <param name="additionalParams">Other additional parameters</param>
        public static void MissionAbandoned(Dictionary<string, object> additionalParams)
        {
            TTPLogger.Log("TTPGameProgression::MissionAbandoned:");
            DDNAEvents.MissionAbandoned(additionalParams);
        }

        /// <summary>
        /// Log mission failed event
        /// </summary>
        /// <param name="additionalParams">Other additional parameters</param>
        public static void MissionFailed(Dictionary<string, object> additionalParams)
        {
            TTPLogger.Log("TTPGameProgression::MissionFailed:");
            DDNAEvents.MissionFailed(additionalParams);
        }

        /// <summary>
        /// Player levels up in the game
        /// </summary>
        /// <param name="skinName">Player's skin name</param>
        /// <param name="levelUpName">New level name</param>
        /// <param name="level">Level value</param>
        /// <param name="additionalParams">Other additional parameters</param>
        public static void LevelUp(string skinName, string levelUpName, int level, Dictionary<string, object> additionalParams)
        {
            TTPLogger.Log("TTPGameProgression::LevelUp:");
            UpdateCurrentLevel(level);
            DDNAEvents.LevelUp(skinName, levelUpName, level, additionalParams);
        }

        /// <summary>
        /// Deprecated class for Delta DNA analytics
        /// Use only for old apps
        /// </summary>
        public class DDNAEvents
        {
            /// <summary>
            /// Log mission started event
            /// </summary>
            /// <param name="id">Mission ID</param>
            /// <param name="name">Mission name</param>
            /// <param name="type">Mission type</param>
            /// <param name="missionStartedType">Type of mission startup</param>
            /// <param name="additionalParams">Other additional parameters</param>
            public static void MissionStarted(string id, string name, string type, string missionStartedType, Dictionary<string, object> additionalParams)
            {
                Debug.Log(TTPLogger.LOGLabel + "TTPGameProgression::DDNAEvents:MissionStarted used from GameProgression plugin");
                CallAnalyticsByReflection("MissionStarted", new object[] { id, name, type, missionStartedType, additionalParams });
            }

            /// <summary>
            /// Log mission completed event
            /// </summary>
            /// <param name="additionalParams">Other additional parameters</param>
            public static void MissionComplete(Dictionary<string, object> additionalParams)
            {
                Debug.Log(TTPLogger.LOGLabel + "TTPGameProgression::DDNAEvents:MissionComplete used from GameProgression plugin");
                AppsFlyerLevelComplete(additionalParams, "ddna");
                CallAnalyticsByReflection("MissionComplete",
                    new object[] { TTPEvents.MISSION_COMPLETED, additionalParams });
            }

            /// <summary>
            /// Log mission is abandoned by user
            /// </summary>
            /// <param name="additionalParams">Other additional parameters</param>
            public static void MissionAbandoned(Dictionary<string, object> additionalParams)
            {
                Debug.Log(TTPLogger.LOGLabel + "TTPGameProgression::DDNAEvents:MissionAbandoned used from GameProgression plugin");
                CallAnalyticsByReflection("MissionComplete",
                    new object[] { TTPEvents.MISSION_ABANDONED, additionalParams });
            }

            /// <summary>
            /// Log mission failed event
            /// </summary>
            /// <param name="additionalParams">Other additional parameters</param>
            public static void MissionFailed(Dictionary<string, object> additionalParams)
            {
                Debug.Log(TTPLogger.LOGLabel + "TTPGameProgression::DDNAEvents:MissionFailed used from GameProgression plugin");
                CallAnalyticsByReflection("MissionComplete",
                    new object[] { TTPEvents.MISSION_FAILED, additionalParams });
            }

            /// <summary>
            /// Player levels up in the game
            /// </summary>
            /// <param name="skinName">Player's skin name</param>
            /// <param name="levelUpName">New level name</param>
            /// <param name="level">Level value</param>
            /// <param name="additionalParams">Other additional parameters</param>
            public static void LevelUp(string skinName, string levelUpName, int level, Dictionary<string, object> additionalParams)
            {
                Debug.Log(TTPLogger.LOGLabel + "TTPGameProgression::DDNAEvents:LevelUp used from GameProgression plugin");
                UpdateCurrentLevel(level);

                CallAnalyticsByReflection("LevelUp", new object[] { skinName, levelUpName, level, additionalParams });
                if (Impl != null)
                {
                    Impl.PopUpMgrSetLevel(level);
                }
            }
        }


        public class FirebaseEvents
        {
            /// <summary>
            /// Log mission started event
            /// </summary>
            /// <param name="missionID">Mission ID</param>
            /// <param name="additionalParams">Other additional parameters</param>
            public static void MissionStarted(int missionID, Dictionary<string, object> additionalParams)
            {
                Debug.Log(TTPLogger.LOGLabel + "TTPGameProgression::FirebaseEvents:MissionStarted: missionID=" + missionID);
                CallAnalyticsByReflection("MissionStartedFirebase", new object[] { missionID, additionalParams });
            }

            /// <summary>
            /// Log mission completed event
            /// </summary>
            /// <param name="additionalParams">Other additional parameters</param>
            public static void MissionComplete(Dictionary<string, object> additionalParams)
            {
                Debug.Log(TTPLogger.LOGLabel + "TTPGameProgression::FirebaseEvents:MissionComplete:");

                AppsFlyerLevelComplete(additionalParams,"firebase");
                CallAnalyticsByReflection("MissionCompleteFirebase",
                    new object[] { TTPEvents.MISSION_COMPLETED, additionalParams });
            }

            /// <summary>
            /// Log mission is abandoned by user
            /// </summary>
            /// <param name="additionalParams">Other additional parameters</param>
            public static void MissionAbandoned(Dictionary<string, object> additionalParams)
            {
                Debug.Log(TTPLogger.LOGLabel + "TTPGameProgression::FirebaseEvents: MissionAbandoned:");
                CallAnalyticsByReflection("MissionCompleteFirebase",
                    new object[] { TTPEvents.MISSION_ABANDONED, additionalParams });
            }

            /// <summary>
            /// Log mission failed event
            /// </summary>
            /// <param name="additionalParams">Other additional parameters</param>
            public static void MissionFailed(Dictionary<string, object> additionalParams)
            {
                Debug.Log(TTPLogger.LOGLabel + "TTPGameProgression::FirebaseEvents:MissionFailed:");
                CallAnalyticsByReflection("MissionCompleteFirebase",
                    new object[] { TTPEvents.MISSION_FAILED, additionalParams });
            }

            public static void LevelUp(int level, Dictionary<string, object> additionalParams)
            {
                Debug.Log(TTPLogger.LOGLabel + "TTPGameProgression::FirebaseEvents:LevelUp: level=" + level);
                UpdateCurrentLevel(level);
                
                CallAnalyticsByReflection("LevelUpFirebase", new object[] { level, additionalParams });
                if (Impl != null)
                {
                    Impl.PopUpMgrSetLevel(level);
                }
            }
        }

        private static void CallAnalyticsByReflection(string methodName, object[] parameters)
        {
            System.Type analyticsClsType = System.Type.GetType("Tabtale.TTPlugins.TTPAnalytics");
            if (analyticsClsType != null)
            {
                MethodInfo method = analyticsClsType.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static);
                if (method != null)
                {
                    method.Invoke(null, parameters);
                }
                else
                {
                    Debug.LogWarning("CallAnalyticsByReflection:: could not find method - " + methodName);
                }
            }
            else
            {
                Debug.LogWarning("CallAnalyticsByReflection:: could not find TTPAnalytics class");
            }
        }

        private interface ITTPGameProgression
        {
            void PopUpMgrSetLevel(int level);
        }
        private static ITTPGameProgression _impl;
        private static ITTPGameProgression Impl
        {
            get
            {
                if (_impl == null)
                {
                    if (TTPCore.DevMode)
                    {
                        _impl = new EditorImpl();
                    }
                    else if (UnityEngine.Application.platform == UnityEngine.RuntimePlatform.Android ||
                        UnityEngine.Application.platform == UnityEngine.RuntimePlatform.IPhonePlayer)
                    {
#if UNITY_ANDROID
                        _impl = new AndroidImpl ();
#endif
#if UNITY_IOS && !TTP_DEV_MODE
                        _impl = new IosImpl();
#endif

                    }
                    else
                    {
#if UNITY_EDITOR
                        _impl = new EditorImpl();
#endif
                    }
                }
                if (_impl == null)
                {
                    Debug.LogError("TTPGameProgression::Impl: failed to create native impl");
                }
                return _impl;
            }
        }
#if UNITY_ANDROID
        private class AndroidImpl : ITTPGameProgression
        {
            private const string SERVICE_GET_METHOD = "getPopUpMgr";

            private AndroidJavaObject _serivceJavaObject;

            private AndroidJavaObject ServiceJavaObject
            {
                get
                {
                    if (_serivceJavaObject == null)
                    {
                        _serivceJavaObject = ((TTPCore.ITTPCoreInternal)TTPCore.Impl).GetServiceJavaObject(SERVICE_GET_METHOD);
                    }
                    if (_serivceJavaObject == null)
                        Debug.LogError("TTPGameProgression::AndroidImpl: failed to get popupmgr native instance.");
                    return _serivceJavaObject;
                }
            }

            public void PopUpMgrSetLevel(int level)
            {
                if(ServiceJavaObject != null)
                {
                    ServiceJavaObject.Call("setLevel", level);
                }
            }
        }
#endif
#if UNITY_IOS && !TTP_DEV_MODE
        private class IosImpl : ITTPGameProgression
        {
            [DllImport("__Internal")]
            private static extern void ttpPopUpMgrSetLevel(int level);

            public void PopUpMgrSetLevel(int level)
            {
                ttpPopUpMgrSetLevel(level);
            }
        }
#endif
        //#if UNITY_EDITOR
        private class EditorImpl : ITTPGameProgression
        {
            public void PopUpMgrSetLevel(int level)
            {
            }
        }
//#endif
    }

}
#endif