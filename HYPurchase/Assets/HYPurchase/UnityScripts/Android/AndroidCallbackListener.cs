using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace HYPurchase
{
    public class AndroidCallbackListener : AndroidJavaProxy
    {
        private Action<OnConnectComplete> connectCallback;
        private Action<OnRefreshComplete> refreshCallback;
        private Action<OnPurchaseComplete> purchaseCallback;
        private Action<OnConsumeComplete> consumeCallback;
        private Action<OnRestoreComplete> restoreCallback;
        internal AndroidCallbackListener() : base("com.hypurchase.playstorepurchase.PurchaseListener")
        {

        }
        public void OnConnectComplete(string result, string unConsumedProducts)
        {
            var response = GetResponse(result);
            var productList = JsonConvert.DeserializeObject<HYReceipt[]>(unConsumedProducts);

            HYSyncContext.RunOnUnityThread(() =>
            {
                connectCallback(new HYPurchase.OnConnectComplete(response.Item1, response.Item2, productList.Select(x => x.productId).ToList()));
            });
        }
        public void OnRefreshComplete(string result, string products)
        {
            var response = GetResponse(result);

            var productArray = JsonConvert.DeserializeObject<HYProduct[]>(products);
            HYSyncContext.RunOnUnityThread(() =>
            {
                refreshCallback(new HYPurchase.OnRefreshComplete(response.Item1, response.Item2, productArray.ToList()));
            });
        }
        public void OnPurchaseComplete(string result, string receipt)
        {
            var response = GetResponse(result);

            var receiptObject = JsonConvert.DeserializeObject<HYReceipt>(receipt);
            HYSyncContext.RunOnUnityThread(() =>
            {
                purchaseCallback(new HYPurchase.OnPurchaseComplete(response.Item1, response.Item2, receiptObject));
            });
        }

        public void OnConsumeComplete(string result)
        {
            var response = GetResponse(result);
            HYSyncContext.RunOnUnityThread(() =>
            {
                consumeCallback(new HYPurchase.OnConsumeComplete(response.Item1, response.Item2));
            });
        }
        public void OnRestoreComplete(string result, string receipt)
        {
            var response = GetResponse(result);
            var receiptObject = JsonConvert.DeserializeObject<HYReceipt>(receipt);

            HYSyncContext.RunOnUnityThread(() =>
            {
                restoreCallback(new HYPurchase.OnRestoreComplete(response.Item1, response.Item2, receiptObject));
            });
        }

        private Tuple<int, string> GetResponse(string json)
        {
            var resultJson = JObject.Parse(json);
            int code = resultJson["responseCode"].Value<int>();
            string message = resultJson["responseMessage"].Value<string>();

            return new Tuple<int, string>(code, message);
        }

        public void SetConnectCallback(Action<OnConnectComplete> callback)
        {
            this.connectCallback = callback;
        }
        public void SetRefreshCallback(Action<OnRefreshComplete> callback)
        {
            this.refreshCallback = callback;
        }
        public void SetPurchaseCallback(Action<OnPurchaseComplete> callback)
        {
            this.purchaseCallback = callback;
        }
        public void SetConsumeCallback(Action<OnConsumeComplete> callback)
        {
            this.consumeCallback = callback;
        }
        public void SetRestoreCallback(Action<OnRestoreComplete> callback)
        {
            this.restoreCallback = callback;
        }
        //     fun OnConnectComplete(result : String, unConsumedProducts : String)
        // fun OnRefreshComplete(result : String, products : String)
        // fun OnPurchaseComplete(result : String, receipt : String)
        // fun OnConsumeComplete(result : String)
        // fun OnRestoreComplete(result : String, receipt : String)
    }

}
