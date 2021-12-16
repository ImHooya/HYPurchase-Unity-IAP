using System;
using System.Collections.Generic;
using UnityEngine;

namespace HYPurchase
{
    public class AndroidPurchase : IHYPlatformPurchase
    {
        private const string AndroidClass = "com.hypurchase.playstorepurchase.HYPurchase";
        private readonly AndroidJavaObject androidJavaObject;
        private readonly AndroidCallbackListener callbackListener;

        public AndroidPurchase()
        {
            callbackListener = new AndroidCallbackListener();
            androidJavaObject = new AndroidJavaObject(AndroidClass);
        }

        public void Init(Action<OnConnectComplete> onConnectComplete)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");

            callbackListener.SetConnectCallback(onConnectComplete);
            androidJavaObject.Call("init", callbackListener, activity);
            androidJavaObject.Call("connect");
        }

        public void Refresh(List<string> productIdList, Action<OnRefreshComplete> onRefreshComplete)
        {
            androidJavaObject.Call("refresh", productIdList.ToArray());
            callbackListener.SetRefreshCallback(onRefreshComplete);
        }

        public void Purchase(string productId, string developerPayload, Action<OnPurchaseComplete> onPurchaseComplete)
        {
            androidJavaObject.Call("purchase", productId, developerPayload);
            callbackListener.SetPurchaseCallback(onPurchaseComplete);
        }

        public void Consume(string productId, Action<OnConsumeComplete> onConsumeComplete)
        {
            androidJavaObject.Call("consume", productId);
            callbackListener.SetConsumeCallback(onConsumeComplete);
        }

        public void Restore(string productId, Action<OnRestoreComplete> onRestoreComplete)
        {
            androidJavaObject.Call("restore", productId);
            callbackListener.SetRestoreCallback(onRestoreComplete);
        }

        public void Dispose()
        {
            //Dispose
        }
    }
}

