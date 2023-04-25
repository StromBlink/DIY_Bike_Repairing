using System.Collections;
using System.Collections.Generic;
using Tabtale.TTPlugins;
using UnityEngine;
using UnityEngine.UI;

public class CLIKBillingDemoSceneItem : MonoBehaviour
{
    public enum PurchaseStatus
    {
        NA, Purchased, NotPurchased
    }

    public string itemId;
    public string itemName;
    private string _itemPrice = "NA";
    private string _itemType = "NA";
    private PurchaseStatus _purchaseStatus = PurchaseStatus.NA;
    private Text _text;
    private Button _button;

    private void Awake()
    {
        _text = GetComponentInChildren<Text>();
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        TTPBilling.PurchaseItem(itemId, OnItemPurchaseResult);
    }

    private void OnItemPurchaseResult(PurchaseIAPResult purchaseIAPResult)
    {
        if(purchaseIAPResult.result == PurchaseIAPResultCode.Success)
        {
            UpdateItem(purchaseStatus: PurchaseStatus.Purchased);
        }
        else if (purchaseIAPResult.result == PurchaseIAPResultCode.Failed)
        {
            Debug.LogError("Purchased failed. Error - " + purchaseIAPResult.error);
        }
        else if (purchaseIAPResult.result == PurchaseIAPResultCode.Cancelled)
        {
            Debug.LogWarning("Purchase Cancelled.");
        }
    }

    public void UpdateItem(string itemType = null, string itemPrice = null, PurchaseStatus purchaseStatus = PurchaseStatus.NA)
    {
        _itemType = (itemType ?? _itemType);
        _itemPrice = (itemPrice ?? _itemPrice);
        _purchaseStatus = (purchaseStatus != PurchaseStatus.NA ? purchaseStatus : _purchaseStatus);
        _text.text = itemName + "\n" + _itemType + "\n" + _itemPrice + "\n" + _purchaseStatus;
    }

   


}
