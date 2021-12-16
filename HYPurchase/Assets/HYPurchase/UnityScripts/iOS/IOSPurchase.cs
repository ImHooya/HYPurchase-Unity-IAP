using System;
using System.Collections.Generic;
using AOT;
using System.Runtime.InteropServices;

namespace HYPurchase
{
    public class IOSPurchase : IHYPlatformPurchase
    {
        #region NativeMethod
        [DllImport("__Internal")]
        private extern static void Init();
        [DllImport("__Internal")]
        private extern static void Refresh();
        [DllImport("__Internal")]
        private extern static void Purchase();
        [DllImport("__Internal")]
        private extern static void Consume();
        [DllImport("__Internal")]
        private extern static void Restore();
        #endregion
        #region CallbackListener
        private delegate void ConnectComplete(string message);
        [MonoPInvokeCallback(typeof(ConnectComplete))]
        private void ConnectCompleteListener(string message)
        {

        }
        private delegate void RefreshComplete(string message);
        [MonoPInvokeCallback(typeof(RefreshComplete))]
        private void RefreshCompleteListener(string message)
        {

        }
        private delegate void PurchaseComplete(string message);
        [MonoPInvokeCallback(typeof(PurchaseComplete))]
        private void PurchaseCompleteListener(string message)
        {

        }
        private delegate void ConsumeComplete(string message);
        [MonoPInvokeCallback(typeof(ConsumeComplete))]
        private void ConsumeCompleteListener(string message)
        {

        }
        private delegate void RestoreComplete(string message);
        [MonoPInvokeCallback(typeof(RestoreComplete))]
        private void RestoreCompleteListener(string message)
        {

        }
        #endregion
        public void Init(Action<OnConnectComplete> onConnectComplete)
        {

        }

        public void Consume(string productId, Action<OnConsumeComplete> onConsumeComplete)
        {
            throw new NotImplementedException();
        }

        public void Purchase(string productId, string developerPayload, Action<OnPurchaseComplete> onPurchaseComplete)
        {
            throw new NotImplementedException();
        }

        public void Refresh(List<string> productIdList, Action<OnRefreshComplete> onRefreshComplete)
        {
            throw new NotImplementedException();
        }

        public void Restore(string productId, Action<OnRestoreComplete> onRestoreComplete)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {

        }
    }
}
