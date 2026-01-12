
using System.Collections.Generic;
using ImHooya.Purchase;
using ImHooya.Purchase.Models;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HyPurchaseSample : MonoBehaviour
{
    [SerializeField]
    private TMP_Text text;
    private List<string> productIds = new List<string>
    {

    };

    private List<HyProductModel> productModels = new();

    public void Init()
    {
        PurchaseStore purchaseStore = PurchaseStore.PlayStore;

#if UNITY_IOS
        purchaseStore = PurchaseStore.AppStore;
#endif
        HYPurchase.Init(purchaseStore, (response) =>
        {
            Debug.Log($"response : {response.ResponseCode}, {response.ResponseMessage}");
            text.text = JsonConvert.SerializeObject(response, Formatting.Indented);
        });
    }

    public void RefreshProducts()
    {
        HYPurchase.RefreshProducts(productIds, response =>
        {
            Debug.Log($"[RefreshProducts] code={response.ResponseCode}, msg={response.ResponseMessage}");

            if (response.ResponseCode == 0 && response.ResponseObject != null)
            {
                text.text = JsonConvert.SerializeObject(response, Formatting.Indented);
                foreach (var product in response.ResponseObject)
                {
                    Debug.Log($"Product: {product.ProductId}, price={product.PriceString}");
                }

                productModels = response.ResponseObject;
            }
        });
    }

    public void BuyWithoutPayload()
    {
        HYPurchase.Purchase(productModels[0].ProductId, response =>
        {
            Debug.Log($"[Purchase] code={response.ResponseCode}, msg={response.ResponseMessage}");

            if (response.ResponseCode == 0 && response.ResponseObject != null)
            {
                Debug.Log($"Purchased productId={response.ResponseObject.ProductId}");

                text.text = JsonConvert.SerializeObject(response, Formatting.Indented);
            }
        });
    }

    public void BuyWithPayload()
    {
        string payload = System.Guid.NewGuid().ToString();

        HYPurchase.Purchase(productModels[0].ProductId, payload, response =>
        {
            Debug.Log($"[Purchase+Payload] code={response.ResponseCode}, msg={response.ResponseMessage}");

            if (response.ResponseCode == 0 && response.ResponseObject != null)
            {
                Debug.Log($"Purchased productId={response.ResponseObject.ProductId}");

                text.text = JsonConvert.SerializeObject(response, Formatting.Indented);
            }
        });
    }

    public void GetUnconsumedReceipts()
    {
        HYPurchase.GetUnconsumedReceipts(response =>
        {
            Debug.Log($"[GetUnconsumedReceipts] code={response.ResponseCode}, msg={response.ResponseMessage}");

            if (response.ResponseCode == 0 && response.ResponseObject != null)
            {
                text.text = JsonConvert.SerializeObject(response, Formatting.Indented);
                foreach (var receipt in response.ResponseObject)
                {
                    Debug.Log($"Receipt productId={receipt.ProductId}, orderId={receipt.OrderId}");
                }
            }
        });
    }

    public void Consume()
    {
        HYPurchase.Consume(productModels[0].ProductId, response =>
        {
            Debug.Log($"[Consume] code={response.ResponseCode}, msg={response.ResponseMessage}");
        });
    }

    public void ConsumeAll()
    {
        HYPurchase.ConsumeAll(response =>
        {
            Debug.Log($"[ConsumeAll] code={response.ResponseCode}, msg={response.ResponseMessage}");
        });
    }
}