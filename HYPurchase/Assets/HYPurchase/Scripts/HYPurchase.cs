using System;
using System.Collections.Generic;
using ImHooya.Purchase.Models;
using UnityEngine;

namespace ImHooya.Purchase
{
    public enum PurchaseStore
    {
        PlayStore,
        AppStore
    }
    public static class HYPurchase
    {
        private static IHYStorePurchase storePurchase;
        public static void Init(PurchaseStore store, Action<HyResponseModel> callback)
        {
            if (store == PurchaseStore.PlayStore)
            {
#if UNITY_ANDROID
                storePurchase = new PlayStorePurchase();
#endif
            }
            else if (store == PurchaseStore.AppStore)
            {
#if UNITY_IOS
                storePurchase = new AppStorePurchase();
#endif
            }

            if (storePurchase == null)
            {
                var response = new HyResponseModel();
                response.ResponseCode = -900;
                response.ResponseMessage = "Store os mismatch";
                callback(response);
            }
            else
            {
                storePurchase.Init(callback);
            }

        }

        public static void RefreshProducts(List<string> productIds, Action<HyResponseModel<List<HyProductModel>>> callback)
        {
            if (storePurchase == null)
            {
                var response = new HyResponseModel<List<HyProductModel>>();
                response.ResponseCode = -901;
                response.ResponseMessage = "not initialized";
                callback(response);
                return;
            }

            storePurchase.RefreshProducts(productIds, callback);
        }

        public static void Purchase(string productId, Action<HyResponseModel<HyReceiptModel>> callback)
        {
            if (storePurchase == null)
            {
                var response = new HyResponseModel<HyReceiptModel>();
                response.ResponseCode = -901;
                response.ResponseMessage = "not initialized";
                callback(response);
                return;
            }

            storePurchase.Purchase(productId, string.Empty, callback);
        }

        public static void Purchase(string productId, string payload, Action<HyResponseModel<HyReceiptModel>> callback)
        {
            if (storePurchase == null)
            {
                var response = new HyResponseModel<HyReceiptModel>();
                response.ResponseCode = -901;
                response.ResponseMessage = "not initialized";
                callback(response);
                return;
            }

            if (!string.IsNullOrEmpty(payload))
            {
                if (!Guid.TryParse(payload, out _))
                {
                    callback(new HyResponseModel<HyReceiptModel>
                    {
                        ResponseCode = -902,
                        ResponseMessage = "invalid payload uuid format"
                    });
                    return;
                }
            }

            storePurchase.Purchase(productId, payload, callback);
        }

        public static void GetUnconsumedReceipts(Action<HyResponseModel<List<HyReceiptModel>>> callback)
        {
            if (storePurchase == null)
            {
                var response = new HyResponseModel<List<HyReceiptModel>>();
                response.ResponseCode = -901;
                response.ResponseMessage = "not initialized";
                callback(response);
                return;
            }

            storePurchase.GetUnconsumedReceipts(callback);
        }

        public static void Consume(string productId, Action<HyResponseModel> callback)
        {
            if (storePurchase == null)
            {
                var response = new HyResponseModel();
                response.ResponseCode = -901;
                response.ResponseMessage = "not initialized";
                callback(response);
                return;
            }

            storePurchase.Consume(productId, callback);
        }

        public static void ConsumeAll(Action<HyResponseModel> callback)
        {
            if (storePurchase == null)
            {
                var response = new HyResponseModel();
                response.ResponseCode = -901;
                response.ResponseMessage = "not initialized";
                callback(response);
                return;
            }

            storePurchase.ConsumeAll(callback);
        }
    }
}
