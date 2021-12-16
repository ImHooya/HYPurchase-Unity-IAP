#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using Newtonsoft.Json;

namespace HYPurchase
{
    public class EditorPurchase : IHYPlatformPurchase
    {
        private HYPurchaseEditor editorSetting;
        public EditorPurchase()
        {
            editorSetting = HYPurchaseEditor.GetInstance();
        }
        public void Init(Action<OnConnectComplete> onConnectComplete)
        {
            if (onConnectComplete != null)
            {
                int responseCode = editorSetting.ConnectSuccess ? 0 : 1;
                string message = responseCode == 0 ? "Connect Success" : "Connect Failed";
                onConnectComplete(new OnConnectComplete(responseCode, message, null));
            }
            else
            {
                throw new Exception("OnConnectComplete Callback is null");
            }
        }
        public void Refresh(List<string> productIdList, Action<OnRefreshComplete> onRefreshComplete)
        {
            if (onRefreshComplete != null)
            {
                int responseCode = editorSetting.RefreshSuccess ? 0 : 1;
                string message = responseCode == 0 ? "Refresh Success" : "Refresh Failed";
                List<HYProduct> productList = new List<HYProduct>();

                if (responseCode == 0)
                {
                    productList = productIdList
                    .Where(productId =>
                    {
                        return editorSetting.productList.Any(x => x.productId == productId);
                    })
                    .Select(productId =>
                    {
                        return editorSetting.productList.First(x => x.productId == productId);
                    })
                    .Select(editorProduct =>
                    {
                        string json = EditorJsonUtility.ToJson(editorProduct);
                        HYProduct product = JsonConvert.DeserializeObject<HYProduct>(json);
                        EditorJsonUtility.FromJsonOverwrite(json, product);
                        return product;
                    })
                    .ToList();
                }
                onRefreshComplete(new OnRefreshComplete(responseCode, message, productList));
            }
            else
            {
                throw new Exception("OnRefreshComplete Callback is null");
            }
        }
        public void Purchase(string productId, string developerPayload, Action<OnPurchaseComplete> onPurchaseComplete)
        {
            if (onPurchaseComplete != null)
            {
                int responseCode = editorSetting.PurchaseSuccess ? 0 : 1;
                string message = responseCode == 0 ? "Purchase Success" : "Purchase Failed";

                HYReceipt receipt = null;
                if (responseCode == 0)
                {
                    receipt = new HYReceipt();
                }

                onPurchaseComplete(new OnPurchaseComplete(responseCode, message, receipt));
            }
            else
            {
                throw new Exception("OnPurchaseComplete Callback is null");
            }
        }
        public void Consume(string productId, Action<OnConsumeComplete> onConsumeComplete)
        {
            if (onConsumeComplete != null)
            {
                int responseCode = editorSetting.ConsumeSuccess ? 0 : 1;
                string message = responseCode == 0 ? "Consume Success" : "Consume Failed";
                onConsumeComplete(new OnConsumeComplete(responseCode, message));
            }
            else
            {
                throw new Exception("OnConsumeComplete Callback is null");
            }
        }
        public void Restore(string productId, Action<OnRestoreComplete> onRestoreComplete)
        {
            if (onRestoreComplete != null)
            {
                int responseCode = editorSetting.RestoreSuccess ? 0 : 1;
                string message = responseCode == 0 ? "Restore Success" : "Restore Failed";

                HYReceipt receipt = null;
                if (responseCode == 0)
                {
                    receipt = new HYReceipt();
                }

                onRestoreComplete(new OnRestoreComplete(responseCode, message, receipt));
            }
            else
            {
                throw new Exception("OnRestoreComplete Callback is null");
            }
        }

        public void Dispose()
        {

        }
    }
}

#endif