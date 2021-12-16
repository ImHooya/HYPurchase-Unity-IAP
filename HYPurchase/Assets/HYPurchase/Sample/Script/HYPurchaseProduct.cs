using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class HYPurchaseProduct : MonoBehaviour
{
    public Text displayName;
    public Text description;
    public Text price;
    private HYPurchase.HYProduct product;
    private HYPurchaseSample purchaseManager;
    public void Init(HYPurchase.HYProduct product, HYPurchaseSample sample)
    {
        this.product = product;
        displayName.text = product.displayName;
        description.text = product.description;
        RegionInfo regionInfo = new RegionInfo(product.locale);
        price.text = regionInfo.CurrencySymbol + product.price.ToString();

        purchaseManager = sample;
    }

    public void Purchase()
    {
        HYPurchase.HYPurchaseAPI.Purchase(product.productId, "TEST PURCHASE", (onPurchaseComplete) =>
        {
            purchaseManager.PurchaseComplete(product.productId, onPurchaseComplete.responseCode, onPurchaseComplete.message);
        });
    }
}
