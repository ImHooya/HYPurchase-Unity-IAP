
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
        public Action<string> ConsumeListener { get; set; }

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
        public void purchaseComplete(string message)
        {
            PurchaseListener?.Invoke(message);
        }
        public void consumeComplete(string message)
        {
            ConsumeListener?.Invoke(message);
        }
    }
}