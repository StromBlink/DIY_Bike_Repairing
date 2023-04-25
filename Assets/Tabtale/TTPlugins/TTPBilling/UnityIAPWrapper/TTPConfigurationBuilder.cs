using System.Collections;
using System.Collections.Generic;
using Tabtale.TTPlugins.UnityIAPWrapper;
using UnityEngine;
#if UIAP_INSTALLED
using  SubscriptionInfo = UnityEngine.Purchasing.SubscriptionInfo;
#endif
namespace Tabtale.TTPlugins.UnityIAPWrapper
{
#if UNITY_IAP
    // Create alias for existing Unity purchasing classes
    using TTPIDs = UnityEngine.Purchasing.IDs;
    using TTPConfigurationBuilder = UnityEngine.Purchasing.ConfigurationBuilder;
#else 
    // Stub classes
    public class TTPIDs : IEnumerable<KeyValuePair<string, string>>, IEnumerable
    {
        private Dictionary<string, string> m_Dic = new Dictionary<string, string>();
        
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return (IEnumerator<KeyValuePair<string, string>>) this.m_Dic.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator) this.m_Dic.GetEnumerator();
        }

        public void Add(string iapId, string storeName)
        {
            m_Dic[iapId] = storeName;
        }
    }
    
    public abstract class TTPConfigurationBuilder
    {
        private static TTPConfigurationBuilder _instance;
        
        public static TTPConfigurationBuilder Instance(TTPAbstractPurchasingModule purchasingModule)
        {
            return _instance;
        }
        
        public abstract void AddProduct(string iapId, TTPIProductType productType, TTPIDs ds);

        public TTPIAppleConfiguration Configure<T>()
        {
            return null;
        }
    }
    
    public class TTPMacAppStore
    {
        public const string Name = "MacAppStore";
    }

    public class TTPAmazonApps
    {
        public const string Name = "AmazonApps";
    }

    public class TTPAppleAppStore
    {
        public const string Name = "AppleAppStore";
    }

    public class TTPGooglePlay
    {
        public const string Name = "GooglePlay";
    }

    public class TTPAppleInAppPurchaseReceipt
    {
        public string transactionID { get; set; }
        public string originalTransactionIdentifier { get; set; }
        public string productID { get; set; }
    }

    public class TTPAppleReceipt
    {
        public IEnumerable<TTPAppleInAppPurchaseReceipt> inAppPurchaseReceipts { get; set; }
    }

    public class TTPAppleValidator
    {
        public TTPAppleValidator(byte[] data)
        {
            
        }

        public TTPAppleReceipt Validate(byte[] receiptData)
        {
            return new TTPAppleReceipt();
        }
    }

    public class TTPAppleTangle
    {
        public static byte[] Data()
        {
            return null;
        }
    }

    public class TTPSubscriptionManager
    {
        public TTPSubscriptionManager(TTPIProduct item, string introJson)
        {
            
        }

        public SubscriptionInfo getSubscriptionInfo()
        {
            return null;
        }
    }
#endif
    public interface TTPIAppleConfiguration
    {
        void SetPublicKey(string googlePublicKey);
        string appReceipt { get; set; }
    }
}

