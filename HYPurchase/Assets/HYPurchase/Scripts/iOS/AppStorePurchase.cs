#if UNITY_IOS

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ImHooya.Purchase.IOS;
using ImHooya.Purchase.Models;
using Newtonsoft.Json;

namespace ImHooya.Purchase
{
    internal class AppStorePurchase : IHYStorePurchase
    {
        [DllImport("__Internal")]
        private static extern void HyPurchase_init(IOSPurchaseHandler.InitComplete callback);

        [DllImport("__Internal")]
        private static extern void HyPurchase_refreshProducts(string productIds, IOSPurchaseHandler.RefreshProductsComplete callback);

        [DllImport("__Internal")]
        private static extern void HyPurchase_purchase(string productId, string payload, IOSPurchaseHandler.PurchaseComplete callback);

        [DllImport("__Internal")]
        private static extern void HyPurchase_consume(string productId, IOSPurchaseHandler.ConsumeComplete callback);

        [DllImport("__Internal")]
        private static extern void HyPurchase_consumeAll(IOSPurchaseHandler.ConsumeComplete callback);

        [DllImport("__Internal")]
        private static extern void HyPurchase_getUnconsumedReceipts(IOSPurchaseHandler.GetUnconsumedComplete callback);

        public void Consume(string itemId, Action<HyResponseModel> callback)
        {
            IOSPurchaseHandler.OnConsumeComplete = json =>
            {
                var res = JsonConvert.DeserializeObject<HyResponseModel>(json);

                if (res == null)
                {
                    res = new HyResponseModel()
                    {
                        ResponseCode = -902,
                        ResponseMessage = "json parsing error"
                    };
                }

                HYMainThreadDispatcher.RunOnUnityThread(() =>
                {
                    callback?.Invoke(res);
                });
            };

            HyPurchase_consume(itemId, IOSPurchaseHandler.OnConsumeCompleteReceived);
        }

        public void ConsumeAll(Action<HyResponseModel> callback)
        {
            IOSPurchaseHandler.OnConsumeComplete = json =>
            {
                var res = JsonConvert.DeserializeObject<HyResponseModel>(json);

                if (res == null)
                {
                    res = new HyResponseModel()
                    {
                        ResponseCode = -902,
                        ResponseMessage = "json parsing error"
                    };
                }

                HYMainThreadDispatcher.RunOnUnityThread(() =>
                {
                    callback?.Invoke(res);
                });
            };

            HyPurchase_consumeAll(IOSPurchaseHandler.OnConsumeCompleteReceived);
        }

        public PurchaseStore GetStoreType()
        {
            return PurchaseStore.AppStore;
        }

        public void GetUnconsumedReceipts(Action<HyResponseModel<List<HyReceiptModel>>> callback)
        {
            IOSPurchaseHandler.OnGetUnconsumedComplete = json =>
            {
                var res = JsonConvert.DeserializeObject<HyResponseModel<List<HyReceiptModel>>>(json);

                if (res == null)
                {
                    res = new HyResponseModel<List<HyReceiptModel>>()
                    {
                        ResponseCode = -902,
                        ResponseMessage = "json parsing error"
                    };
                }

                HYMainThreadDispatcher.RunOnUnityThread(() =>
                {
                    callback?.Invoke(res);
                });
            };

            HyPurchase_getUnconsumedReceipts(IOSPurchaseHandler.OnGetUnconsumedCompleteReceived);
        }

        public void Init(Action<HyResponseModel> callback)
        {
            IOSPurchaseHandler.OnInitComplete = json =>
            {
                var res = JsonConvert.DeserializeObject<HyResponseModel>(json);

                if (res == null)
                {
                    res = new HyResponseModel()
                    {
                        ResponseCode = -902,
                        ResponseMessage = "json parsing error"
                    };
                }

                HYMainThreadDispatcher.RunOnUnityThread(() =>
                {
                    callback?.Invoke(res);
                });
            };

            HyPurchase_init(IOSPurchaseHandler.OnInitCompleteReceived);
        }

        public void Purchase(string productId, string payload, Action<HyResponseModel<HyReceiptModel>> callback)
        {
            IOSPurchaseHandler.OnPurchaseComplete = json =>
            {
                var res = JsonConvert.DeserializeObject<HyResponseModel<HyReceiptModel>>(json);

                if (res == null)
                {
                    res = new HyResponseModel<HyReceiptModel>()
                    {
                        ResponseCode = -902,
                        ResponseMessage = "json parsing error"
                    };
                }

                HYMainThreadDispatcher.RunOnUnityThread(() =>
                {
                    callback?.Invoke(res);
                });
            };

            HyPurchase_purchase(productId, payload, IOSPurchaseHandler.OnPurchaseCompleteReceived);
        }

        public void RefreshProducts(List<string> productIds, Action<HyResponseModel<List<HyProductModel>>> callback)
        {
            var payloadObj = new { list = productIds ?? new List<string>() };
            var itemIdsJson = JsonConvert.SerializeObject(payloadObj);

            IOSPurchaseHandler.OnRefreshProductsComplete = json =>
            {
                var res = JsonConvert.DeserializeObject<HyResponseModel<List<HyProductModel>>>(json);

                if (res == null)
                {
                    res = new HyResponseModel<List<HyProductModel>>()
                    {
                        ResponseCode = -902,
                        ResponseMessage = "json parsing error"
                    };
                }

                HYMainThreadDispatcher.RunOnUnityThread(() =>
                {
                    callback?.Invoke(res);
                });
            };

            HyPurchase_refreshProducts(itemIdsJson, IOSPurchaseHandler.OnRefreshProductsCompleteReceived);
        }
    }
}

#endif