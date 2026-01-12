using AOT;

namespace ImHooya.Purchase.IOS
{
    internal class IOSPurchaseHandler
    {
        internal delegate void InitComplete(string message);
        internal delegate void RefreshProductsComplete(string message);
        internal delegate void PurchaseComplete(string message);
        internal delegate void ConsumeComplete(string message);
        internal delegate void GetUnconsumedComplete(string message);
        internal delegate void GetStoreCountryCodeComplete(string message);

        internal static InitComplete OnInitComplete;
        internal static RefreshProductsComplete OnRefreshProductsComplete;
        internal static PurchaseComplete OnPurchaseComplete;
        internal static ConsumeComplete OnConsumeComplete;
        internal static GetUnconsumedComplete OnGetUnconsumedComplete;
        internal static GetStoreCountryCodeComplete OnGetStoreCountryCodeComplete;

        [MonoPInvokeCallback(typeof(InitComplete))]
        internal static void OnInitCompleteReceived(string message)
        {
            if (OnInitComplete != null)
                OnInitComplete(message);
        }
        [MonoPInvokeCallback(typeof(RefreshProductsComplete))]
        internal static void OnRefreshProductsCompleteReceived(string message)
        {
            if (OnRefreshProductsComplete != null)
                OnRefreshProductsComplete(message);
        }
        [MonoPInvokeCallback(typeof(PurchaseComplete))]
        internal static void OnPurchaseCompleteReceived(string message)
        {
            if (OnPurchaseComplete != null)
                OnPurchaseComplete(message);
        }
        [MonoPInvokeCallback(typeof(ConsumeComplete))]
        internal static void OnConsumeCompleteReceived(string message)
        {
            if (OnConsumeComplete != null)
                OnConsumeComplete(message);
        }
        [MonoPInvokeCallback(typeof(GetUnconsumedComplete))]
        internal static void OnGetUnconsumedCompleteReceived(string message)
        {
            if (OnGetUnconsumedComplete != null)
                OnGetUnconsumedComplete(message);
        }
        [MonoPInvokeCallback(typeof(GetStoreCountryCodeComplete))]
        internal static void OnGetStoreCountryCodeCompleteReceived(string message)
        {
            if (OnGetStoreCountryCodeComplete != null)
                OnGetStoreCountryCodeComplete(message);
        }
    }
}