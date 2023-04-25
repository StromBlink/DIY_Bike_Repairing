using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tabtale.TTPlugins.UnityIAPWrapper
{
    public interface TTPIPurchaseEventArgs
    {
        //TTPIProduct GetPurchasedProduct();
        TTPIProduct purchasedProduct { get; }
    }
}

