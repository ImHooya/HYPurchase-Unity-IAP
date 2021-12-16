using System.Collections;
using System.Collections.Generic;
using System;

namespace HYPurchase
{
    public static class HYPurchaseAPI
    {
        private static IHYPlatformPurchase platformPurchase;
        public static void Init(Action<OnConnectComplete> onConnectComplete)
        {
            if (platformPurchase != null)
            {
                onConnectComplete(new OnConnectComplete(-999, "Already initilized", null));
                return;
            }
            HYSyncContext syncContext = UnityEngine.GameObject.FindObjectOfType<HYSyncContext>();
            if (syncContext == null)
            {
                syncContext = new UnityEngine.GameObject("SyncContext").AddComponent<HYSyncContext>();
            }

#if UNITY_EDITOR
            platformPurchase = new EditorPurchase();
#elif UNITY_ANDROID
            platformPurchase = new AndroidPurchase();
#endif
            platformPurchase.Init(onConnectComplete);
        }

        public static void Refresh(List<string> productIdList, Action<OnRefreshComplete> onRefreshComplete)
        {
            platformPurchase.Refresh(productIdList, onRefreshComplete);
        }

        public static void Purchase(string productId, string developerPayload, Action<OnPurchaseComplete> onPurchaseComplete)
        {
            platformPurchase.Purchase(productId, developerPayload, onPurchaseComplete);
        }

        public static void Consume(string productId, Action<OnConsumeComplete> onConsumeComplete)
        {
            platformPurchase.Consume(productId, onConsumeComplete);
        }

        public static void Restore(string productId, Action<OnRestoreComplete> onRestoreComplete)
        {
            platformPurchase.Restore(productId, onRestoreComplete);
        }

        public static void Dispose()
        {
            platformPurchase.Dispose();
        }
    }
}
