using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tabtale.TTPlugins.UnityIAPWrapper
{
    public abstract class TTPIProduct
    {
        public bool availableToPurchase;
        public TTPIProductDefinition definition;
        public bool hasReceipt;
        public TTPIProductMetadata metadata;
        public string receipt;
        public string transactionID;
    }

    public abstract class TTPIProductDefinition
    {
        public bool enabled;
        public string id;
        public TTPIPayoutDefinition payout;
        public IEnumerable<TTPIPayoutDefinition> payouts;
        public string storeSpecificId;
        public TTPIProductType type;
    }

    public abstract class TTPIPayoutDefinition
    {
        public string data;
        public double quantity;
        public string subtype;
        public TTPIPayoutType type;
        public string typeString;
    }

    public enum TTPIPayoutType
    {
        Other,Currency,Resource
    }

    public enum TTPIProductType
    {
        Consumable, NonConsumable, Subscription
    }

    public abstract class TTPIProductMetadata
    {
        public string isoCurrencyCode;
        public string localizedDescription;
        public Decimal localizedPrice;
        public string localizedPriceString;
        public string localizedTitle;
    }

    public abstract class TTPIProductCollection
    {
        public TTPIProduct[] all;
        public HashSet<TTPIProduct> set;

        public TTPIProduct WithStoreSpecificID(string id)
        {
            return null;
        }
    }
}

