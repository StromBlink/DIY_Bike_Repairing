using System.Collections;
using System.Collections.Generic;
using Tabtale.TTPlugins;
using UnityEngine;

public class CLIKBillingDemoScene : MonoBehaviour
{
    private enum ItemType
    {
        Consumable, NonConsumable, Subscription
    }

    //fill in your store items in the unity editor and create corresponding buttons with CLIKBillingDemoSceneItem components
    public CLIKBillingDemoSceneItem[] storeItems;

    // Start is called before the first frame update
    void Start()
    {
        TTPBilling.OnBillingInitEvent += TTPBilling_OnBillingInitEvent;
        TTPBilling.OnItemPurchasedEvent += TTPBilling_OnItemPurchasedEvent;
        TTPCore.Setup();
    }

    //Will be fire automatically on Android when initiating CLIK in case item was already purchased.
    private void TTPBilling_OnItemPurchasedEvent(PurchaseIAPResult purchaseIAPResult)
    {
        foreach(var item in storeItems)
        {
            if(item.itemId == purchaseIAPResult.purchasedItem.mainId)
            {
                var itemType = ItemType.Consumable;
                if (purchaseIAPResult.purchasedItem.isSubscription)
                {
                    itemType = ItemType.Subscription;
                }
                item.UpdateItem(itemType.ToString(), purchaseIAPResult.purchasedItem.localizedPrice,
                    CLIKBillingDemoSceneItem.PurchaseStatus.Purchased);
            }
        }
    }

    private void TTPBilling_OnBillingInitEvent(BillerErrors billerErrors)
    {
        if(billerErrors == BillerErrors.NO_ERROR)
        {
            foreach(var item in storeItems)
            {
                var itemType = ItemType.Consumable;
                if (TTPBilling.GetSubscriptionInfo(item.itemId) != null)
                {
                    itemType = ItemType.Subscription;
                }
                else if(!TTPBilling.IsConsumable(item.itemId))
                {
                    itemType = ItemType.NonConsumable;
                }

                var itemPrice = TTPBilling.GetLocalizedPriceString(item.itemId);
                var isPurchased = TTPBilling.IsPurchased(item.itemId);
                item.UpdateItem(itemType.ToString(), itemPrice,
                    (isPurchased ?
                    CLIKBillingDemoSceneItem.PurchaseStatus.Purchased :
                    CLIKBillingDemoSceneItem.PurchaseStatus.NotPurchased));
            }
        }
        else
        {
            Debug.LogError("Billing failed to init. Error = " + billerErrors);
        }
    }

}
