#if UNITY_ANDROID
using System;
using UnityEngine;

namespace ImHooya.Purchase.Android
{
    internal class AndroidPurchaseHandler : AndroidJavaProxy
    {
        private const string AndroidClassPath = "com.imhooya.hypurchase.IHyPurchaseHandler";

        public AndroidPurchaseHandler() : base(AndroidClassPath)
        {

        }

        public Action<string> InitListener { get; set; }
        public Action<string> RefreshProductsListener { get; set; }
        public Action<string> PurchaseListener { get; set; }
        public Action<string> GetUnconsumedReceiptsListener { get; set; }
        public Action<string> GetSubscribeReceiptsListener { get; set; }
        public Action<string> SubscribeListener { get; set; }
        public Action<string> ConsumeListener { get; set; }
        public Action<string> AcknowledgeListener { get; set; }
        public Action<string> ConsumeAllListener { get; set; }
        public Action<string> GetStoreCountryListener { get; set; }

        public void initComplete(string message)
        {
            InitListener?.Invoke(message);
        }
        public void getProductsComplete(string message)
        {
            RefreshProductsListener?.Invoke(message);
        }
        public void getUnconsumedReceiptsComplete(string message)
        {
            GetUnconsumedReceiptsListener?.Invoke(message);
        }
        public void getSubscribeReceiptsComplete(string message)
        {
            GetSubscribeReceiptsListener?.Invoke(message);
        }
        public void purchaseComplete(string message)
        {
            PurchaseListener?.Invoke(message);
        }
        public void subscribeComplete(string message)
        {
            SubscribeListener?.Invoke(message);
        }
        public void consumeComplete(string message)
        {
            ConsumeListener?.Invoke(message);
        }
        public void acknowledgeComplete(string message)
        {
            AcknowledgeListener?.Invoke(message);
        }
        public void consumeAllComplete(string message)
        {
            ConsumeAllListener?.Invoke(message);
        }
        public void getStoreCountryComplete(string message)
        {
            GetStoreCountryListener?.Invoke(message);
        }
    }
}
#endif
