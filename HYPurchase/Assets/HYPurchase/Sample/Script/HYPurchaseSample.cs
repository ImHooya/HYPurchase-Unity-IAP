using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HYPurchase;
using UnityEngine.UI;

public class HYPurchaseSample : MonoBehaviour
{
    public Transform productViewContainer;
    public Transform productContent;
    public Text responseCode;
    public Text responseMessage;
    public List<string> testProdcuctIdList = new List<string>();
    private List<HYPurchaseProduct> productObjectList = new List<HYPurchaseProduct>();
    private List<HYProduct> productList = new List<HYProduct>();
    private List<string> unConsumedIdList = new List<string>();
    private string lastPurchaseProductId = string.Empty;

    /// <summary>
    /// Init HYPurchaseAPI
    /// </summary>
    public void Init()
    {
        HYPurchaseAPI.Init((x) =>
        {
            responseCode.text = x.responseCode.ToString();
            responseMessage.text = x.message;
            unConsumedIdList = x.unConsumedProductIdList;

            if (productObjectList.Count != 0)
            {
                foreach (var product in productObjectList)
                {
                    Destroy(product.gameObject);
                }

                productObjectList = new List<HYPurchaseProduct>();
            }
        });
    }

    /// <summary>
    /// Refresh Products
    /// </summary>
    public void Refresh()
    {
        HYPurchaseAPI.Refresh(testProdcuctIdList, (x) =>
        {
            responseCode.text = x.responseCode.ToString();
            responseMessage.text = x.message;
            Debug.Log("x.productList.Count : " + x.productList.Count);
            for (int i = 0; i < x.productList.Count; i++)
            {
                Debug.Log(x.productList[i].productId);
                HYPurchaseProduct product = GameObject.Instantiate(productContent, productViewContainer)
                                                .GetComponent<HYPurchaseProduct>();
                product.GetComponent<RectTransform>().anchoredPosition = new Vector2(140 + 240 * i, -40);
                product.Init(x.productList[i], this);

                productObjectList.Add(product);
            }
        });
    }

    /// <summary>
    /// Consume Product
    /// </summary>
    public void Consume()
    {
        if (string.IsNullOrEmpty(lastPurchaseProductId))
        {
            responseCode.text = "-999";
            responseMessage.text = "No Purchased Item";
            return;
        }

        HYPurchaseAPI.Consume(lastPurchaseProductId, (x) =>
        {
            responseCode.text = x.responseCode.ToString();
            responseMessage.text = x.message;
        });
    }

    /// <summary>
    /// Restore Product
    /// </summary>
    public void Restore()
    {
        unConsumedIdList.ForEach((x) =>
        {
            HYPurchaseAPI.Restore(x, (onRestoreComplete) =>
            {
                HYPurchaseAPI.Consume(lastPurchaseProductId, (onConsumeComplete) =>
                {
                    responseCode.text = onRestoreComplete.responseCode.ToString();
                    responseMessage.text = onRestoreComplete.message;
                });
            });
        });
    }

    public void PurchaseComplete(string productId, int responseCode, string responseMessage)
    {
        lastPurchaseProductId = productId;
        this.responseCode.text = responseCode.ToString() + " : purchase";
        this.responseMessage.text = responseMessage;
    }

    void OnDestroy()
    {
        HYPurchaseAPI.Dispose();
    }
}
