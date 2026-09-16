#if UNITY_ANDROID

using System;
using System.Collections.Generic;
using ImHooya.Purchase.Android;
using ImHooya.Purchase.Models;
using UnityEngine;
using Newtonsoft.Json;

namespace ImHooya.Purchase
{
    internal class PlayStorePurchase : IHYStorePurchase
    {
        private const string AndroidClassPath = "com.imhooya.hypurchase.HyPlayStorePurchase";

        private readonly AndroidJavaObject purchaseObject;
        private readonly AndroidPurchaseHandler handler = new();

        private Action<HyResponseModel> initCallback;
        private Action<HyResponseModel<List<HyProductModel>>> refreshProductsCallback;
        private Action<HyResponseModel<List<HyReceiptModel>>> getUnconsumedReceiptsCallback;
        private Action<HyResponseModel<List<HyReceiptModel>>> getSubscribeReceiptsCallback;
        private Action<HyResponseModel<HyReceiptModel>> purchaseCallback;
        private Action<HyResponseModel<HyReceiptModel>> subscribeCallback;
        private Action<HyResponseModel> consumeCallback;
        private Action<HyResponseModel> acknowledgeCallback;
        private Action<HyResponseModel> consumeAllCallback;
        private Action<HyResponseModel<HyStoreCountryModel>> getStoreCountryCallback;

        public PlayStorePurchase()
        {
            purchaseObject = new AndroidJavaObject(AndroidClassPath);
            InitListeners();
        }

        public void Consume(string itemId, Action<HyResponseModel> callback)
        {
            consumeCallback = callback;

            purchaseObject.Call("consume", itemId);
        }

        public void ConsumeAll(Action<HyResponseModel> callback)
        {
            consumeAllCallback = callback;

            purchaseObject.Call("consumeAll");
        }

        public void Acknowledge(string itemId, Action<HyResponseModel> callback)
        {
            acknowledgeCallback = callback;

            purchaseObject.Call("acknowledge", itemId);
        }

        public PurchaseStore GetStoreType()
        {
            return PurchaseStore.PlayStore;
        }

        public void GetUnconsumedReceipts(Action<HyResponseModel<List<HyReceiptModel>>> callback)
        {
            getUnconsumedReceiptsCallback = callback;

            purchaseObject.Call("getUnconsumedReceipts");
        }

        public void GetSubscribeReceipts(Action<HyResponseModel<List<HyReceiptModel>>> callback)
        {
            getSubscribeReceiptsCallback = callback;

            purchaseObject.Call("getSubscribeReceipts");
        }

        public void Init(Action<HyResponseModel> callback)
        {
            using AndroidJavaClass unityPlayerActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            var currentActivity = unityPlayerActivity.GetStatic<AndroidJavaObject>("currentActivity");

            initCallback = callback;

            purchaseObject.Call("init", currentActivity, handler);
        }

        public void Purchase(string itemId, string payload, Action<HyResponseModel<HyReceiptModel>> callback)
        {
            purchaseCallback = callback;

            purchaseObject.Call("purchase", itemId, payload);
        }

        public void Subscribe(string itemId, string payload, Action<HyResponseModel<HyReceiptModel>> callback)
        {
            subscribeCallback = callback;

            purchaseObject.Call("subscribe", itemId, payload);
        }

        public void RefreshProducts(List<string> productIds, Action<HyResponseModel<List<HyProductModel>>> callback)
        {
            refreshProductsCallback = callback;

            purchaseObject.Call("refreshProducts", (productIds ?? new List<string>()).ToArray());
        }

        public void GetStoreCountry(Action<HyResponseModel<HyStoreCountryModel>> callback)
        {
            getStoreCountryCallback = callback;

            purchaseObject.Call("getStoreCountry");
        }

        private void OnInitComplete(string json)
        {
            var res = DeserializeResponse<HyResponseModel>(json);

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
                initCallback?.Invoke(res);
            });
        }

        private void OnRefreshProductsComplete(string json)
        {
            var res = DeserializeResponse<HyResponseModel<List<HyProductModel>>>(json);

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
                refreshProductsCallback?.Invoke(res);
            });
        }

        private void OnGetUnconsumedReceiptsComplete(string json)
        {
            var res = DeserializeResponse<HyResponseModel<List<HyReceiptModel>>>(json);

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
                getUnconsumedReceiptsCallback?.Invoke(res);
            });
        }

        private void OnGetSubscribeReceiptsComplete(string json)
        {
            var res = DeserializeResponse<HyResponseModel<List<HyReceiptModel>>>(json);

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
                getSubscribeReceiptsCallback?.Invoke(res);
            });
        }

        private void OnPurchaseComplete(string json)
        {
            var res = DeserializeResponse<HyResponseModel<HyReceiptModel>>(json);

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
                purchaseCallback?.Invoke(res);
            });
        }

        private void OnSubscribeComplete(string json)
        {
            var res = DeserializeResponse<HyResponseModel<HyReceiptModel>>(json);

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
                subscribeCallback?.Invoke(res);
            });
        }

        private void OnConsumeComplete(string json)
        {
            var res = DeserializeResponse<HyResponseModel>(json);

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
                consumeCallback?.Invoke(res);
            });
        }

        private void OnConsumeAllComplete(string json)
        {
            var res = DeserializeResponse<HyResponseModel>(json);

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
                consumeAllCallback?.Invoke(res);
            });
        }

        private void OnAcknowledgeComplete(string json)
        {
            var res = DeserializeResponse<HyResponseModel>(json);

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
                acknowledgeCallback?.Invoke(res);
            });
        }

        private void OnGetStoreCountryComplete(string json)
        {
            var res = DeserializeResponse<HyResponseModel<HyStoreCountryModel>>(json);

            if (res == null)
            {
                res = new HyResponseModel<HyStoreCountryModel>()
                {
                    ResponseCode = -902,
                    ResponseMessage = "json parsing error"
                };
            }

            HYMainThreadDispatcher.RunOnUnityThread(() =>
            {
                getStoreCountryCallback?.Invoke(res);
            });
        }

        private static T DeserializeResponse<T>(string json) where T : HyResponseModel
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (JsonException)
            {
                return null;
            }
        }

        private void InitListeners()
        {
            handler.InitListener = OnInitComplete;
            handler.RefreshProductsListener = OnRefreshProductsComplete;
            handler.GetUnconsumedReceiptsListener = OnGetUnconsumedReceiptsComplete;
            handler.GetSubscribeReceiptsListener = OnGetSubscribeReceiptsComplete;
            handler.PurchaseListener = OnPurchaseComplete;
            handler.SubscribeListener = OnSubscribeComplete;
            handler.ConsumeListener = OnConsumeComplete;
            handler.AcknowledgeListener = OnAcknowledgeComplete;
            handler.ConsumeAllListener = OnConsumeAllComplete;
            handler.GetStoreCountryListener = OnGetStoreCountryComplete;
        }
    }
}

#endif
