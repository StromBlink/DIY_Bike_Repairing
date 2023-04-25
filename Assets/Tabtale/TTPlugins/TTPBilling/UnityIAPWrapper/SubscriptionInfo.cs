#if !UIAP_INSTALLED
using System;

    public class SubscriptionInfo
    {
        public SubscriptionInfo(
            string skuDetails,
            bool isAutoRenewing,
            DateTime purchaseDate,
            bool isFreeTrial,
            bool hasIntroductoryPriceTrial,
            bool purchaseHistorySupported,
            string updateMetadata)
        {
        }

        public SubscriptionInfo(string productId)
        {
            
        }


        public string getProductId()
        {
            return null;
        }

        public DateTime getPurchaseDate()
        {
            return DateTime.Now;
        }

        public Result isSubscribed()
        {
            return Result.False;
        }

        public Result isExpired()
        {
            return Result.False;
        }

        public Result isCancelled() 
        {
            return Result.False;
        }
        public Result isFreeTrial() 
        {
            return Result.False;
        }

        public Result isAutoRenewing() 
        {
            return Result.False;
        }

        public TimeSpan getRemainingTime()
        {
            return TimeSpan.Zero;
        }

        public Result isIntroductoryPricePeriod() 
        {
            return Result.False;
        }

        public TimeSpan getIntroductoryPricePeriod() 
        {
            return TimeSpan.Zero;
        }

        public string getIntroductoryPrice()
        {
            return null;
        }

        public long getIntroductoryPricePeriodCycles()
        {
            return 0;
        }

        public DateTime getExpireDate() 
        {
            return DateTime.Now;
        }

        public DateTime getCancelDate() 
        {
            return DateTime.Now;
        }

        public TimeSpan getFreeTrialPeriod() 
        {
            return TimeSpan.Zero;
        }

        public TimeSpan getSubscriptionPeriod() 
        {
            return TimeSpan.Zero;
        }

        public string getFreeTrialPeriodString()
        {
            return null;
        }

        public string getSkuDetails()
        {
            return null;
        }

        public string getSubscriptionInfoJsonString() 
        {
            return null;
        }

    }
    
    public enum Result
    {
        True,
        False,
        Unsupported,
    }
#endif

