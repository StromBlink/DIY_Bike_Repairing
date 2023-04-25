using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using UnityEngine;

namespace Tabtale.TTPlugins
{
    public static class TestIAP
    {
        public const string NO_ADS_ITEM_ID = "testNoAds";
        private static readonly ReadOnlyCollection<string> testBundleIds = new ReadOnlyCollection<string>(
            new string[]
            {
                "com.tabtaleint.uptaiap",
                "com.tabtaleint.ttplugins"
            }
        );

        public static bool IsNoAdsPurchased
        { get; private set; }

        private static bool? isTest;

        static TestIAP()
        {
            IsNoAdsPurchased = false;
        }

        public static bool IsTest()
        {
            if (!isTest.HasValue)
            {
                isTest = false;
                string configuration = new StreamReader(Path.Combine(Application.temporaryCachePath, "ttp/configurations/configuration.json")).ReadToEnd();
                if (configuration != null)
                {
                    bool containsAutoTestConfig = configuration.IndexOf("\"autoTestConfig\":", StringComparison.CurrentCulture) > 0;
                    string bundleId = Application.identifier;
                    if (testBundleIds.Contains(bundleId) && containsAutoTestConfig)
                        isTest = true;
                }
            }

            return isTest ?? false;
        }

        public static bool ProccessNoAdsPurchase (string itemId, System.Action<PurchaseIAPResult> onPurchasedAction)
        {
            if(!IsNoAdsPurchased)
                ProcessPurchase(itemId, onPurchasedAction);

            IsNoAdsPurchased = !IsNoAdsPurchased;
            return IsNoAdsPurchased;
        }

        public static void ProcessPurchase(string itemId, System.Action<PurchaseIAPResult> onPurchasedAction)
        {
#if TTP_ANALYTICS
            IDictionary<string, object> fakeEventParams = new Dictionary<string, object>()
            {
                {"fakeIap", itemId}
            };
            TTPAnalytics.LogTransaction("Store Purchase", null, null, fakeEventParams);
            LogTransactionEventToFirebase();
#endif
            onPurchasedAction.Invoke(new PurchaseIAPResult(new InAppPurchasableItem(), PurchaseIAPResultCode.Success, BillerErrors.NO_ERROR));
        }
        
        private static void LogTransactionEventToFirebase()
        {
#if TTP_ANALYTICS

                Dictionary<string, object> transactionParams = new Dictionary<string, object>();
                transactionParams["price"] ="3.99";
                transactionParams["currency"] = "USD";
                transactionParams["productID"] = "";
                transactionParams["geo"] = "US";
                transactionParams["fakeIap"] = NO_ADS_ITEM_ID;
                TTPAnalytics.LogEvent(AnalyticsTargets.ANALYTICS_TARGET_FIREBASE,
                    TTPEvents.TRANSACTION, transactionParams, false, true);
#endif

        }
    }
}