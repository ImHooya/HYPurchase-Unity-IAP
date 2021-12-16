using System;
using System.Collections.Generic;
namespace HYPurchase
{
    public interface IHYPlatformPurchase : IDisposable
    {
        void Init(Action<OnConnectComplete> onConnectComplete);
        void Refresh(List<string> productIdList, Action<OnRefreshComplete> onRefreshComplete);
        void Purchase(string productId, string developerPayload, Action<OnPurchaseComplete> onPurchaseComplete);
        void Consume(string productId, Action<OnConsumeComplete> onConsumeComplete);
        void Restore(string productId, Action<OnRestoreComplete> onRestoreComplete);
    }
}

