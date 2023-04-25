using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UIAP_INSTALLED
using TTPIStoreController = UnityEngine.Purchasing.IStoreController;
#endif
namespace Tabtale.TTPlugins.UnityIAPWrapper
{
    public interface TTPIStoreListener
    {
        void OnInitialized(TTPIStoreController controller, TTPIExtensionProvider extensions);
        void OnInitializeFailed(TTPIInitializationFailureReason error);
        void OnPurchaseFailed(TTPIProduct product, TTPIPurchaseFailureReason purchaseFailureReason);
        TTPIPurchaseProcessingResult ProcessPurchase(TTPIPurchaseEventArgs e);
    }

    public abstract class TTPStandardPurchasingModule : TTPAbstractPurchasingModule
    {
        public TTPFakeStoreUIMode useFakeStoreUIMode { get; set; }

        public static TTPAbstractPurchasingModule Instance()
        {
            return null;
        }
    }

    public enum TTPIPurchaseProcessingResult
    {
        Complete, Pending
    }

    public enum TTPIPurchaseFailureReason
    {
        PurchasingUnavailable, 
        ExistingPurchasePending, 
        ProductUnavailable, 
        SignatureInvalid, 
        UserCancelled, 
        PaymentDeclined, 
        DuplicateTransaction, 
        Unknown
    }

    public enum TTPIInitializationFailureReason
    {
        PurchasingUnavailable, NoProductsAvailable, AppNotKnown
    }
}

